using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BossStatePillarAttack : AttackState
{ 
    [Header("Pillar settings")]
    [SerializeField] protected Pillar pillar = null;
    [Space(10)]
    [SerializeField] protected int pillarCount;
    [Space(10)]
    [SerializeField] private bool stopAttackOnHit;
    [Space(10)]
    [SerializeField] protected float windUpTime;
    [SerializeField] protected float delayBetweenPillars;
    [Space(10)]
    [SerializeField] protected float pillarWaitTime;
    [SerializeField] protected float pillarAscendSpeed;
    [SerializeField] protected float pillarDescendSpeed;
    [SerializeField] protected float pillarStayTime;
    [Space(10)]
    [SerializeField] protected float startYPos;
    [SerializeField] protected float finalYPos;

    protected int spawnedPillars;

    public override void OnEnter()
    {        
        base.OnEnter();
        spawnedPillars = 0;
    }//End OnEnter

    public override void OnExit()
    {
        base.OnExit();
    }//End OnExit

    protected virtual void SpawnPillar(Vector3 startPos)
    {
        //Set the pillar's start position to the predicted position of the player
        Vector3 spawnPos = startPos;
        spawnPos.y = startYPos;

        //Set the pillar's end position
        Vector3 endPos = startPos;
        endPos.y = finalYPos;

        Pillar newPillar = Instantiate(pillar, spawnPos, Quaternion.identity);
        newPillar.hitPlayer += OnPillarHit;
        //Initialise the pillar
        newPillar.Initialise(pillarWaitTime, pillarAscendSpeed, pillarDescendSpeed, pillarStayTime, spawnPos, endPos);
        spawnedPillars++;
    }//End SpawnPillar

    protected void OnPillarHit()
    {
        //Stop spawning pillars
        if (stopAttackOnHit)
            spawnedPillars = pillarCount;
        //Call the hitplayer event
        BossEventsHandler.current.HitPlayer(GetDamageValue());
    }//End OnPillarHit
}
