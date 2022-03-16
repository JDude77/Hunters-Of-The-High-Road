using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Boss : Character
{
    public enum State
    {
        Idle,
        Charging,
        LandsRoots,
        Uproot,
        CircleSwipe,
        DecisionPrototype,
        Stunned,
        Burrow,
        Slashing
    }

    public State state;
    public LayerMask attackLayer;

    public Rigidbody body;
    private CapsuleCollider capsuleCollider;

    public BossEventsHandler eventsHandler;
    public EventResponder eventResponder;
    private BossStateMachine stateMachine;
    private BossState currentState;
    //Start is called before the first frame update

    void Awake()
    {
        //Get the collider if it exists
        capsuleCollider = GetComponent<CapsuleCollider>();
        if (capsuleCollider == null) capsuleCollider = gameObject.AddComponent<CapsuleCollider>();
        //Get the current state
        currentState = GetComponent<BossStateIdle>();
        if (currentState == null) currentState = gameObject.AddComponent<BossStateIdle>();

        if (!GetComponent<BossStateCharging>()) gameObject.AddComponent<BossStateCharging>();

        if (!GetComponent<BossStateLandsRoots>()) gameObject.AddComponent<BossStateLandsRoots>();

        if (!GetComponent<BossStateUproot>()) gameObject.AddComponent<BossStateUproot>();

        if (!GetComponent<BossStateCircleSwipe>()) gameObject.AddComponent<BossStateCircleSwipe>();

        if (!GetComponent<BossStateBurrow>()) gameObject.AddComponent<BossStateBurrow>();

        if (!GetComponent<BossStateStunned>()) gameObject.AddComponent<BossStateStunned>();

        if (!GetComponent<BossEventsHandler>()) gameObject.AddComponent<BossEventsHandler>();

        eventResponder = GetComponent<EventResponder>();
        if (eventResponder == null) eventResponder = gameObject.AddComponent<EventResponder>();

        //Get the rigidbody
        body = GetComponent<Rigidbody>();
        if (body == null) body = gameObject.AddComponent<Rigidbody>();

        //Add rotation constraints
        body.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        body.isKinematic = true;
    }

    protected override void Start()
    {
        base.Start();     

    } //End Start

    //Update is called once per frame
    void Update()
    {
        //Runs the current state's update
        currentState.Run();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            eventResponder.Respond(BossEvent.ChargeEvents.ChargeStart.ToString());
            Debug.Log("Input detected");
        }

    } //End Update

    private void FixedUpdate()
    {
        //Runs the curren't state's fixed update
        currentState.FixedRun();
    } //End FixedUpdate

    //Deletes the current state component and adds the new state
    public State ChangeState(State state_)
    {
        //Exits the current state
        currentState.OnExit();
        Debug.Log("Exiting: " + state.ToString());
        //Get the type of the state
        Type type = Type.GetType("BossState" + state_.ToString());
        //Add that state as a component
        currentState = (BossState)GetComponent(type);
        state = state_;
        Debug.Log("Entering: " + state.ToString());
        //Enter the new state
        currentState.OnEnter();

        return state;
    } //End ChangeState

    //Activates the boss' decision making
    public void ActivateBoss()
    {
        if (currentState is BossStateIdle)
        {
            ChangeState(State.DecisionPrototype);
        }
    }
}