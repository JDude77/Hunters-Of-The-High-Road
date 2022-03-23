using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cubetest : MonoBehaviour
{
    public Rigidbody body;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {

        body.angularVelocity = new Vector3(0, 100f, 0); 
    }
}
