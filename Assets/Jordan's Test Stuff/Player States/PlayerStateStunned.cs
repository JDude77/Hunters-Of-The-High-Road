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

        playerAnimator.Play("Stunned");

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

    public override bool ExitState()
    {
        base.ExitState();

        playerAnimator.SetTrigger("LeaveStunned");

        return true;
    }//End ExitState
}