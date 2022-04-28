using System.Collections;
using UnityEngine;

[DisallowMultipleComponent]
public class BossStateBurrow : AttackState
{
    #region Attack Variables
    [Header("Burrow Prefab Settings")]
    [SerializeField] private BurrowMovement burrowMovementPrefab;
    [SerializeField] private float timeBetweenParticleBursts;

    [Header("Attack Settings")]
    [Tooltip("This should be the y position of the boss when it plays its dig up and dig down animations")]
    [SerializeField] private float goundedYPosition;
    [Tooltip("The position of the boss as the particles are moving")]
    [SerializeField] private float burrowYPosition;
    [Tooltip("The position of the particles as they are moving")]
    [SerializeField] private float particleYPosition;
    [Tooltip("Higher max rotation speed means the boss will align itself with the direction to the player faster")]
    [Space(5)]
    [SerializeField] private float startRotationSpeed;
    [Tooltip("Rate at which the rotation speed increases")]
    [SerializeField] private float rotationSpeedIncreaseRate;
    [Space(5)]
    [Tooltip("Forward movement speed of the boss")]
    [SerializeField] private float burrowSpeed;
    [Tooltip("Maximum amount of time the boss will burrow for")]
    [SerializeField] private float burrowTime;
    private float burrowTimer;
    [Space(5)]
    [Tooltip("The distance to the player that the boss must be to activate the dig up animation")]
    [SerializeField] private float inRangeDistance;
    [Space(5)]
    [Tooltip("The time the boss will wait before jumping up")]
    [SerializeField] private float digUpDelay;
    #endregion

    #region Sound & Animation
    [Header("Sounds")]
    [SerializeField] private AK.Wwise.Event digDownSound;
    [SerializeField] private AK.Wwise.Event digToPlayerSound;
    [SerializeField] private AK.Wwise.Event digUpSound;
    #endregion

    #region Anim Event Params
    //[SerializeField] [ReadOnlyProperty] private string[] AnimationEventParameters = new string[] { "DigDownSound", "DigUpSound", "StartBurrow", "ExitAttack" };
    #endregion

    public void Start()
    {
        base.Start();
        InitEvents();
    }//End Start

    public override void OnEnter()
    {
        base.OnEnter();
        burrowTimer = 0;
        //Look at the player's XZ position
        Vector3 newPosition = player.transform.position;
        newPosition.y = transform.position.y;
        boss.animator.SetTrigger("DoBurrowStart");
    } //End OnEnter

    public override void OnExit()
    {
        base.OnExit();
        StopAllCoroutines();
    } //End OnExit

    IEnumerator StartBurrow()
    {
        digDownSound.Post(gameObject);

        //Set the y position of the boss after the animation plays
        Vector3 pos = transform.position;
        pos.y = burrowYPosition;
        transform.position = pos;

        //Set the y position to the particle y position
        pos.y = particleYPosition;

        //Spawn particles
        BurrowMovement burrower = Instantiate(burrowMovementPrefab, pos, transform.rotation);
        
        //Set the target position to the player with the same y position as the particles
        Vector3 targetPosition = player.transform.position;
        targetPosition.y = particleYPosition;

        //Initialise the distance to the player
        float distance = (burrower.transform.position - targetPosition).magnitude;
        //Give the particle object the rotation speed and movement speed
        burrower.Init(startRotationSpeed, rotationSpeedIncreaseRate, burrowSpeed, timeBetweenParticleBursts);

        //Update the target position while out of range
        while (distance > inRangeDistance && burrowTimer < burrowTime)
        {
            burrowTimer += Time.deltaTime;
            digToPlayerSound.Post(gameObject);
            //Update the target position
            targetPosition.x = player.transform.position.x;
            targetPosition.z = player.transform.position.z;
            
            //Update the distance
            distance = (burrower.transform.position - targetPosition).magnitude;
            //Update the target position on the particle object
            burrower.SetPlayerPosition(targetPosition);
            yield return null;
        }

        Vector3 particlePos = burrower.transform.position;
        Destroy(burrower.gameObject);

        yield return new WaitForSeconds(digUpDelay);

        //Reset the boss position
        transform.position = new Vector3(particlePos.x, goundedYPosition, particlePos.z);

        Vector3 newDir = player.transform.position;
        newDir.y = transform.position.y;
        transform.LookAt(newDir);

        boss.animator.SetTrigger("DoJumpUp");
        digUpSound.Post(gameObject);
    }

    [ContextMenu("Fill default values")]
    public override void SetDefaultValues()
    {
        goundedYPosition = 9f;
        burrowYPosition = 1f;
        particleYPosition = 8f;
        startRotationSpeed = 5f;
        burrowSpeed = 10f;
        inRangeDistance = 1f;
        digUpDelay = 0.5f; 
        timeBetweenParticleBursts = 0.5f;
    }//End SetDefaultValues

    void InitEvents()
    {
        //Animation events
        eventResponder.AddSoundEffect("DigDownSound", digDownSound, gameObject);
        eventResponder.AddSoundEffect("DigUpSound", digUpSound, gameObject);
        eventResponder.AddAction("ExitAttack", boss.ReturnToMainState);
        eventResponder.AddAction("StartBurrow", () => StartCoroutine(StartBurrow()));
        eventResponder.AddAction("DisableStun", () => { canBeStunned = false; });
        eventResponder.AddAction("EnableStun", () => { canBeStunned = true; });
    }//End InitEvents
}
