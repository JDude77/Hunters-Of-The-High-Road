using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BossStateLandsRoots : BossStatePillarAttack
{
    private Vector3 previousPosition;
    private Vector3 playerVelocity;
    [Space(10)]
    [SerializeField] private AK.Wwise.Event windUp;
    [SerializeField] private AK.Wwise.Event handInGround;
    [SerializeField] private AK.Wwise.Event pillarSound;

    public void Start()
    {
        base.Start();
        previousPosition = player.transform.position;
        playerVelocity = Vector3.zero;

        eventResponder.AddSoundEffect("WindUpSound", windUp, gameObject);
        eventResponder.AddAction("StartAttack", () => { StartCoroutine(DoAttack()); });
        eventResponder.AddAnimation("LandsRootsStart", "Boss_Lands_Roots", false);
        eventResponder.AddAnimation("LandsRootsLoop", "Boss_Lands_Roots_Loop", false);
        eventResponder.AddAnimation("LandsRootsExit", "Boss_Lands_Roots_Exiting", false);
    }//End Start


    public override void FixedRun()
    {
        base.FixedRun();
        //Get the speed of the player based on the players current and previous position
        playerVelocity = (player.transform.position - previousPosition) / Time.deltaTime;
        previousPosition = player.transform.position;
    }//End FixedRun

    public override void OnEnter()
    {
        base.OnEnter();
        previousPosition = player.transform.position;
        eventResponder.Activate("WindUpSound");
    }//End OnEnter


    public override void OnExit()
    {
        base.OnExit();
        //Make sure the attack coroutine is ended
        StopCoroutine(DoAttack());
    }//End OnExit


    IEnumerator DoAttack()
    {
        //Spawn a new pillar if we're under the cound
        while (spawnedPillars < pillarCount)
        {
            Vector3 predictedPosition = player.transform.position + (playerVelocity * pillarWaitTime);
            SpawnPillar(predictedPosition);
            yield return new WaitForSeconds(delayBetweenPillars);
        }
        //Change state back to idle
        boss.ReturnToMainState();
    }//End DoAttack

}
