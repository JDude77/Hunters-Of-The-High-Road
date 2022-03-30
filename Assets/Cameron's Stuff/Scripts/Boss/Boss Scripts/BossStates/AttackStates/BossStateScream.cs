using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BossStateScream: AttackState
{
    [Space(10)]
    [Header("Swipe settings")]
    [SerializeField] private float radius;
    [Space(10)]
    [SerializeField] private float windUpTime;
    [SerializeField] private float windDownTime;
    [SerializeField] private AK.Wwise.Event slashNoise;

    public void Start()
    {
        base.Start();
        eventResponder.AddAnimation("Animation", "Boss_Slash", false);
        eventResponder.AddSoundEffect("SlashNoise", slashNoise, gameObject);
        eventResponder.AddAction("AttackEnd", boss.ReturnToMainState);
    }

    public override void OnEnter()
    {
        base.OnEnter();
        eventResponder.Activate("Animation");
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
