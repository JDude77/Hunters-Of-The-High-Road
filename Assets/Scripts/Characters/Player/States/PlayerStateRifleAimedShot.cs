using UnityEngine;

public class PlayerStateRifleAimedShot : PlayerState
{
    private Rifle rifle;
    private Transform playerTransform;
    private bool gunHasBeenShot = false;

    protected override void Awake()
    {
        base.Awake();
        stateID = Player.State.RifleAimedShot;
    }//End Awake

    private void Start()
    {
        rifle = playerReference.GetComponentInChildren<Rifle>();
        playerTransform = playerReference.GetComponentInChildren<Animator>().transform;
    }//End Start

    public override bool EnterState()
    {
        playerAdvancedAnimations.SetIsUsingGun(true, 0);
        rifle.SetIsBeingAimed(true);
        playerAnimator.SetBool("isAiming", true);
        playerAnimator.SetLayerWeight(1, 0);

        rifle.Deadshot();

        return true;
    }//End EnterState

    public override bool ExitState()
    {
        rifle.DeactivateDeadshot();
        rifle.SetIsBeingAimed(false);
        if (gunHasBeenShot)
        {
            playerAnimator.Play("HipFiring");
        }//End if
        playerAnimator.SetBool("isAiming", false);
        playerAnimator.SetLayerWeight(1, 1);

        return true;
    }//End ExitState

    public override void UpdateState()
    {
        base.UpdateState();

        playerReference.RegenerateFaith();

        MakePlayerLookAtAimLocation();
        rifle.UpdateLinePosition();
    }//End UpdateState

    private void MakePlayerLookAtAimLocation()
    {
        //Get the clicked position on the screen
        Vector2 mouseClickPosition = Input.mousePosition;

        //Create a ray from the camera to the clicked point
        Ray cameraRay = Camera.main.ScreenPointToRay(mouseClickPosition);

        //Get world location of clicked point from ray hit position
        Physics.Raycast(cameraRay, out RaycastHit cameraRayHitInfo);
        Vector3 mouseClickWorldPosition = cameraRayHitInfo.point;

        Vector3 playerLookAtLocation = mouseClickWorldPosition - playerReference.transform.position;
        playerLookAtLocation.y = 0;

        playerTransform.rotation = Quaternion.LookRotation(playerLookAtLocation);
    }//End MakePlayerLookAtAimLocation

    protected override void UpdateStateInputs()
    {
        base.UpdateStateInputs();

        //When the player shoots
        if(Input.GetMouseButtonDown(0))
        {
            //Shoot
            rifle.Use();
            gunHasBeenShot = true;

            //Then change state
            ChangePlayerStateToIdleOrRun();
        }//End if

        //Dodging overrides aiming
        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            gunHasBeenShot = false;
            playerReference.ChangeState(Player.State.Dodging);
        }//End if

        //When the aim button is released
        if (Input.GetMouseButtonUp(1))
        {
            gunHasBeenShot = false;
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