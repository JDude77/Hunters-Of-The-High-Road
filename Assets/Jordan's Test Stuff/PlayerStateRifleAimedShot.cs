using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateRifleAimedShot : PlayerState
{
    private Rifle rifle;

    protected override void Awake()
    {
        base.Awake();
        stateID = Player.State.RifleAimedShot;
    }//End Awake

    private void Start()
    {
        rifle = playerReference.GetComponentInChildren<Rifle>();
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