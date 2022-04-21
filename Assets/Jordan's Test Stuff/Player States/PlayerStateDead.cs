using UnityEngine;

public class PlayerStateDead : PlayerState
{
    protected override void Awake()
    {
        base.Awake();
        stateID = Player.State.Dead;
    }//End Awake

    public override bool EnterState()
    {
        //CAMERON CAN PUT GAME OVER STUFF TO RUN ONCE HERE
        return true;
    }//End EnterState
}