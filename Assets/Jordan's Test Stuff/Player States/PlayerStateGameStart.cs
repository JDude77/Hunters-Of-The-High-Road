using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateGameStart : PlayerState
{
    protected override void Awake()
    {
        base.Awake();
        stateID = Player.State.GameStart;
    }//End Awake

    public override bool ExitState()
    {
        playerAnimator.Play("StandUp");
        return true;
    }//End ExitState
}
