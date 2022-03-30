using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BossState : MonoBehaviour
{
    protected static Boss boss;
    protected static GameObject player;
    public EventResponder<string> eventResponder { get; private set; }

    public void Awake()
    {       
        //Gets the Boss component
        boss = GetComponent<Boss>();
        if (boss == null)
        {
            Debug.LogError("ERROR: No Boss component detected");
        }

        //Get a reference to the player
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Error: No object of type Player found");
        }
    }

    protected void Start()
    {
        Animator a = GetComponentInChildren<Animator>();
        if (a != null)
        {
            eventResponder = new EventResponder<string>(a);
        }
    }//End Start

    public virtual void OnEnter() 
    {
        enabled = true;
    }//End OnEnter
    public virtual void OnExit()
    {
        enabled = false;
    }//End OnExit
    public virtual void Run() 
    {
    }
    public virtual void FixedRun()
    {

    }
}
