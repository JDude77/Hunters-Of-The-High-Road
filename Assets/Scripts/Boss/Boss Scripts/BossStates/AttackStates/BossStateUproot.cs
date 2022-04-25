using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class BossStateUproot : BossStatePillarAttack
{
    [Header("Uproot settings")]
    [Tooltip("Distance to the final pillar that will be spawned")]
    [SerializeField] private float attackDistance;
    [Tooltip("Position of the first pillar that will be spawned")]
    [SerializeField] private float attackStartOffset;
    [Tooltip("The sound of the boss stomping - For pillar sound effects, see 'URPillar' prefab")]
    [SerializeField] private AK.Wwise.Event stomp;

    //Used for initialising uproot trigger box
    [HideInInspector] public float rangeEnd { 
        get { 
            return attackDistance; 
        } 
    }
    //Used for initialising uproot trigger box
    [HideInInspector] public float rangeStart { 
        get { 
            return attackStartOffset; 
        } 
    }

    private Vector3 startPosition;
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        InitEvents();
    }//End Start

    public override void OnEnter()
    {
        base.OnEnter();
        StartCoroutine(StartWindUp());
    }//End OnEnter

    public override void OnExit()
    {
        base.OnExit();
        StopAllCoroutines();
    }//End OnExit

    IEnumerator StartWindUp()
    {
        float timer = 0.0f;
        while (timer < windUpTime)
        {
            //Update the timer
            timer += Time.deltaTime;
            //Get the player's X and Z position
            Vector3 playerXZ = player.transform.position;
            playerXZ.y = transform.position.y;
            //Get the direction to that position
            Vector3 targetDir = playerXZ - transform.position;
            //Rotate towards that direction
            Quaternion targetRot = Quaternion.LookRotation(targetDir, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, 360f * Time.deltaTime);
            yield return null;
        }
        //Set the start position of the attack
        startPosition = transform.position + attackStartOffset * transform.forward;
        boss.animator.SetTrigger("DoStomp");        
    }//End StartWindUp

    IEnumerator DoAttack()
    {
        //While there are pillars to be spawned
        while (base.spawnedPillars < base.pillarCount)
        {
            Vector3 position = startPosition + transform.forward * (attackDistance * base.spawnedPillars / base.pillarCount);

            SpawnPillar(position);
            //Wait
            yield return new WaitForSeconds(base.delayBetweenPillars);
        }
        //Change state back to idle
        boss.ReturnToMainState();
    }

    private void OnDrawGizmos()
    {
        Vector3 direction = transform.forward * attackDistance;
        Vector3 start = transform.position + attackStartOffset * transform.forward;
        Gizmos.DrawRay(start, direction);
    }//End OnDrawGizmos

    private void InitEvents()
    {
        eventResponder.AddSoundEffect("StompSound", stomp, gameObject);
        eventResponder.AddAction("DoAttack", () => StartCoroutine(DoAttack()));
    }//End InitEvents
}
