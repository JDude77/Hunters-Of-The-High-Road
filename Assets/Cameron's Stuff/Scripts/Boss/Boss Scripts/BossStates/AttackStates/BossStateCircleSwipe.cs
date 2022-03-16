using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BossStateCircleSwipe : AttackState
{
    [Space(10)]
    [Header("Swipe settings")]
    [SerializeField] private float radius;
    [Space(10)]
    [SerializeField] private float windUpTime;
    [SerializeField] private float windDownTime;

    public void Start()
    {
        base.Start();
    }

    public override void OnEnter()
    {
        base.OnEnter();
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
