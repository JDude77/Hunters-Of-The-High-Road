using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateExecutionerSwordAttack : PlayerState
{
    private ExecutionerSword sword;

    private bool animationDone = false;

    [SerializeField]
    private float staminaCost;
    public float GetStaminaCost() { return staminaCost; }

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

        playerReference.ReduceStaminaByAmount(staminaCost);

        playerAnimator.Play("SwordAttack");
        playerAdvancedAnimations.SetIsUsingSword(true);

        return true;
    }//End EnterState

    public override bool ExitState()
    {
        base.ExitState();

        playerAdvancedAnimations.SetIsUsingSword(false);
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