using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class BossStateCharging : AttackState
{
    enum SubState
    {
        GettingInRange,
        WindUp,
        Charging,
        Swiping,
        WindDown
    }

    SubState state;

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

    //TODO: Change the substate switching to animation events

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
        BossEventsHandler.current.GetInRange();

        state = SubState.GettingInRange;

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
        state = SubState.WindUp;
        BossEventsHandler.current.ChargeWindUp();

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
        BossEventsHandler.current.ChargeStart();

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
        BossEventsHandler.current.ChargeSwipe();
        chargesCompleted++;
        state = SubState.Swiping;

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
        state = SubState.WindDown;
        BossEventsHandler.current.ChargeWindDown();
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
