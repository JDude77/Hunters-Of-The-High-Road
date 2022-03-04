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

    public override bool EnterState()
    {
        playerAdvancedAnimations.SetIsUsingGun(true, 0);

        rifle.Deadshot();

        return true;
    }//End EnterState

    public override bool ExitState()
    {
        rifle.DeactivateDeadshot();

        return true;
    }//End ExitState

    public override void UpdateState()
    {
        base.UpdateState();

        MakePlayerLookAtAimLocation();
        rifle.UpdateLinePosition();
    }//End UpdateState

    private void MakePlayerLookAtAimLocation()
    {
        Vector3 shotLocation = Vector3.zero;

        Ray hitRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(hitRay, out RaycastHit hit);

        if (hit.transform != null)
        {
            shotLocation = hit.point;
        }//End if

        shotLocation.y = rifle.GetShotHeight();

        Vector3 lookAt = shotLocation - playerReference.transform.position;
        lookAt.y = 0;
        playerReference.transform.rotation = Quaternion.LookRotation(lookAt);
    }//End MakePlayerLookAtAimLocation

    protected override void UpdateStateInputs()
    {
        base.UpdateStateInputs();

        //When the player shoots
        if(Input.GetMouseButtonDown(0))
        {
            //Shoot

            //Then change state
            ChangePlayerStateToIdleOrRun();
        }//End if

        //Dodging overrides aiming
        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            playerReference.ChangeState(Player.State.Dodging);
        }//End if

        //When the aim button is released
        if (Input.GetMouseButtonUp(1))
        {
            ChangePlayerStateToIdleOrRun();
        }//End if
    }//End UpdateStateInputs

    private static void ChangePlayerStateToIdleOrRun()
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
    }//End ChangePlayerStateToIdleOrRun
}