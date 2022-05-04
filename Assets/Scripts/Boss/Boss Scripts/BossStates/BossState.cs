using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class BossState : MonoBehaviour
{
    protected static Boss boss;
    protected static GameObject player;
    protected bool canBeStunned = false;
    public bool stunnable { get { return canBeStunned; } }
    public EventResponder<string> eventResponder { get; private set; }

    public void Awake()
    {       
        //Gets the Boss component
        boss = GetComponent<Boss>();
        if (boss == null)
        {
            Debug.LogError("ERROR: No Boss component detected");
        }//End if

        //Get a reference to the player
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Error: No object of type Player found");
        }//End if
    }//End Awake

    protected void Start()
    {
        Animator a = GetComponentInChildren<Animator>();
        if (a != null)
        {
            eventResponder = new EventResponder<string>(a);
        }//End if
    }//End Start

    public virtual void OnEnter() 
    {
        enabled = true;
    }//End OnEnter
    public virtual void OnExit()
    {
        enabled = false;
        StopAllCoroutines();
    }//End OnExit
    public virtual void Run() 
    {
    }//End Run
    public virtual void FixedRun()
    {
    }//End FixedRun
}
