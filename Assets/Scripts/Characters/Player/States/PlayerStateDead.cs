public class PlayerStateDead : PlayerState
{
    protected override void Awake()
    {
        base.Awake();
        stateID = Player.State.Dead;
    }//End Awake

    public override bool EnterState()
    {
        base.EnterState();

        playerAnimator.Play("Dying");

        return true;
    }//End EnterState
}