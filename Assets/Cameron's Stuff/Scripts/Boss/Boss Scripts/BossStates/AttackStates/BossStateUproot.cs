using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class BossStateUproot : BossStatePillarAttack
{
    [Header("Uproot settings")]
    [SerializeField] private float attackDistance;
    [SerializeField] private float attackStartOffset;

    [SerializeField] private AK.Wwise.Event stomp;

    [HideInInspector] public float rangeEnd { 
        get { 
            return attackDistance; 
        } 
    }
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
    }

    public override void OnEnter()
    {
        spawnedPillars = 0;
        StartCoroutine(StartWindUp());
    }

    public override void OnExit()
    {
        base.OnExit();
        StopAllCoroutines();
    }

    IEnumerator StartWindUp()
    {
        float timer = 0.0f;
        while (timer < windUpTime)
        {
            timer += Time.deltaTime;

            Vector3 playerXZ = player.transform.position;
            playerXZ.y = transform.position.y;

            Vector3 targetDir = player.transform.position - transform.position;
            Quaternion targetRot = Quaternion.LookRotation(targetDir, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, 360f * Time.deltaTime);
            yield return null;
        }

        startPosition = transform.position + attackStartOffset * transform.forward;
        boss.animator.SetTrigger("DoStomp");        
    }

    IEnumerator DoAttack()
    {
        while (spawnedPillars < pillarCount)
        {
            Vector3 position = startPosition + transform.forward * (attackDistance * spawnedPillars / pillarCount);

            SpawnPillar(position);
            //Wait
            yield return new WaitForSeconds(delayBetweenPillars);
        }
        //Change state back to idle
        boss.ReturnToMainState();
    }

    private void OnDrawGizmos()
    {
        Vector3 direction = transform.forward * attackDistance;
        Vector3 start = transform.position + attackStartOffset * transform.forward;
        Gizmos.DrawRay(start, direction);
    }

    private void InitEvents()
    {
        eventResponder.AddSoundEffect("StompSound", stomp, gameObject);
        eventResponder.AddAction("DoAttack", () => StartCoroutine(DoAttack()));
    }
}
