using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BossStateBurrow : AttackState
{
    #region Attack Variables
    [Header("Particle Prefab")]
    [SerializeField] private BurrowMovement particlesPrefab;

    [Header("Attack Settings")]
    [SerializeField] private float windUpTime;
    [Tooltip("This should be the y position of the boss when it plays its dig up and dig down animations")]
    [SerializeField] private float goundedYPosition;
    [Tooltip("The position of the boss as the particles are moving")]
    [SerializeField] private float burrowYPosition;
    [SerializeField] private float particleYPosition;
    [Tooltip("Higher max rotation speed means the boss will align itself with the direction to the player faster")]
    [SerializeField] private float maxRotationSpeed;
    [SerializeField] private float burrowSpeed;
    [Tooltip("The distance to the player that the particles must be to activate the dig up animation")]
    [SerializeField] private float inRangeDistance;
    [SerializeField] private float digUpDelay;
    #endregion

    #region Sound & Animation
    [Header("Sounds")]
    [SerializeField] private AK.Wwise.Event digDownSound;
    [SerializeField] private AK.Wwise.Event digToPlayerSound;
    [SerializeField] private AK.Wwise.Event digUpSound;

    [Header("Animation Names")]
    [SerializeField] private string digUpAnimation;
    [SerializeField] private string digDownAnimation;

    [Header("Available animation event parameters")]
    [SerializeField] [ReadOnly] private string digDownSoundKey = "DigDownSound";
    [SerializeField] [ReadOnly] private string digUpSoundKey = "DigUpSound";

    private string digToPlayerKey = "DigToPlayer", digUpKey = "DigUp", digDownKey = "DigDown";
    #endregion

    public void Start()
    {
        base.Start();
        InitEvents();
    }//End Start

    public override void OnEnter()
    {
        base.OnEnter();
        //Look at the player's XZ position
        Vector3 newPosition = player.transform.position;
        newPosition.y = transform.position.y; 
        StartCoroutine(StartBurrow());
    } //End OnEnter

    public override void OnExit()
    {
        base.OnExit();
        StopAllCoroutines();
    } //End OnExit

    IEnumerator StartBurrow()
    {
        yield return new WaitForSeconds(windUpTime);
        eventResponder.ActivateAnimation(digDownKey);

        //Set the y position of the boss after the animation plays
        Vector3 pos = transform.position;
        pos.y = burrowYPosition;
        transform.position = pos;

        //Set the y position to the particle y position
        pos.y = particleYPosition;

        //Spawn particles
        BurrowMovement burrower = Instantiate(particlesPrefab, pos, transform.rotation);
        
        //Set the target position to the player with the same y position as the particles
        Vector3 targetPosition = player.transform.position;
        targetPosition.y = particleYPosition;

        //Initialise the distance to the player
        float distance = (burrower.transform.position - targetPosition).magnitude;
        //Give the particle object the rotation speed and movement speed
        burrower.Init(maxRotationSpeed, burrowSpeed);

        //Update the target position while out of range
        while (distance > inRangeDistance)
        {
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
        eventResponder.ActivateAnimation(digUpKey);
    }

    [ContextMenu("Fill default values")]
    public override void SetDefaultValues()
    {
        windUpTime = 0f;
        goundedYPosition = 9f;
        burrowYPosition = 1f;
        particleYPosition = 8f;
        maxRotationSpeed = 5f;
        burrowSpeed = 10f;
        inRangeDistance = 1f;
        digUpDelay = 0.5f;

        digUpAnimation = "Boss_Burrow_Start";
        digDownAnimation = "Boss_Jump_Up";
    }//End SetDefaultValues

    void InitEvents()
    {
        //Animation events
        eventResponder.AddSoundEffect(digDownSoundKey, digDownSound, gameObject);
        eventResponder.AddSoundEffect(digUpSoundKey, digUpSound, gameObject);
        //Non-Animation events
        eventResponder.AddSoundEffect(digToPlayerKey, digToPlayerSound, gameObject);

        eventResponder.AddAnimation(digUpKey, digUpAnimation, gameObject);
        eventResponder.AddAnimation(digDownKey, digDownAnimation, gameObject);
    }//End InitEvents
}
