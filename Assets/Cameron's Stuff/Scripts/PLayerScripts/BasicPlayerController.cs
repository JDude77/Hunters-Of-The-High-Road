using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicPlayerController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed, rollingSpeedBoost, rollTime;

    [SerializeField]
    private AnimationCurve rollCurve;

    [SerializeField]
    private Transform playerBody;

    [SerializeField]
    private TestGun gun;

    private CharacterController controller;
    private Vector3 moveVector;

    private bool isRolling = false, isFrozen = false;
    private bool hasGameStarted = false;

    private Animator ani;
    private PlayerAdvancedAnimations advancedAni;

    private bool isAiming = false;

    private PlayerStats playerStats;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        
        ani = GetComponentInChildren<Animator>();
        advancedAni = GetComponent<PlayerAdvancedAnimations>();
        
        playerStats = GetComponent<PlayerStats>();
    }

    // Update is called once per frame
    void Update()
    {
        if (hasGameStarted)
        {
            GameplayControls();
        }
    }

    private void GameplayControls()
    {
        if (!isRolling && !isFrozen)
        {
            if (Input.GetMouseButtonUp(1))
            {
                advancedAni.setIsUsingGun(true, 1);
            }

            ani.SetBool("isAiming", isAiming);
            gun.SetAiming(isAiming);
            GunInputs();

            if (Input.GetMouseButton(1))
            {
                isAiming = true;
                Aiming();
                advancedAni.setIsUsingGun(true, 0);
            }
            else
            {
                isAiming = false;

                Movement();
                swordInput();

                if (Input.GetKeyDown(KeyCode.LeftShift) && playerStats.getCurrentStamina() > playerStats.getRollingCost())
                {
                    ActivateRoll();
                }
            }

        }
    }

    private void Aiming()
    {
        Vector3 shootHere = Vector3.zero;

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out hit);

        if (hit.transform != null)
        {
            shootHere = hit.point;
        }

        shootHere.y = gun.getAimHeight();

        Vector3 lookat = shootHere - transform.position;
        lookat.y = 0;
        playerBody.rotation = Quaternion.LookRotation(lookat);
    }

    private void swordInput()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            ani.Play("SwordAttack");
            isFrozen = true;
            advancedAni.setIsUsingSword(true);
        }
    }

    private void GunInputs()
    {
        if(Input.GetMouseButtonDown(0))
        {
            advancedAni.setIsUsingGun(true, 1);
            ani.Play("HipFiring");
            gun.Shoot();
        }
    }

    private void Movement()
    {
        moveVector.x = Input.GetAxis("Horizontal");
        moveVector.z = Input.GetAxis("Vertical");

        controller.SimpleMove(Vector3.ClampMagnitude(moveVector, 1) * moveSpeed);

        ani.SetFloat("MovementBlend", Vector3.ClampMagnitude(moveVector, 1).magnitude);

        if (moveVector != Vector3.zero)
        {
            Quaternion rotateTo = Quaternion.LookRotation(moveVector, Vector3.up);
            playerBody.transform.rotation = Quaternion.RotateTowards(playerBody.transform.rotation, rotateTo, 720 * Time.deltaTime);
        }
    }

    //snaps the player towards their movement input then starts the rolling coroutine
    private void ActivateRoll()
    {
        moveVector.x = Input.GetAxis("Horizontal");
        moveVector.z = Input.GetAxis("Vertical");

        if (moveVector != Vector3.zero)
        {
            Quaternion rotateTo = Quaternion.LookRotation(moveVector, Vector3.up);
            playerBody.transform.rotation = rotateTo;
        }

        ani.Play("Rolling");

        playerStats.useStamina(playerStats.getRollingCost());
        StartCoroutine(RollDodge());
    }

    private IEnumerator RollDodge()
    {
        float timeInRoll = 0;
        isRolling = true;
        do
        {
            float jumpForce = rollCurve.Evaluate(timeInRoll);
            controller.Move(playerBody.transform.forward * jumpForce * rollingSpeedBoost * Time.deltaTime);
            timeInRoll += Time.deltaTime;
            yield return null;

        } while (timeInRoll < rollTime);

        isRolling = false;
    }

    public bool IsRolling()
    {
        return isRolling;
    }

    public void removeStatic()
    {
        isFrozen = false;
        advancedAni.setIsUsingSword(false);
    }

    public void startGame()
    {
        hasGameStarted = true;
        ani.Play("StandUp");
    }
}
