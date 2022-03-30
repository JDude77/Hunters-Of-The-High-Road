using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStateLandsRoots : BossStatePillarAttack
{
    private Vector3 previousPosition;
    private Vector3 playerVelocity;

    public void Start()
    {
        base.Start();
        previousPosition = player.transform.position;
        playerVelocity = Vector3.zero;
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
        //Start the attack
        StartCoroutine(DoAttack());
    }//End OnEnter


    public override void OnExit()
    {
        base.OnExit();
        //Make sure the attack coroutine is ended
        StopCoroutine(DoAttack());
    }//End OnExit


    IEnumerator DoAttack()
    {
        //Wait for the initial delay
        yield return new WaitForSeconds(windUpTime);
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
