using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class BossStateLandsRoots : BossStatePillarAttack
{
    [Space(5)]
    [SerializeField] private bool addRandomWaitTime;
    [Tooltip("Random amount that is added to 'delayBetweenPillars'. Amount added is between 0 and this maximum range")]
    [SerializeField] private float randomWaitTimeRange;

    #region Sounds
    [Header("Sounds")]
    [SerializeField] private AK.Wwise.Event windUp;
    [SerializeField] private AK.Wwise.Event handInGround;
    [SerializeField] private AK.Wwise.Event windDown;
    #endregion

    #region Anim Event Params
    [Space(10f)]
    [SerializeField] [ReadOnlyProperty] private string[] AnimationEventParameters = new string[] { "StartPillars", "ExitState" };
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
        while (spawnedPillars < pillarCount)
        {
            Vector3 predictedPosition = player.transform.position + (playerVelocity * pillarWaitTime);
            SpawnPillar(predictedPosition);

            if (addRandomWaitTime)
            {
                yield return new WaitForSeconds(delayBetweenPillars + Random.Range(0, randomWaitTimeRange));
            }

            yield return new WaitForSeconds(delayBetweenPillars);
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
