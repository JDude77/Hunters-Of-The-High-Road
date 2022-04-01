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
    private Boss.State previousAttack;

    private Vector3 playerPosition;
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

        //Create a child object with the bouding box attached
        if (uprootBox == null)
        {
            //Create an empty game object as a child
            GameObject blank = new GameObject();
            blank = Instantiate(blank, gameObject.transform);
            blank.name = "UprootBox";
            //Add a boc colider to the gameobject
            uprootBox = blank.AddComponent<BoxCollider>();
            uprootBox.isTrigger = true;

            BossStateUproot up = GetComponent<BossStateUproot>();
            uprootBox.center = new Vector3(0f, 1.5f, up.rangeEnd / 2f + up.rangeStart);
            uprootBox.size = new Vector3(3f, 3f, up.rangeEnd);
        }
    }

    public override void OnEnter()
    {
        base.OnEnter();
        playerPosition = player.transform.position;
        playerDistance = (player.transform.position - transform.position).magnitude;

        float rand = UnityEngine.Random.Range(minDecisionTime, maxDecisionTime);
        StartCoroutine(wait(rand));
    }

    public override void OnExit()
    {
        base.OnExit();
    }

    Boss.State ChooseAttack()
    {
        SetAttackPool();

        Boss.State newAttack;
        //If there are attacks in the pool
        if (attackPool.Count > 0)
        {
            //Set the 
            newAttack = attackPool[UnityEngine.Random.Range(0, attackPool.Count)];
        }
        else
        {
            newAttack = Boss.State.Idle;
        }

        previousAttack = newAttack;
        return newAttack;
    }

    void SetAttackPool()
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

        //If the previous attack is the only available attack, re-add it to the pool
        if (attackPool.Count == 0)
        {
            attackPool.Add(previousAttack);
        }//End if
    }

    IEnumerator wait(float time)
    {
        yield return new WaitForSeconds(time);
        boss.ChangeState(ChooseAttack());
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

        if (attacks.Contains(Boss.State.Uproot)) attackDictionary.Add(Boss.State.Uproot, () => uprootBox.bounds.Contains(playerPosition));
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
