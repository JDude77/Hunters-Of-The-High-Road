using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class BossStateLandsRoots : BossStatePillarAttack
{
    [Space(5)]
    [Tooltip("If true, each pillar will wait for a random amount of time between 'randomWaitTimeMin' and 'randomWaitTimeMax'")]
    [SerializeField] private bool randomDelay;
    [Tooltip("A random value is then picked using these values as the range")]
    [SerializeField] private float randomDelayMax;
    [SerializeField] private float randomDelayMin;

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
        while (spawnedPillars < pillarCount)
        {
            Vector3 predictedPosition = player.transform.position + (playerVelocity * pillarWaitTime);
            SpawnPillar(predictedPosition, transform.rotation);

            if (randomDelay)
            {
                float time = Random.Range(randomDelayMax, randomDelayMin);
                yield return new WaitForSeconds(time);
            }//End if

            yield return new WaitForSeconds(delayBetweenPillars);
        }//End while

        boss.animator.SetTrigger("DoEndLandsRoots");
        windDown.Post(gameObject);
    }//End DoAttack

    private void InitEvents()
    {
        //Animation event
        eventResponder.AddAction("StartPillars", () => { StartCoroutine(DoAttack()); });
        eventResponder.AddSoundEffect("HandSound", handInGround, gameObject);
        eventResponder.AddAction("ExitState", boss.ReturnToMainState);
    }//End InitEvents
}
