using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BossStatePillarAttack : AttackState
{ 
    [Header("Pillar settings")]
    [SerializeField] protected Pillar pillar = null;
    [Space(10)]
    [Tooltip("Total pillars that should spawn during the attack")]
    [SerializeField] protected int pillarCount;
    [Space(10)]
    [Tooltip("If true, the attack will stop once the player has been hit by a pillar")]
    [SerializeField] private bool stopAttackOnHit;
    [Space(10)]
    [Tooltip("The boss's wait time before attacking")]
    [SerializeField] protected float windUpTime;
    [Tooltip("The base amount of time between each pillar being spawned")]
    [SerializeField] protected float delayBetweenPillars;
    [Space(10)]
    [Tooltip("The amount of time each pillar will wait before rising up")]
    [SerializeField] protected float pillarWaitTime;
    [Tooltip("Speed that the pillar rises")]
    [SerializeField] protected float pillarAscendSpeed;
    [Tooltip("Speed that the pillar falls")]
    [SerializeField] protected float pillarDescendSpeed;
    [Tooltip("Amount of time that the pillar will stay at it's highest point")]
    [SerializeField] protected float pillarStayTime;
    [Space(10)]
    [Tooltip("The position of the pillar under the ground")]
    [SerializeField] protected float startYPos;
    [Tooltip("The position of the pillar at its highest point")]
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

    protected virtual void SpawnPillar(Vector3 startPos, Quaternion rotation)
    {
        //Set the pillar's start position to the predicted position of the player
        Vector3 spawnPos = startPos;
        spawnPos.y = startYPos;

        //Set the pillar's end position
        Vector3 endPos = startPos;
        endPos.y = finalYPos;

        Pillar newPillar = Instantiate(pillar, spawnPos, rotation);
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
