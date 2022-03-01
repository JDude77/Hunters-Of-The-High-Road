using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;

public class PlayerControllerPrototype : MonoBehaviour
{
    enum State
    {
        NotDodging, Dodging
    }

    Rigidbody rb;
    public float speed = 5f;
    public float rollDistance = 1.0f;
    public float rollTime = 0.2f;
    public float rollStaminaCost = 0.5f;
    private float rollTimer = 0.0f;

    [ReadOnly]
    public float stamina = 1.0f;

    public float staminaRegenRate = 0.33f;
    Vector3 startPos;
    Vector3 target;
    State state = State.NotDodging;

    void Start()
    {
        //Fetch the Rigidbody from the GameObject with this script attached
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (stamina < 1.0f)
            stamina += Time.deltaTime * staminaRegenRate;

        if (state != State.Dodging)
        {
            if (Input.GetButtonDown("Fire2") && stamina > rollStaminaCost)
            {
                stamina -= rollStaminaCost;
                Debug.Log(stamina + " Stamina remaining");
                state = State.Dodging;
                startPos = transform.position;
                rollTimer = 0.0f;
                Vector3 direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
                direction = direction.normalized;
                target = transform.position + (direction * rollDistance);
            }
        }
    }

    void FixedUpdate()
    {
        Vector3 inputVector = new Vector3(0,0,0);

        switch (state)
        {
            case State.Dodging:
                rollTimer += Time.deltaTime;
                Roll();
                break;
            case State.NotDodging:
                //Store user input as a movement vector
                inputVector = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

                //Apply the movement vector to the current position, which is
                //multiplied by deltaTime and speed for a smooth MovePosition
                rb.MovePosition(transform.position + inputVector * Time.deltaTime * speed);
                break;
        }
    }

    void Roll()
    {
        float t = rollTimer / rollTime;
        t = Mathf.Sin(t * Mathf.PI * 0.5f);
        rb.MovePosition(Vector3.Lerp(startPos, target, t));

        if (t >= 1.0f)
        {
            state = State.NotDodging;
        }
    }

    public Vector3 GetVelocity()
    {
        return rb.velocity;
    }
}

