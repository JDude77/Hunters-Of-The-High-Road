using System.Collections;
using UnityEngine;

public class PlayerStateDodging : PlayerState
{
    private CharacterController playerController;
    private Transform playerTransform;
    private Vector3 movement;

    [SerializeField]
    private float staminaCost;
    public float GetStaminaCost() { return staminaCost; }

    private bool isRolling;
    [SerializeField]
    private float rollTime;
    [SerializeField]
    private float rollingSpeedBoost;

    [SerializeField]
    private AnimationCurve rollCurve;

    [Header("Roll Camera Shake Settings")]
    [SerializeField]
    private float shakeIntensity;
    [SerializeField]
    private float shakeTime;

    protected override void Awake()
    {
        base.Awake();
        stateID = Player.State.Dodging;
    }//End Awake

    private void Start()
    {
        playerController = playerReference.GetComponent<CharacterController>() ? playerReference.GetComponent<CharacterController>() : playerReference.gameObject.AddComponent<CharacterController>();
        playerTransform = playerReference.GetComponentInChildren<Animator>().transform;

        isRolling = false;
    }//End Start

    public void ShakeCamera()
    {
        CameraShakeScript.Instance.ShakeCamera(shakeIntensity, shakeTime);
    }//End ShakeCamera

    public override bool EnterState()
    {
        base.EnterState();

        playerReference.ReduceStaminaByAmount(staminaCost);

        movement.x = Input.GetAxis("Horizontal");
        movement.z = Input.GetAxis("Vertical");

        if (movement != Vector3.zero)
        {
            Quaternion rotateTo = Quaternion.LookRotation(movement, Vector3.up);
            playerTransform.rotation = rotateTo;
        }//End if

        playerAnimator.Play("Rolling");

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
    }//End GetIsRolling
}
