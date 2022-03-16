using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class BossEventResponse : EventResponse
{
    public BossEvent eventName;
    public override string GetEventName() { return eventName.ToString(); }
}//End BossEventResponse

public class BossState : MonoBehaviour
{
    protected static Boss boss;
    protected static GameObject player;
    [SerializeField] private List<BossEventResponse> eventResponses;
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

        boss.eventResponder.InitResponses(eventResponses, GetType());        
    }//End Start

    public virtual void InvokeEvent<T>(T e)
    {
        Debug.Log("Invoking: " + GetType() + e.ToString());
        boss.eventResponder.Respond(GetType().ToString() + e.ToString());
    }//End InvokeEvent

    public virtual void OnEnter() 
    {
        InvokeEvent(BossEvent.StateEnter);
    }//End OnEnter
    public virtual void OnExit()
    {
        InvokeEvent(BossEvent.StateExit);
    }//End OnExit
    public virtual void Run() 
    {
    }
    public virtual void FixedRun()
    {

    }
}
