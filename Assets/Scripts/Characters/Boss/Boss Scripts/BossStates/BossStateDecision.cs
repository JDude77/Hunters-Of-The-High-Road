using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[DisallowMultipleComponent]
public class BossStateDecision : BossState
{
    //Attacks and their activation condition
    private Dictionary<Boss.State, Func<bool>> attackDictionary = new Dictionary<Boss.State, Func<bool>>();
    //List of desired attacks in the inspector
    [SerializeField] private List<Boss.State> allAttacks;
    private List<Boss.State> availableAttackPool = new List<Boss.State>();
    private Queue<Boss.State> recentAttacks = new Queue<Boss.State>();

    [HideInInspector] public List<Boss.State> priorityAttacksInOrder = new List<Boss.State>();

    private Boss.State previousAttack;

    private Vector3 playerCenter;
    private float playerDistance;

    [Header("Attack Ranges")]
    [Tooltip("Attack is an eligible attack if the player is within this range")]
    [SerializeField] private float chargeInRange;
    [SerializeField] private float landsRootsInRange;
    [SerializeField] private float burrowInRange;    
    [SerializeField] private float screamInRange;
    [SerializeField] private float radialUprootInRange;
    [Tooltip("Uproot is an eligible attack if the player is within this area")]
    [SerializeField] private BoxCollider uprootBox;
    [Space(5)]
    [Tooltip("The amount of attacks that can be between two burrow attacks")]
    [SerializeField] private int maxAttacksBetweenBurrows;

    [Header("Decision Delay Settings")]
    [Tooltip("The maximum time the boss will wait before changing to a new attack")]
    [SerializeField] private float maxDecisionTime;
    [Tooltip("The minimum time the boss will wait before changing to a new attack")]
    [SerializeField] private float minDecisionTime;

    private void Awake()
    {        
        base.Awake();
        InitAttacks();

        BoxCollider[] colliders = GetComponentsInChildren<BoxCollider>();
        foreach(BoxCollider col in colliders)
        {
            if(col.name == "UprootBox")
            {
                uprootBox = col;
                break;
            }//End if
        }//End foreach

        uprootBox = transform.Find("UprootBox").GetComponent<BoxCollider>();

        ////Create a child object with the bouding box attached
        if (uprootBox == null) {
            //Create an empty game object as a child
            GameObject blank = new GameObject();
            blank = Instantiate(blank, gameObject.transform);
            blank.name = "UprootBox";
            blank.layer = 6;
            //Add a boc colider to the gameobject
            uprootBox = blank.AddComponent<BoxCollider>();
            uprootBox.isTrigger = true;

            BossStateUproot up = GetComponent<BossStateUproot>();
            uprootBox.center = new Vector3(0f, 1.5f, up.rangeEnd / 2f + up.rangeStart);
            uprootBox.size = new Vector3(3f, 3f, up.rangeEnd);
        }//End if

        //Priority in this order
        priorityAttacksInOrder.Add(Boss.State.Scream);
        priorityAttacksInOrder.Add(Boss.State.Uproot);
    }//End Awake

    public override void OnEnter()
    {
        base.OnEnter();
        //Toggle can be stunned
        canBeStunned = true;
        //Activate the idle animation
        boss.animator.SetTrigger("DoReturnToIdle");
        //Get the player's position with a y axis offset
        playerCenter = player.transform.position;
        playerCenter.y += 5f;
        //Get the distance to the player
        playerDistance = (player.transform.position - transform.position).magnitude;
        //Get a random decision time
        float rand = UnityEngine.Random.Range(minDecisionTime, maxDecisionTime);

        Boss.State newState = ChooseState();
        StartCoroutine(WaitThenSwitch(rand, newState));
    }//End OnEnter

    public override void OnExit()
    {
        base.OnExit();
        StopAllCoroutines();
    }//End OnExit

    private Boss.State ChooseState()
    {
        //Set the list of available attacks
        SetAttackPool();
        //If there are available attacks
        if (availableAttackPool.Count > 0)
        {
            //Pick one at random
            previousAttack = availableAttackPool[UnityEngine.Random.Range(0, availableAttackPool.Count)];
            //Add it to the recent attacks list
            recentAttacks.Enqueue(previousAttack);
            //Remove the oldest recent attacks
            if (recentAttacks.Count > maxAttacksBetweenBurrows)
                recentAttacks.Dequeue();

            return previousAttack;
        }//End if

        return Boss.State.Idle;        
    }//End ChooseState 
    
