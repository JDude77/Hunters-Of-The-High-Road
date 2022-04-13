using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BurrowMovement : MonoBehaviour
{
    private Rigidbody body;
    private float rotationSpeed;
    private float rotationSpeedIncreaseRate;
    private float speed;
    private Vector3 targetPosition;

    // Start is called before the first frame update
    void Start()
    {
        if (!GetComponent<ParticleSystem>())
        {
            Debug.LogWarning("WARNING: No particle system detected on burrow movement object.");
        }

        //Set up the rigidbody
        body = GetComponent<Rigidbody>();
        if (body == null)
        {
            //Add the component
            body = gameObject.AddComponent<Rigidbody>();
        }

        body.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        body.useGravity = false;
        body.isKinematic = false;
    }//End Start

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 targetDirection = targetPosition - transform.position;
        targetDirection.Normalize();
        //Get the rotation amount in terms of the cross product as a scalar
        float rotationAmount = Vector3.Cross(targetDirection, transform.forward).y;
        //Rotate around the y axis by the rotation amount * rotation speed
        body.angularVelocity = new Vector3(0, -rotationAmount * rotationSpeed, 0);
        body.velocity = transform.forward * speed;

        rotationSpeed += rotationSpeedIncreaseRate;
    }//End FixedUpdate

    public void Init(float rotationSpeed,float rotationSpeedIncreaseRate, float speed)
    {
        this.rotationSpeed = rotationSpeed;
        this.rotationSpeedIncreaseRate = rotationSpeedIncreaseRate;
        this.speed = speed;
    }//End Init

    //Called every frame in the boss's burrow state script
    public void SetPlayerPosition(Vector3 position)
    {
        targetPosition = position;
    }//End SetPlayerPosition
}
