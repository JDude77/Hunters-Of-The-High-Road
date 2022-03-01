using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateExecutionerSwordAttack : PlayerState
{
    private ExecutionerSword sword;

    protected override void Awake()
    {
        base.Awake();
        stateID = Player.State.ExecutionerSwordAttack;
    }//End Awake

    private void Start()
    {
        sword = playerReference.GetComponentInChildren<ExecutionerSword>();
    }//End Start

    public override void UpdateState()
    {
        base.UpdateState();
    }//End UpdateState

    protected override void UpdateStateInputs()
    {
        base.UpdateStateInputs();
    }//End UpdateStateInputs
}