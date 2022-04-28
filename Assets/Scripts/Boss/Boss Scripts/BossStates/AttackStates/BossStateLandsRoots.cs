using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class BossStateLandsRoots : BossStatePillarAttack
{
    [Space(5)]
    [Tooltip("If true, each pillar will wait for a random amount of time between 'delayBetweenPillars' as the min and 'randomWaitTimeMaxRange' as the max")]
    [SerializeField] private bool addRandomWaitTime;
    [Tooltip("Amount that is added to 'delayBetweenPillars'. A random value is then picked using these values as the range")]
    [SerializeField] private float randomWaitTimeMaxRange;

    #region Sounds
    [Header("Sounds")]
    [SerializeField] private AK.Wwise.Event windUp;
    [SerializeField] private AK.Wwise.Event handInGround;
    [SerializeField] private AK.Wwise.Event windDown;
    #endregion

    #region Anim Event Params
    //[SerializeField] [ReadOnlyProperty] private string[] AnimationEventParameters = new string[] { "StartPillars", "ExitState" };
    #endregion

    private Vector3 previousPosition;
    private Vector3 playerVelocity;

    public void Start()
    {
        base.Start();
        previousPosition = player.transform.position;
        playerVelocity = Vector3.zero;        

        InitEvents();
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
        canBeStunned = true;
        previousPosition = player.transform.position;
        windUp.Post(gameObject);        

        boss.animator.SetTrigger("DoLandsRoots");
        
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
        while (base.spawnedPillars < base.pillarCount)
        {
            Vector3 predictedPosition = player.transform.position + (playerVelocity * base.pillarWaitTime);
            SpawnPillar(predictedPosition, transform.rotation);

            if (addRandomWaitTime)
            {
                yield return new WaitForSeconds(base.delayBetweenPillars + Random.Range(0, randomWaitTimeMaxRange));
            }

            yield return new WaitForSeconds(base.delayBetweenPillars);
        }
        boss.animator.SetTrigger("DoEndLandsRoots");
        windDown.Post(gameObject);
    }//End DoAttack

    [ContextMenu("Fill Default Values")]
    public override void SetDefaultValues()
    {
        pillarCount = 5;
        windUpTime = 1f;
        delayBetweenPillars = 0.8f;
        pillarWaitTime = 0.8f;
        pillarAscendSpeed = 20f;
        pillarDescendSpeed = 10f;
        pillarStayTime = 0.5f;
        startYPos = 5f;
        finalYPos = 9f;
    }

    private void InitEvents()
    {
        //Animation event
        eventResponder.AddAction("StartPillars", () => { StartCoroutine(DoAttack()); });
        eventResponder.AddSoundEffect("HandSound", handInGround, gameObject);
        eventResponder.AddAction("ExitState", boss.ReturnToMainState);
    }
}
