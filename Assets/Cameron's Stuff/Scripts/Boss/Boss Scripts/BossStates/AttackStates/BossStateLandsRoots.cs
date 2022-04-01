using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStateLandsRoots : BossStatePillarAttack
{
    #region Sounds
    [Header("Sounds")]
    [SerializeField] private AK.Wwise.Event windUp;
    [SerializeField] private AK.Wwise.Event handInGround;
    [SerializeField] private AK.Wwise.Event windDown;
    [Header("Animation Names")]
    [SerializeField] private string startAnimation;
    [SerializeField] private string loopAnimation;
    [SerializeField] private string exitAnimation;
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
        boss.animator.Play("LandsRootsStart");
    }//End OnEnter


    public override void OnExit()
    {
        base.OnExit();
        //Make sure the attack coroutine is ended
        StopCoroutine(DoAttack());
    }//End OnExit


    IEnumerator DoAttack()
    {
        boss.animator.Play("LandsRootsLoop");
        //Spawn a new pillar if we're under the cound
        while (spawnedPillars < pillarCount)
        {
            Vector3 predictedPosition = player.transform.position + (playerVelocity * pillarWaitTime);
            SpawnPillar(predictedPosition);
            yield return new WaitForSeconds(delayBetweenPillars);
        }
        boss.animator.Play("LandsRootsExit");
        windDown.Post(gameObject);
    }//End DoAttack

    [ContextMenu("Fill Default Values")]
    public override void SetDefaultValues()
    {
        startAnimation = "Boss_Lands_Roots";
        loopAnimation = "Boss_Lands_Roots_Loop";
        exitAnimation = "Boss_Lands_RootsExiting";
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
        eventResponder.AddAction("ExitState", boss.ReturnToMainState);
    }
}
