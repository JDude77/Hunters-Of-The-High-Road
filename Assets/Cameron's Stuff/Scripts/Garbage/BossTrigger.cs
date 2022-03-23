using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class BossTrigger : MonoBehaviour
{
    public Action TriggerActivated;
    // Start is called before the first frame update    

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            TriggerActivated?.Invoke();
            GetComponent<Collider>().enabled = false;
        }
    }
}
