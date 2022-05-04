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
    [Tooltip("Time the boss will wait AFTER spawning all the pillars before exiting the state")]
    [SerializeField] private float windDownTime;
    [Space(5)]
    [Tooltip("if true, the pillars will be able to rotate towards the player even if the boss has stopped rotating")]
    [SerializeField] private bool rotateAttackSeperately;
    [Tooltip("Maximum speed that the attack can rotate towards the player")]
    [SerializeField] private float attackRotationSpeed;
    [Tooltip("After this time, the pillars will stop rotating, meaning they will move in a straight line")]
    [SerializeField] private float attackRotationTime;
    [Tooltip("The sound of the boss stomping - For pillar sound effects, see 'URPillar' prefab")]
    [SerializeField] private AK.Wwise.Event stomp;
    private Transform attackTransform;
    private bool rotateBoss;
    private bool rotateAttack;
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
        canBeStunned = true;
        rotateBoss = true;
        rotateAttack = true;
        //StartCoroutine(StartWindUp());
        boss.animator.SetTrigger("DoStomp");
    }//End OnEnter

    public override void OnExit()
    {
        base.OnExit();
        StopAllCoroutines();
        rotateBoss = false;
        rotateAttack = false;
        spawnedPillars = pillarCount;
    }//End OnExit

    public override void FixedRun() {
        base.FixedRun();

        if (rotateBoss || rotateAttack) {
            //Get the player's X and Z position
            Vector3 playerXZ = player.transform.position;
            playerXZ.y = transform.position.y;
            //Get the direction to that position
            Vector3 targetDir = playerXZ - transform.position;
            //Rotate towards that direction
            Quaternion targetRot = Quaternion.LookRotation(targetDir, Vector3.up);

            if (rotateBoss) {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, 360f * Time.deltaTime);
                attackTransform = transform;
            }

            if (rotateAttack && rotateAttackSeperately) {
                attackTransform.rotation = Quaternion.RotateTowards(attackTransform.rotation, targetRot, attackRotationSpeed * 360f * Time.deltaTime);
            }
        }
    }

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
        startPosition = transform.position + attackStartOffset * transform.forward;
        boss.animator.SetTrigger("DoStomp");        
    }//End StartWindUp

    IEnumerator DoAttack() 
    {
        StartCoroutine(RotateAttackTimer());
        startPosition = transform.position + attackStartOffset * transform.forward;
        //While there are pillars to be spawned
        while (spawnedPillars < pillarCount)
        {
            Vector3 position = startPosition + attackTransform.forward * (attackDistance * spawnedPillars / pillarCount);

            SpawnPillar(position, attackTransform.rotation);
            //Wait
            yield return new WaitForSeconds(delayBetweenPillars);
        }

        yield return new WaitForSeconds(windDownTime);
        //Change state back to idle
        boss.ReturnToMainState();
    }

    IEnumerator RotateAttackTimer() {
        yield return new WaitForSeconds(attackRotationTime);
        rotateAttack = false;
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
        eventResponder.AddAction("StopRotateBoss", () => { rotateBoss = false; });
        eventResponder.AddAction("StopRotateAttack", () => { rotateAttack = false; });
    }//End InitEvents
}
