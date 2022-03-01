using System.Collections;
using UnityEngine;

public class PlayerStateDodging : PlayerState
{
    private CharacterController playerController;
    private Transform playerTransform;
    private Vector3 movement;

    private bool isRolling;
    [SerializeField]
    private float rollTime;
    [SerializeField]
    private float rollingSpeedBoost;

    [SerializeField]
    private AnimationCurve rollCurve;

    protected override void Awake()
    {
        base.Awake();
        stateID = Player.State.Dodging;
    }//End Awake

    private void Start()
    {
        playerController = playerReference.GetComponent<CharacterController>() ? playerReference.GetComponent<CharacterController>() : playerReference.gameObject.AddComponent<CharacterController>();
        playerTransform = playerReference.transform;

        isRolling = false;
    }//End Start

    public override bool EnterState()
    {
        base.EnterState();

        movement.x = Input.GetAxis("Horizontal");
        movement.z = Input.GetAxis("Vertical");

        if (movement != Vector3.zero)
        {
            Quaternion rotateTo = Quaternion.LookRotation(movement, Vector3.up);
            playerTransform.rotation = rotateTo;
        }//End if

        StartCoroutine(RollDodge());
        isRolling = true;
        
        return true;
    }//End EnterState

    public override void UpdateState()
    {
        if(!isRolling)
        {
            if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
            {
                playerReference.ChangeState(Player.State.Running);
            }//End if
            else
            {
                playerReference.ChangeState(Player.State.Idle);
            }//End else
        }//End if
    }//End UpdateState

    private IEnumerator RollDodge()
    {
        float timeInRoll = 0;
        
        while (timeInRoll < rollTime)
        {
            float jumpForce = rollCurve.Evaluate(timeInRoll);
            playerController.Move(playerTransform.forward * jumpForce * rollingSpeedBoost * Time.deltaTime);
            timeInRoll += Time.deltaTime;
            yield return null;
        }//End while

        isRolling = false;
    }//End RollDodge

    public bool GetIsRolling()
    {
        return isRolling;
    }
}
