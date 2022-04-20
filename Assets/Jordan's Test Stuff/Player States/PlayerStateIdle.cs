using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateIdle : PlayerState
{
    protected override void Awake()
    {
        base.Awake();
        stateID = Player.State.Idle;
    }//End Awake

    public override bool EnterState()
    {
        base.EnterState();

        playerAnimator.SetFloat("MovementBlend", 0.0f);

        return true;
    }//End EnterState

    public override void UpdateState()
    {
        base.UpdateState();

        playerReference.RegenerateFaith();
    }//End UpdateState

    protected override void UpdateStateInputs()
    {
        base.UpdateStateInputs();

        //Dodging
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (playerReference.HasState(Player.State.Dodging))
            {
                PlayerStateDodging dodging = (PlayerStateDodging)playerReference.GetState(Player.State.Dodging);
                if (playerReference.GetStamina() >= dodging.GetStaminaCost())
                {
                    playerReference.ChangeState(Player.State.Dodging);
                }//End if
            }//End if
        }//End if

        //Sword Attack
        //There's definitely a better way to do this
        if (Input.GetKeyDown(KeyCode.E))
        {
            //If both sword and sword attack state are on the player
            if (playerReference.HasItem(typeof(ExecutionerSword)) && playerReference.HasState(Player.State.ExecutionerSwordAttack))
            {
                PlayerStateExecutionerSwordAttack swordAttack = (PlayerStateExecutionerSwordAttack)playerReference.GetState(Player.State.ExecutionerSwordAttack);
                if (playerReference.GetStamina() >= swordAttack.GetStaminaCost())
                {
                    playerReference.ChangeState(Player.State.ExecutionerSwordAttack);
                }//End if
            }//End if
        }//End if

        //Aimed Shot
        //There's definitely a better way to do this
        if (Input.GetMouseButtonDown(1) && playerReference.HasItem(typeof(Rifle)) && playerReference.HasState(Player.State.RifleAimedShot))
        {
            playerReference.ChangeState(Player.State.RifleAimedShot);
        }//End if

        //Hipfire Shot
        //There's definitely a better way to do this
        if (Input.GetMouseButtonDown(0) && playerReference.HasItem(typeof(Rifle)))
        {
            //Get a reference to the rifle on the player
            //Should probably be cached earlier but that's an optimisation thing for later
            Rifle rifle = (Rifle)playerReference.GetItem(typeof(Rifle));

            //Checking if the rifle is reloaded here prevents animation jank
            if(rifle.GetIsReloaded())
            {
                playerAdvancedAnimations.SetIsUsingGun(true, 1);
                playerAnimator.Play("HipFiring");

                rifle.Use();
            }//End if
        }//End if

        //Running
        if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
        {
            playerReference.ChangeState(Player.State.Running);
        }//End if
    }//End UpdateStateInputs
}