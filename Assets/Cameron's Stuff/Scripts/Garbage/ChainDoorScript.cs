using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainDoorScript : MonoBehaviour
{
    private Animator ani;
    [SerializeField]
    private GameObject chain;

    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<PlayerEventsHandler>().OnHitChain += OpenDoor;
        FindObjectOfType<BossTrigger>().TriggerActivated += SlamDoor;
        ani = GetComponent<Animator>();
    }

    public void SlamDoor()
    {
        ani.Play("SlamDoor");
    }

    public void OpenDoor(GameObject instance)
    {
        if (instance == chain)
        {
            ani.Play("OpenDoor");
            Destroy(chain);
            //chain.SetActive(false);
            FindObjectOfType<PlayerEventsHandler>().OnHitChain -= OpenDoor;
        }//End if
    }//End OpenDoor
}
