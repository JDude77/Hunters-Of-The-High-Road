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

    public State state;
    [SerializeField] public State mainState { get; private set; }

    public LayerMask attackLayer;

    public Rigidbody body;
    private CapsuleCollider capsuleCollider;

    public BossEventsHandler eventsHandler;
    public BossState currentState { get; private set; }
    public Animator animator;
    //Start is called before the first frame update

    void Awake()
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

        GameObject animatedChild = GetComponentInChildren<Animator>().gameObject;
        if (animatedChild == null)
        {
            Debug.LogError("No animator detected on Boss child");
        }
        else if(!animatedChild.GetComponent<BossAnimationEventsHandler>())
        {
            animatedChild.AddComponent<BossAnimationEventsHandler>();
        }        

        mainState = State.Decision;
    }

    protected override void Start()
    {
        base.Start();
        FindObjectOfType<BossTrigger>().TriggerActivated += () => { if (currentState is BossStateIdle) ChangeState(mainState); };

        if (PlayerEventsHandler.current != null) {
            PlayerEventsHandler.current.OnHitEnemy += ReduceHealthByAmount;
        }
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

    #region States
    //Deletes the current state component and adds the new state
    public State ChangeState(State state_)
    {
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

    public void ReturnToMainState()
    {
        //Exits the current state
        currentState.OnExit();
        //Get the type of the state
        Type type = Type.GetType("BossState" + mainState.ToString());
        //Add that state as a component
        currentState = (BossState)GetComponent(type);
        state = mainState;
        //Enter the new state
        currentState.OnEnter();
    }
    #endregion
}
