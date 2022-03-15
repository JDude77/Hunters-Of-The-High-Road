using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SwipeEventResponse : EventResponse
{
    public BossEvent.CircleSwipe eventName;

    public override string GetEventName()
    {
        Debug.Log(eventName.ToString());
        return eventName.ToString();
    }
}

public class BossStateCircleSwipe : AttackState
{
    [Space(10)]
    [SerializeField] List<SwipeEventResponse> eventResponses;
    [Space(10)]
    [Header("Swipe settings")]
    [SerializeField] private float radius;
    [Space(10)]
    [SerializeField] private float windUpTime;
    [SerializeField] private float windDownTime;

    public void Start()
    {
        base.Start();
        //Add all the event responses to the response dictionary
        boss.eventResponder.InitResponses(eventResponses);
    }

    public override void OnEnter()
    {
        base.OnEnter();
        //TODO subscribe 'DoSphereCast' to animation event
        boss.eventResponder.Respond(BossEvent.CircleSwipe.Attack.ToString());
        boss.ChangeState(Boss.State.Idle);
    }//End OnEnter

    public override void OnExit()
    {
        base.OnExit();
        //TODO unsubscribe 'DoSphereCast' from animation event
    }//End OnExit

    public void DoSphereCast()
    {
        //Store the intersections with the 'Player' layer
        Collider[] collisions = Physics.OverlapSphere(transform.position, radius, boss.attackLayer);

        if (collisions.Length > 0)
            BossEventsHandler.current.HitPlayer(GetDamageValue());
    }//End DoSphereCast

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }//End OnDrawGizmos
}
