using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainDoorScript : MonoBehaviour
{
    private Animator ani;

    // Start is called before the first frame update
    void Start()
    {
        ani = GetComponent<Animator>();    
    }

    public void openDoor()
    {
        ani.Play("OpenDoor");
    }
}
