using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class ChargeEventResponse : EventResponse
{
    public BossEvent.ChargeEvents eventName;

    public override string GetEventName()
    {
        Debug.Log(eventName.ToString());
        return eventName.ToString();
    }
}

public class BossStateCharging : AttackState
{
    [Space(10)]
    [SerializeField] public List<ChargeEventResponse> eventResponses;

    [Header("Charge settings")]
    [Tooltip("The distance to the player that the boss will run to and start winding up")]
    [SerializeField] private float inRangeDistance;
    [Tooltip("An offset from the charge's end point so that the boss does not go the full distance. Preferably set to half the radius of the swipe attack circle")]
    [SerializeField] private float stopDistance;
    [SerializeField]  public float runSpeed;
    [Space(5)]
    [SerializeField] private float windUpTime;
    [SerializeField] private float consecutiveWindUpTime;
    [Space(5)] 
    [SerializeField] private float chargeSpeed;
    [SerializeField] private int totalConsecutiveCharges;
    [Space(5)]
    [SerializeField] private float swipeRadius;
    [Tooltip("If this is checked, the damage check for the player will only be done on the animation event")]
    [SerializeField] private bool damageCheckOnAnimation;
    private Vector3 chargePoint;
    private Coroutine currentCoroutine;
    private int chargesCompleted;

    private void Awake()
    {

    }

    private void Start()
    {
        //Add all the event responses to the response dictionary
        boss.eventResponder.InitResponses(eventResponses);
    }

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

        Vector3 distanceToPlayer = player.transform.position - transform.position;

        //While we're out of range
        while (distanceToPlayer.magnitude >= inRangeDistance)
        {
            transform.LookAt(player.transform.position);
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
    } //End GetInRange

    //Selects the position to charge to and waits for X seconds
    IEnumerator WindUp(float windTime)
    {

        transform.LookAt(player.transform.position);

        //Set the point that the boss should charge to
        chargePoint = player.transform.position;
        chargePoint.y = transform.position.y;

        yield return new WaitForSeconds(windTime);

        ChangeCoroutineTo(Charge());
    } //End WindUp

    //Moves to the selected position
    IEnumerator Charge()
    {
        boss.eventResponder.Respond(BossEvent.ChargeEvents.ChargeStart.ToString());

        Debug.Log("Charging");
        //Convert the stopping distance to a percentage of the distance to cover
        float chargeDistanceOffsetPercent = stopDistance / (chargePoint - transform.position).magnitude;
        //Reset the charge point to account for the stopping distance
        chargePoint = Vector3.Lerp(transform.position, chargePoint, 1f - chargeDistanceOffsetPercent);
        //Check if we're not at the desired position
        while(transform.position != chargePoint)
        {
            //Charge to the charge point
            transform.position = Vector3.MoveTowards(transform.position, chargePoint, chargeSpeed * Time.deltaTime);
            yield return null;
        }
        
        ChangeCoroutineTo(Swipe());
    } //End Charge

    //Checks if the player is within range of the attack
    IEnumerator Swipe()
    {
        chargesCompleted++;

        //Check if the boss has hit the player
        if (DoSphereCast())
        {
            BossEventsHandler.current.HitPlayer(GetDamageValue());
        }
        else if(chargesCompleted < totalConsecutiveCharges)
        { 
            //charge again
            ChangeCoroutineTo(WindUp(consecutiveWindUpTime));
        }//End else if

        yield return new WaitForSeconds(1.0f);

        ChangeCoroutineTo(WindDown());

    } //End Swipe

    IEnumerator WindDown()
    {
        yield return new WaitForSeconds(1.0f);
        boss.ChangeState(Boss.State.Idle);
    } //End WindDown

    public bool DoSphereCast()
    {
        //Set the center of the sphere to the boss' position + the radius of the sphere
        Vector3 sphereCenter = transform.forward * swipeRadius + transform.position;
        //Store the intersections with the 'Player' layer
        Collider[] collisions = Physics.OverlapSphere(sphereCenter, swipeRadius, boss.attackLayer);
        //Check if there was a collision
        if (collisions.Length > 0)
            return true;
        
        return false;
    }

    private void OnDrawGizmos()
    {
        Vector3 sphereCenter = transform.forward * swipeRadius + transform.position;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(sphereCenter, swipeRadius);
    }
} 
