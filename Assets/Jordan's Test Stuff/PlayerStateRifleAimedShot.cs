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

        //Dodging overrides aiming
        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            playerReference.ChangeState(Player.State.Dodging);
        }//End if

        //When the aim to shoot button is released
        if (Input.GetMouseButtonUp(1))
        {
            Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
            if (movement != Vector3.zero)
            {
                playerReference.ChangeState(Player.State.Running);
            }//End if
            else
            {
                playerReference.ChangeState(Player.State.Idle);
            }//End else
        }//End if
    }//End UpdateStateInputs
}