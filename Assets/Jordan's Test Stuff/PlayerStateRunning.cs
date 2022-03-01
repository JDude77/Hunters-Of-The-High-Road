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
        playerTransform = playerReference.transform;
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
            playerReference.ChangeState(Player.State.Dodging);
        }//End if

        //Sword Attack
        //There's definitely a better way to do this
        if (Input.GetKeyDown(KeyCode.E) && playerReference.HasItem(typeof(ExecutionerSword)) && playerReference.HasState(Player.State.ExecutionerSwordAttack))
        {
            playerReference.ChangeState(Player.State.ExecutionerSwordAttack);
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
            playerReference.GetItem(typeof(Rifle)).Use();
        }//End if
    }//End UpdateStateInputs
}
