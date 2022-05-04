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
    private float particleTimer;
    private float timeBetweenParticleBursts;
    private Vector3 targetPosition;
    [SerializeField] private GameObject particles;
    [SerializeField] private GameObject mesh;
    [SerializeField] private float materialRotationSpeed;
    private float rot;
    // Start is called before the first frame update
    void Start()
    {
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

    private void Update() {
        particleTimer += Time.deltaTime;
        if(particleTimer >= timeBetweenParticleBursts && particles) {

            Instantiate(particles, transform.position, Quaternion.AngleAxis(180f, Vector3.up) * transform.rotation);
            particleTimer = 0;
        }

        rot -= materialRotationSpeed * Time.deltaTime;
        mesh.transform.rotation = Quaternion.AngleAxis(rot, Vector3.right);
    }//End Update

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

    public void Init(float rotationSpeed,float rotationSpeedIncreaseRate, float speed, float timeBetweenParticleBursts)
    {
        this.rotationSpeed = rotationSpeed;
        this.rotationSpeedIncreaseRate = rotationSpeedIncreaseRate;
        this.speed = speed;
        this.timeBetweenParticleBursts = timeBetweenParticleBursts;
        particleTimer = 0f;
        rot = 0;
    }//End Init

    //Called every frame in the boss's burrow state script
    public void SetPlayerPosition(Vector3 position)
    {
        targetPosition = position;
    }//End SetPlayerPosition

    public void OnTriggerEnter(Collider other)
    {
        IDestructible obj = other.GetComponent<IDestructible>();

        if (obj != null)
        {
            obj.DestroyObject();
        }
    }
}
