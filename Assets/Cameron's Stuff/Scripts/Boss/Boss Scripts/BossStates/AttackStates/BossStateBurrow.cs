using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BossStateBurrow : AttackState
{
    [Header("Particle Prefab")]
    [SerializeField] private BurrowMovement particlesPrefab;
    [Space(10)]

    [Header("Attack Settings")]
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
    public void Awake()
    {
        //BossAnimationEventsHandler.current.OnBurrowDownFinished += BeginBurrowCoroutine;
    }

    public void Start()
    {
        base.Start();
    }//End Start

    public override void OnEnter()
    {
        base.OnEnter();
        //Look at the player's XZ position
        Vector3 newPosition = player.transform.position;
        newPosition.y = transform.position.y; 
        StartCoroutine(StartBurrow());
        //Start animation 
    } //End OnEnter

    public override void OnExit()
    {
        base.OnExit();
        StopAllCoroutines();
    } //End OnExit

    private void BeginBurrowCoroutine()
    {
        StartCoroutine(StartBurrow());
    }

    IEnumerator StartBurrow()
    {
        InvokeEvent(BossEvent.WindUp);
        yield return new WaitForSeconds(1f);
        InvokeEvent(BossEvent.PrimaryAttackStart);

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

        InvokeEvent(BossEvent.PrimaryAttackEnd);
        yield return new WaitForSeconds(digUpDelay);
        InvokeEvent(BossEvent.WindDown);

        //Reset the boss position
        transform.position = new Vector3(particlePos.x, goundedYPosition, particlePos.z);
        //TODO PLAY ANIMATION
        boss.ChangeState(Boss.State.Idle);
    }
}
