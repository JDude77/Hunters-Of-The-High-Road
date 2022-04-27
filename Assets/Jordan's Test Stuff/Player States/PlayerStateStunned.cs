using UnityEngine;

public class PlayerStateStunned : PlayerState
{
    [SerializeField]
    [Tooltip("Change this value to determine how long the player is stunned for in seconds.")]
    private float stunTime = 1.0f;
    private float currentStunTime = 0.0f;

    [SerializeField]
    [Tooltip("A minor balance toggle, to allow designers to choose whether Faith can still regenerate while the player is stunned.")]
    private bool regenerateFaith = true;

    protected override void Awake()
    {
        base.Awake();
        stateID = Player.State.Stunned;
    }//End Awake

    public override bool EnterState()
    {
        base.EnterState();

        currentStunTime = 0.0f;

        return true;
    }//End EnterState

    public override void UpdateState()
    {
        base.UpdateState();

        if(regenerateFaith) playerReference.RegenerateFaith();

        //Basic stun timer
        if (currentStunTime < stunTime)
        {
            currentStunTime += Time.deltaTime;
        }//End if
        else
        {
            playerReference.ChangeState(Player.State.Idle);
        }//End else
    }//End UpdateState



    //To add the stunned animation -
    //1. In OnEnter, play the stunned animation
    //2. In OnExit, call the "LeaveStunned" trigger on the animator
    //   this should transition back to the normal movement tree


    //if it breaks, lol xd
}