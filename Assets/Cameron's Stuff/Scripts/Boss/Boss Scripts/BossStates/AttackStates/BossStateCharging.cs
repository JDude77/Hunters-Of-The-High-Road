using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class BossStateCharging : AttackState
{
    #region Attack Variables
    [Header("Charge settings")]
    [Tooltip("The distance to the player that the boss will run to and start winding up")]
    [SerializeField] private float inRangeDistance;
    [SerializeField] private float rotateSpeed;
    [Tooltip("An offset from the charge's end point so that the boss does not go the full distance. Preferably set to half the radius of the swipe attack circle")]
    [SerializeField] private float stopDistance;
    [SerializeField] public float runSpeed;
    [Space(5)]
    [SerializeField] private float windUpTime;
    [SerializeField] private float windDownTime;
    [SerializeField] private float consecutiveWindUpTime;
    [Space(5)] 
    [SerializeField] private float maxChargeSpeed;

    [SerializeField] private float accelerateTime;
    private float speedMultiplier = 0;
    private float accelerateTimer;

    [SerializeField] private AnimationCurve accelerationCurve;

    [SerializeField] private int totalConsecutiveCharges;
    [Space(5)]
    [SerializeField] private float swipeRadius;
    #endregion

    #region Sounds
    [Header("Sounds")]
    [SerializeField] private AK.Wwise.Event windUpSound;
    [SerializeField] private AK.Wwise.Event lightFootstepSound;
    [SerializeField] private AK.Wwise.Event heavyFootstepSound;
    [SerializeField] private AK.Wwise.Event swipeSound;
    [SerializeField] private AK.Wwise.Event hitSound;    
    #endregion

    #region Anim Event Params
    [Space(10f)]
    [SerializeField] [ReadOnlyProperty] private string[] AnimationEventParameters = new string[] { "LightFootStepSound", "HeavyFootStepSound", "SwipeSound","DamageCheck", "SwipeEnd" };
    #endregion

    enum SubState
    {
        WindUp,
        GetInRange,
        Charge,
        Swipe,
        WindDown
    }

    SubState state;
    private Vector3 chargePoint;
    private Coroutine currentCoroutine;
    private int chargesCompleted;

    //TODO: Set up animation blend tree for boss running

    public void Start()
    {
        base.Start();     
        InitEvents();

    }//End Start

    public override void OnEnter()
    {
        base.OnEnter();
        chargesCompleted = 0;
        ChangeCoroutineTo(GetInRange());
    }//End OnEnter

    public override void OnExit()
    {
        base.OnExit();
        StopAllCoroutines();
    } //End OnExit

    public override void Run()
    {
        base.Run();
        if(state == SubState.GetInRange || state == SubState.WindUp)
        {
            Vector3 playerXZ = player.transform.position;
            playerXZ.y = transform.position.y;

            Vector3 targetDir = playerXZ - transform.position;

            Quaternion targetRot = Quaternion.LookRotation(targetDir, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, 360f * Time.deltaTime);
        }
    }//End Run

    public override void FixedRun()
    {
        base.FixedRun();
    }

    //Stops the current coroutine if one is running before starting the new one
    void ChangeCoroutineTo(IEnumerator newCoroutine)
    {
        //Stop the current coroutine
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }

        currentCoroutine = StartCoroutine(newCoroutine);
    } //End ChangeCoroutine

    //Moves the boss closer to the player
    IEnumerator GetInRange()
    {
        print("GIR");
        state = SubState.GetInRange;
        boss.animator.SetTrigger("DoRun");
        Vector3 distanceToPlayer = player.transform.position - transform.position;

        //While we're out of range
        while (distanceToPlayer.magnitude >= inRangeDistance)
        {
            //Target the player's X and Z position
            Vector3 targetPosition = player.transform.position;
            targetPosition.y = transform.position.y;
            //Set the boss's speed
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, runSpeed * Time.deltaTime);
            //Get the vector from the boss to the player
            distanceToPlayer = player.transform.position - transform.position;
            yield return null;
        }

        //Start the wind up
        ChangeCoroutineTo(WindUp(windUpTime));

    }//End GetInRange

    //Selects the position to charge to and waits for X seconds
    IEnumerator WindUp(float windTime)
    {
        print("WindUp");
        state = SubState.WindUp;

        boss.animator.SetTrigger("DoReturnToIdle");

        //Play the windup sound
        windUpSound.Post(gameObject);

        yield return new WaitForSeconds(windTime);

        //Set the point that the boss should charge to
        chargePoint = player.transform.position;
        chargePoint.y = transform.position.y;

        ChangeCoroutineTo(Charge());
    } //End WindUp

    //Moves to the selected position
    IEnumerator Charge()
    {
        state = SubState.Charge;

        boss.animator.SetTrigger("DoRun");

        accelerateTimer = 0f;
        speedMultiplier = accelerationCurve.Evaluate(0.0f);

        print("Charge");
        //Convert the stopping distance to a percentage of the distance to cover
        float chargeDistanceOffsetPercent = stopDistance / (chargePoint - transform.position).magnitude;
        //Reset the charge point to account for the stopping distance
        chargePoint = Vector3.Lerp(transform.position, chargePoint, 1f - chargeDistanceOffsetPercent);

        //Check if we're not at the desired position
        while(transform.position != chargePoint)
        {
            //if (speedMultiplier < 1f)
            //{
            //    accelerateTimer += Time.deltaTime * accelerateTime;
            //    speedMultiplier = accelerationCurve.Evaluate(accelerateTimer / accelerateTime);
            //    boss.animator.speed = speedMultiplier;
            //}

            //Charge to the charge point
            transform.position = Vector3.MoveTowards(transform.position, chargePoint, maxChargeSpeed * Time.deltaTime);// * speedMultiplier);
            yield return null;
        }

        chargesCompleted++;
        Swipe();

    } //End Charge

    IEnumerator WindDown()
    {
        state = SubState.WindDown;
        yield return new WaitForSeconds(windDownTime);   
    } //End WindDown


    //Checks if the player is within range of the attack
    void Swipe()
    {
        state = SubState.Swipe;
        boss.animator.SetTrigger("DoSlash");
    } //End Swipe

    //Animator function called on final swipe frame
    void CheckChargeCondition()
    {
        if(chargesCompleted < totalConsecutiveCharges)
        {
            ChangeCoroutineTo(WindUp(consecutiveWindUpTime));
        }
        else
        {
            boss.ReturnToMainState();
        }
    }//End CheckChargeCondition

    public void DoSphereCast()
    {
        //Set the center of the sphere to the boss' position + the radius of the sphere
        Vector3 sphereCenter = transform.forward * swipeRadius + transform.position;
        //Store the intersections with the 'Player' layer
        Collider[] collisions = Physics.OverlapSphere(sphereCenter, swipeRadius, boss.attackLayer);
        //Check if there was a collision
        if (collisions.Length > 0)
        {
            print("Hit player");
            hitSound.Post(gameObject);
            BossEventsHandler.current.HitPlayer(GetDamageValue());
            chargesCompleted = totalConsecutiveCharges;
        }
    }//End DoSphereCast

    private void OnDrawGizmos()
    {
        Vector3 sphereCenter = transform.forward * swipeRadius + transform.position;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(sphereCenter, swipeRadius);
    }

    [ContextMenu("Fill default values")]
    public override void SetDefaultValues()
    {
        inRangeDistance = 15f;
        stopDistance = 3f;
        runSpeed = 15f;
        windUpTime = 1f;
        windDownTime = 1f;
        consecutiveWindUpTime = 0.8f;
        maxChargeSpeed = 30f;
        totalConsecutiveCharges = 3;
        swipeRadius = 2f;
    }//End SetDefaultValues

    private void InitEvents()
    {
        //Animation event sounds
        eventResponder.AddSoundEffect("LightFootStepSound", lightFootstepSound, gameObject);
        eventResponder.AddSoundEffect("HeavyFootStepSound", heavyFootstepSound, gameObject);
        eventResponder.AddSoundEffect("SwipeSound", swipeSound, gameObject);
        eventResponder.AddAction("DamageCheck", DoSphereCast);
        eventResponder.AddAction("SwipeEnd", CheckChargeCondition);
        eventResponder.AddAction("ExitState", boss.ReturnToMainState);
    }//End InitEvents
} 
