using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStateUproot : BossStatePillarAttack
{
    [Header("Uproot settings")]
    [SerializeField] private float attackDistance;
    [SerializeField] private float attackStartOffset;

    [SerializeField] private AK.Wwise.Event stomp;
    [SerializeField] private string stompAnimation;

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
        StartCoroutine(DoAttack());
    }

    public override void OnExit()
    {
        base.OnExit();
        StopCoroutine(DoAttack()); 
    }

    IEnumerator DoAttack()
    {
        transform.LookAt(player.transform.position);
        startPosition = transform.position + attackStartOffset * transform.forward;
        //Wait for windUp
        yield return new WaitForSeconds(windUpTime);

        while(spawnedPillars < pillarCount)
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
        eventResponder.AddAnimation("Stomp", stompAnimation, false);
    }
}
