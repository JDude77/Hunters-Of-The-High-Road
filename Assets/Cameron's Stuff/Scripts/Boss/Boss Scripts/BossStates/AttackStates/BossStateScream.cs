using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BossStateScream : AttackState
{
    [Space(10)]
    [Header("Swipe settings")]
    [SerializeField] private float radius;
    [Space(10)]
    [Header("Sounds")]
    [SerializeField] private AK.Wwise.Event screamNoise;
    [Header("Animation Names")]
    [SerializeField] private string animationName;

    public void Start()
    {
        base.Start();
        eventResponder.AddAnimation("Scream", animationName, false);
        eventResponder.AddSoundEffect("SlashNoise", screamNoise, gameObject);
        eventResponder.AddAction("AttackEnd", boss.ReturnToMainState);
        eventResponder.AddAction("DamageCheck", DoSphereCast);
    }

    public override void OnEnter()
    {
        base.OnEnter();
        eventResponder.ActivateAnimation("Scream");
    }//End OnEnter

    public override void OnExit()
    {
        base.OnExit();
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

    [ContextMenu("Fill default values")]    
    public override void SetDefaultValues()
    {
        radius = 6f;
        animationName = "Boss_Scream";
    }
}