    private void SetAttackPool()
    {        
        availableAttackPool.Clear();

        //Prioritise burrow if the boss hasn't moved in recent memory
        if (maxAttacksBetweenBurrows > 0 && !recentAttacks.Contains(Boss.State.Burrow) && allAttacks.Contains(Boss.State.Burrow)) {
            availableAttackPool.Add(Boss.State.Burrow);
            return;
        }//End if

        foreach (Boss.State attack in allAttacks)
        {
            //If the condition for an attack is met
            if (attackDictionary[attack].Invoke() && attack != previousAttack/* && !recentAttacks.Contains(attack)*/)
            {
                availableAttackPool.Add(attack);
            }//End if
        }//End Foreach

        //If there is an available attack that is a priority
        foreach (Boss.State attack in priorityAttacksInOrder)
        {
            if (availableAttackPool.Contains(attack) && attack != previousAttack)
            {
                //If we find a priority attack, clear the attack pool of all other attacks and return
                availableAttackPool.Clear();
                availableAttackPool.Add(attack);
                return;
            }//End if
        }//End for

        //return if there are available attacks in the pool
        if (availableAttackPool.Count > 0)
            return;

        //If the previous attack is the only available attack, It's not idle, and it's condition is met re-add it to the pool
        if (previousAttack != Boss.State.Idle && attackDictionary[previousAttack].Invoke()) {
            print("Doing previous attack");
            availableAttackPool.Add(previousAttack);
        }//End if
    }//End SetAttackPool

    private IEnumerator WaitThenSwitch(float time, Boss.State state)
    {
        if(time > 0f) 
            yield return new WaitForSeconds(time);

        boss.ChangeState(state);
    }//End WaitThenSwitch

    //Remove the non attack states from the list
    private void InitAttacks()
    {
        previousAttack = Boss.State.Idle;

        for (int i = 0; i < allAttacks.Count;i++)
        {
            //Get the type of the state
            Type type = Type.GetType("BossState" + allAttacks[i].ToString());
            BossState bossState = (BossState)GetComponent(type);
            //If the type is an attack state and is present on the object
            if (!(bossState is AttackState))
            {
                allAttacks.Remove(allAttacks[i]);
                i--;
            }//End if
        }//End for

        //Add all the attacks to the dictionary along with their activation condition
        if (allAttacks.Contains(Boss.State.Charging)) attackDictionary.Add(Boss.State.Charging, () => playerDistance < chargeInRange);
        if (allAttacks.Contains(Boss.State.Burrow)) attackDictionary.Add(Boss.State.Burrow, () => playerDistance < burrowInRange);
        if (allAttacks.Contains(Boss.State.LandsRoots)) attackDictionary.Add(Boss.State.LandsRoots, () => playerDistance < landsRootsInRange);
        if (allAttacks.Contains(Boss.State.Scream)) attackDictionary.Add(Boss.State.Scream, () => playerDistance < screamInRange);
        if (allAttacks.Contains(Boss.State.RadialUproot)) attackDictionary.Add(Boss.State.RadialUproot, () => playerDistance < radialUprootInRange);

        if (allAttacks.Contains(Boss.State.Uproot))
        {
            attackDictionary.Add(Boss.State.Uproot, () =>
            {
                Bounds col = FindObjectOfType<Player>().GetComponent<CharacterController>().bounds;
                return uprootBox.bounds.Intersects(col);
            });
        }//End if
    }//End InitAttacks


    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, chargeInRange);
        Gizmos.DrawWireSphere(transform.position, burrowInRange);
        Gizmos.DrawWireSphere(transform.position, landsRootsInRange);
        Gizmos.DrawWireSphere(transform.position, screamInRange);
        Gizmos.DrawWireSphere(transform.position, chargeInRange);
        Gizmos.DrawWireSphere(transform.position, radialUprootInRange);
    }//End OnDrawGizmosSelected
}//End BossStateDecision
