using UnityEngine;

public class PlayerStateRunning : PlayerState
{
    private CharacterController playerController;
    private Transform playerTransform;
    private Vector3 movement;

    [SerializeField]
    private float movementSpeed;

    protected override void Awake()
    {
        base.Awake();
        stateID = Player.State.Running;
    }//End Awake

    private void Start()
    {
        playerController = playerReference.GetComponent<CharacterController>() ? playerReference.GetComponent<CharacterController>() : playerReference.gameObject.AddComponent<CharacterController>();
        playerTransform = playerReference.GetComponentInChildren<Animator>().transform;
    }//End Start

    public override void UpdateState()
    {
        base.UpdateState();

        //Simple movement code stolen from Cameron's script - could be improved
        movement.x = Input.GetAxis("Horizontal");
        movement.z = Input.GetAxis("Vertical");

        if (movement != Vector3.zero)
        {
            playerController.SimpleMove(Vector3.ClampMagnitude(movement, 1) * movementSpeed);

            playerAnimator.SetFloat("MovementBlend", Vector3.ClampMagnitude(movement, 1).magnitude);

            Quaternion rotateTo = Quaternion.LookRotation(movement, Vector3.up);
            playerTransform.rotation = Quaternion.RotateTowards(playerTransform.rotation, rotateTo, 720 * Time.deltaTime);
        }//End if
        else
        {
            playerReference.ChangeState(Player.State.Idle);
        }//End if
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
            if(playerReference.HasItem(typeof(ExecutionerSword)) && playerReference.HasState(Player.State.ExecutionerSwordAttack))
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
            if (rifle.GetIsReloaded())
            {
                playerAdvancedAnimations.SetIsUsingGun(true, 1);
                playerAnimator.Play("HipFiring");

                rifle.Use();
            }//End if
        }//End if
    }//End UpdateStateInputs
}
