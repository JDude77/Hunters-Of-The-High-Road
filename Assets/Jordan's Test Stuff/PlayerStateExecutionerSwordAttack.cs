using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateExecutionerSwordAttack : PlayerState
{
    private ExecutionerSword sword;

    private bool animationDone = false;

    public void SetAnimationDone()
    {
        animationDone = true;
    }//End SetAnimationDone

    protected override void Awake()
    {
        base.Awake();
        stateID = Player.State.ExecutionerSwordAttack;
    }//End Awake

    public override bool EnterState()
    {
        base.EnterState();

        playerAnimator.Play("SwordAttack");
        playerAdvancedAnimations.setIsUsingSword(true);

        return true;
    }//End EnterState

    public override bool ExitState()
    {
        base.ExitState();

        animationDone = false;

        return true;
    }//End ExitState

    private void Start()
    {
        sword = playerReference.GetComponentInChildren<ExecutionerSword>();
    }//End Start

    public override void UpdateState()
    {
        base.UpdateState();

        if(animationDone)
        {
            playerReference.ChangeState(Player.State.Idle);
        }//End if
    }//End UpdateState

    protected override void UpdateStateInputs()
    {
        base.UpdateStateInputs();
    }//End UpdateStateInputs
}