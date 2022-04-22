using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[DisallowMultipleComponent]
public class BossStateDecision : BossState
{
    //List of desired attacks in the inspector
    [SerializeField] private List<Boss.State> attacks;
    private Dictionary<Boss.State, Func<bool>> attackDictionary = new Dictionary<Boss.State, Func<bool>>();
    private List<Boss.State> attackPool = new List<Boss.State>();

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
    [Tooltip("Uproot is an eligible attack if the player is within this area")]
    [SerializeField] private BoxCollider uprootBox;

    [Header("Decision Delay Settings")]
    [SerializeField] private float maxDecisionTime;
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

        playerCenter = player.transform.position;
        playerCenter.y += 5f;

        playerDistance = (player.transform.position - transform.position).magnitude;

        float rand = UnityEngine.Random.Range(minDecisionTime, maxDecisionTime);

        boss.animator.SetTrigger("DoReturnToIdle");

        StartCoroutine(wait(rand));
    }

    public override void OnExit()
    {
        base.OnExit();        
    }

    Boss.State ChooseState()
    {
        if (SetAttackPool())
        {
            previousAttack = attackPool[UnityEngine.Random.Range(0, attackPool.Count)]; ;
            return previousAttack;
        }
        else
        {
            return Boss.State.Idle;
        }
    }

    bool SetAttackPool()
    {
        attackPool.Clear();

        foreach (Boss.State attack in attacks)
        {
            //If the condition for an attack is met
            if (attackDictionary[attack].Invoke() && attack != previousAttack)
            {
                //Add the attack to the pool
                attackPool.Add(attack);
            }//End if
        }//End Foreach

        //If there is an available attack that is a priority
        foreach (Boss.State attack in priorityAttacksInOrder)
        {
            if (attackPool.Contains(attack))
            {
                attackPool.Clear();
                attackPool.Add(attack);
                return true;
            }
        }

        //If there is at least 1 attack in the pool that isn't the previous attack
        if (attackPool.Count > 0)
            return true;

        //If the previous attack is the only available attack, It's not idle, and it's condition is met re-add it to the pool
        if (previousAttack != Boss.State.Idle && attackDictionary[previousAttack].Invoke())
        {
            attackPool.Add(previousAttack);
            return true;
        }//End if

        return false;
    }

    IEnumerator wait(float time)
    {
        yield return new WaitForSeconds(time);
        boss.ChangeState(ChooseState());
    }

    //Remove the non attack states from the list
    void InitAttacks()
    {
        for(int i = 0; i < attacks.Count;i++)
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
    }
}
