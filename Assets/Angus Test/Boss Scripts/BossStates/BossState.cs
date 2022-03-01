using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossState : MonoBehaviour
{
    protected static Boss boss = null;
    protected static GameObject player = null;

    protected void Start()
    {
        //Gets the Boss component
        boss = GetComponent<Boss>();
        if(boss == null)
        {
            Debug.LogError("ERROR: No Boss component detected");
        }

        //Get a reference to the player
        player = GameObject.FindGameObjectWithTag("Player");
        if(player == null)
        {
            Debug.LogError("Error: No object of type Player found");
        }
    }

    public virtual void OnEnter() 
    {
        enabled = true;
    }
    public virtual void OnExit() 
    {  
    }
    public virtual void Run() 
    {
    }
    public virtual void FixedRun()
    {
        enabled = false;
    }
}
