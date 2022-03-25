using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BossStateDecision : BossState
{
    [SerializeField] private List<Boss.State> availableAttacks;
    private List<Boss.State> attacks = new List<Boss.State>();
    private Vector2 playerPosition;

    [Tooltip("Attack is an eligible attack if the player is within this range")]
    [SerializeField] private float chargeInRange, landsRootsInRange, burrowInRange;
    [Tooltip("CircleSwipe is an ineligible attack if the player is outside this range")]
    [SerializeField] private float circleSwipeOutRange;
    [Tooltip("Uproot is an eligible attack if the player is within this area")]
    [SerializeField] private BoxCollider uprootBox;

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

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
    }

    public override void OnEnter()
    {
        base.OnEnter();
        playerPosition = player.transform.position;
    }

    public override void OnExit()
    {
        base.OnExit();
    }

    public override void Run()
    {
        base.Run();

    }

    //Store the attack states from the inspector list in a seperate list
    void InitAttacks()
    {
        for(int i = 0; i < availableAttacks.Count;i++)
        {
            //Get the type of the state
            Type type = Type.GetType("BossState" + availableAttacks[i].ToString());
            BossState bossState = (BossState)GetComponent(type);
            //If the type is an attack state and is present on the object
            if (bossState is AttackState)
            {
                attacks.Add(availableAttacks[i]);
            }
            else
            {
                availableAttacks.Remove(availableAttacks[i]);
                i--;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, chargeInRange);
        Gizmos.DrawWireSphere(transform.position, burrowInRange);
        Gizmos.DrawWireSphere(transform.position, landsRootsInRange);
        Gizmos.DrawWireSphere(transform.position, circleSwipeOutRange);
        Gizmos.DrawWireSphere(transform.position, chargeInRange);
    }
}
