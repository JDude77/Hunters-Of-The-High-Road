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
    [SerializeField] private List<Boss.State> attacks;
    List<Boss.State> attackPool = new List<Boss.State>();
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
    [SerializeField] private float attackMemoryLength;

    [Header("Decision Delay Settings")]
    [Tooltip("The maximum time the boss will wait before changing to a new attack")]
    [SerializeField] private float maxDecisionTime;
    [Tooltip("The minimum time the boss will wait before changing to a new attack")]
    [SerializeField] private float minDecisionTime;
        
    private bool attackChosen = false;

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
            }
        }

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
        }

        //Priority in this order
        priorityAttacksInOrder.Add(Boss.State.Scream);
        priorityAttacksInOrder.Add(Boss.State.Uproot);
    }

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

        foreach(Boss.State attack in recentAttacks) {
            print(attack.ToString());
        }
        print(" ");
        //If an attack has already been chosen, change to the attack 
        // (this is true in the event that the boss is stunned during the decision timer coroutine)
        if (attackChosen)
            boss.ChangeState(previousAttack);

        Boss.State newState = ChooseState();
        StartCoroutine(wait(rand, newState));
    }

    public override void OnExit()
    {
        base.OnExit();
        StopAllCoroutines();
    }

    Boss.State ChooseState()
    {
        //Set the list of available attacks
        SetAttackPool();
        //If there are available attacks
        if (attackPool.Count > 0)
        {
            //Pick one at random
            previousAttack = attackPool[UnityEngine.Random.Range(0, attackPool.Count)];
            //Add it to the recent attacks list
            recentAttacks.Enqueue(previousAttack);
            //Remove the oldest recent attacks
            if (recentAttacks.Count > attackMemoryLength)
                recentAttacks.Dequeue();

            return previousAttack;
        }

        return Boss.State.Idle;        
    }

    void SetAttackPool()
    {        
        attackPool.Clear();

        //Prioritise burrow if the boss hasn't moved in recent memory
        if (!recentAttacks.Contains(Boss.State.Burrow)) {
            attackPool.Add(Boss.State.Burrow);
            return;
        }

        foreach (Boss.State attack in attacks)
        {
            //If the condition for an attack is met
            if (attackDictionary[attack].Invoke() && attack != previousAttack/* && !recentAttacks.Contains(attack)*/)
            {
                attackPool.Add(attack);
            }//End if
        }//End Foreach

        //If there is an available attack that is a priority
        foreach (Boss.State attack in priorityAttacksInOrder)
        {
            if (attackPool.Contains(attack) && !recentAttacks.Contains(attack))
            {
                //If we find a priority attack, clear the attack pool of all other attacks and return
                attackPool.Clear();
                attackPool.Add(attack);
                return;
            }
        }

        //if(attackPool.Count == 0) {
        //    Queue<Boss.State> newQueue = recentAttacks;
        //    //Loop backwards through the recent attacks queue to prioritise the least recent attack
        //    for (int i = newQueue.Count - 1; i >= 0; i--)
        //    {
        //        Boss.State recentAttack = newQueue.Dequeue();
        //        //if the condition is met for the attack
        //        if (attackDictionary[recentAttack].Invoke()) 
        //        {
        //            //Add it too the pool and break
        //            attackPool.Add(recentAttack);
        //            return;
        //        }
        //    }
        //}

        //If the previous attack is the only available attack, It's not idle, and it's condition is met re-add it to the pool
        if ((previousAttack != Boss.State.Idle) && attackDictionary[previousAttack].Invoke())
        {
            attackPool.Add(previousAttack);
        }//End if
    }//End SetAttackPool

    IEnumerator wait(float time, Boss.State state)
    {
        attackChosen = true;

        if(time > 0f) 
            yield return new WaitForSeconds(time);

        attackChosen = false;
        boss.ChangeState(state);
    }

    //Remove the non attack states from the list
    void InitAttacks()
    {
        previousAttack = Boss.State.Idle;

        for (int i = 0; i < attacks.Count;i++)
        {
            //Get the type of the state
            Type type = Type.GetType("BossState" + attacks[i].ToString());
            BossState bossState = (BossState)GetComponent(type);
            //If the type is an attack state and is present on the object
            if (!(bossState is AttackState))
            {
                attacks.Remove(attacks[i]);
                i--;
            }
        }

        //Add all the attacks to the dictionary along with their activation condition
        if (attacks.Contains(Boss.State.Charging)) attackDictionary.Add(Boss.State.Charging, () => playerDistance < chargeInRange);

        if (attacks.Contains(Boss.State.Burrow)) attackDictionary.Add(Boss.State.Burrow, () => playerDistance < burrowInRange);

        if (attacks.Contains(Boss.State.LandsRoots)) attackDictionary.Add(Boss.State.LandsRoots, () => playerDistance < landsRootsInRange);

        if (attacks.Contains(Boss.State.Scream)) attackDictionary.Add(Boss.State.Scream, () => playerDistance < screamInRange);

        if (attacks.Contains(Boss.State.RadialUproot)) attackDictionary.Add(Boss.State.RadialUproot, () => playerDistance < radialUprootInRange);

        if (attacks.Contains(Boss.State.Uproot)) attackDictionary.Add(Boss.State.Uproot, () => {
            Bounds col = FindObjectOfType<Player>().GetComponent<CharacterController>().bounds;
            return uprootBox.bounds.Intersects(col);
            }
        );
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, chargeInRange);
        Gizmos.DrawWireSphere(transform.position, burrowInRange);
        Gizmos.DrawWireSphere(transform.position, landsRootsInRange);
        Gizmos.DrawWireSphere(transform.position, screamInRange);
        Gizmos.DrawWireSphere(transform.position, chargeInRange);
        Gizmos.DrawWireSphere(transform.position, radialUprootInRange);
    }
}
