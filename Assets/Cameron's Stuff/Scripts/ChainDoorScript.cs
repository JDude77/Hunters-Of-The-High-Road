using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainDoorScript : MonoBehaviour
{
    private Animator ani;

    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<PlayerEventsHandler>().OnHit += OpenDoor;
        ani = GetComponent<Animator>();
    }

    public void OpenDoor(string tag)
    {
        if (tag == "Chain")
        {
            ani.Play("OpenDoor");
            gameObject.SetActive(false);
        }//End if
    }//End OpenDoor
}
