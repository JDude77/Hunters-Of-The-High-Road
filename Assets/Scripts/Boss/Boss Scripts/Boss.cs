using UnityEngine;
using System;

[DisallowMultipleComponent]
public class Boss : Character
{
    public enum State
    {
        Idle,
        Charging,
        LandsRoots,
        Uproot,
        RadialUproot,
        Scream,
        Decision,
        Stunned,
        Burrow,
        Dead
    }

    [Header("States")]
    public State state;
    [SerializeField] public State mainState { get; private set; }

    [Header("Player layer")]
    public LayerMask attackLayer;

    [Header("Component references")]
    public Rigidbody body;
    public BossEventsHandler eventsHandler;
    private CapsuleCollider capsuleCollider;
    public Animator animator;
    public BossState currentState { get; private set; }

    [Header("Sounds")]
    [SerializeField] private AK.Wwise.Event OnHitSound;

    void Awake()
    {
        //Check that the boss contains all required components to function
        InitialiseComponents();
        //Initialise the main state
        mainState = State.Decision;
    }//End Awake

    protected override void Start()
    {
        base.Start();
        //Prevent damage until trigger
        canTakeDamage = false;
        //Check that the boss's children have the required components
        InitialiseChildComponents();
        //Subscribe the required functions to the events the boss needs to respond to
        InitialiseEventResponses();
    } //End Start

    //Update is called once per frame
    void Update()
    {
        //Runs the current state's update
        currentState.Run();
    } //End Update

    private void FixedUpdate()
    {
        //Runs the curren't state's fixed update
        currentState.FixedRun();
    } //End FixedUpdate

    private void Death() {
        OnDeath -= Death;
        OnHit -= BossHit;

        if (PlayerEventsHandler.current != null) {
            PlayerEventsHandler.current.OnHitEnemy -= ReduceHealthByAmount;
            PlayerEventsHandler.current.OnStaggerEnemy -= StunnedResponse;
        }//End if

        BossTrigger trig = FindObjectOfType<BossTrigger>();
        if (trig != null)
            trig.TriggerActivated -= Trigger;

        animator.SetTrigger("DoDie");
        ChangeState(State.Dead);
    }//End Death

    #region States
    //Deletes the current state component and adds the new state
    public State ChangeState(State state_) {
        //Exits the current state
        currentState.OnExit();
        //Get the type of the state
        Type type = Type.GetType("BossState" + state_.ToString());
        //Add that state as a component
        currentState = (BossState)GetComponent(type);
        state = state_;
        //Enter the new state
        currentState.OnEnter();
        Debug.Log("Changed state to " + state.ToString());
        return state;
    } //End ChangeState

    public void ReturnToMainState() {
        //Exits the current state
        currentState.OnExit();
        //Get the type of the state
        Type type = Type.GetType("BossState" + mainState.ToString());
        //Add that state as a component
        currentState = (BossState)GetComponent(type);
        state = mainState;
        //Enter the new state
        currentState.OnEnter();
    }//End ReturnToMainState
    #endregion

    #region Event Responses
    private void Trigger() {
        canTakeDamage = true;
        if (currentState is BossStateIdle) ChangeState(mainState);
    }//End Trigger

    private void BossHit() {
        OnHitSound.Post(gameObject);
    }//End BossHit

    private void StunnedResponse(GameObject obj) {
        //Change the boss's state
        if (currentState.stunnable)
            ChangeState(State.Stunned);
    }//End Stunnedresponse
    #endregion

    #region Initialisation
    private void InitialiseComponents() 
    {
        //Get the collider if it exists
        capsuleCollider = GetComponent<CapsuleCollider>();

        if (capsuleCollider == null) capsuleCollider = gameObject.AddComponent<CapsuleCollider>();
        //Get the current state
        currentState = GetComponent<BossStateIdle>();

        if (currentState == null) currentState = gameObject.AddComponent<BossStateIdle>();

        if (!GetComponent<BossStateDecision>()) gameObject.AddComponent<BossStateDecision>();

        if (!GetComponent<BossStateCharging>()) gameObject.AddComponent<BossStateCharging>();

        if (!GetComponent<BossStateLandsRoots>()) gameObject.AddComponent<BossStateLandsRoots>();

        if (!GetComponent<BossStateUproot>()) gameObject.AddComponent<BossStateUproot>();

        if (!GetComponent<BossStateScream>()) gameObject.AddComponent<BossStateScream>();

        if (!GetComponent<BossStateBurrow>()) gameObject.AddComponent<BossStateBurrow>();

        if (!GetComponent<BossEventsHandler>()) gameObject.AddComponent<BossEventsHandler>();

        if (GetComponentInChildren<Animator>()) animator = GetComponentInChildren<Animator>();
        else Debug.LogError("No Animator detected on boss child");

        //Get the rigidbody
        body = GetComponent<Rigidbody>();
        if (body == null) body = gameObject.AddComponent<Rigidbody>();

        //Add rotation constraints
        body.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        body.isKinematic = true;
    }//End InitialiseComponents

    private void InitialiseChildComponents() 
    {
        //Get the child object with an animator
        GameObject animatedChild = GetComponentInChildren<Animator>().gameObject;

        if (animatedChild == null) 
        {
            Debug.LogError("No child with animator detected");
        }//End if
        else if (!animatedChild.GetComponent<BossAnimationEventsHandler>()) 
        {
            animatedChild.AddComponent<BossAnimationEventsHandler>();
        }//End else if
    }//End InitialiseChildComponents

    private void InitialiseEventResponses() 
    {

        BossTrigger trig = FindObjectOfType<BossTrigger>();

        if (trig != null)
            trig.TriggerActivated += Trigger;

        //Subscribe to the hit enemy action
        if (PlayerEventsHandler.current != null) {
            PlayerEventsHandler.current.OnHitEnemy += ReduceHealthByAmount;
            PlayerEventsHandler.current.OnStaggerEnemy += StunnedResponse;
        }//End if

        OnHit += BossHit;
        OnDeath += Death;
    }//End InitialiseEventResponses
    #endregion

}
