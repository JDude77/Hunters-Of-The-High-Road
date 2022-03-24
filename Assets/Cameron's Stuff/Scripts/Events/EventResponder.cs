using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventResponder : MonoBehaviour
{
    //A dictionary of the event names and the functions that create particles, sounds and animations
    private Dictionary<string, Action> eventDictionary;

    private Animator animator;
    private AudioSource audioSource;

    private void Awake()
    {
        //Create a new instance of the dictionary
        eventDictionary = new Dictionary<string, Action>();

        //Get the required dependencies
        animator = GetComponentInChildren<Animator>();
        if (animator == null) Debug.LogWarning("No Animator detected");

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) { audioSource = gameObject.AddComponent<AudioSource>(); }
    }

    //Adds the events to the event dictionary
    //Pass in the list of event responses. Must be of type EventResponse
    //Pass GetType() into the type parameter to give each event a unique identifier
    public void InitResponses<T>(List<T> responses, Type type, GameObject soundTarget) where T : EventResponse
    {
        foreach (EventResponse r in responses)
        {
            //Create a new key that is the type of class calling the events, and the event names
            string uniqueIdentifier = type.ToString() + r.GetEventName();
            //If the event does not already exist
            if (!eventDictionary.ContainsKey(uniqueIdentifier))
            {
                Debug.Log(uniqueIdentifier);
                //Gives each response a reference to the animator and audiosource
                r.InitDependencies(ref animator, ref audioSource, ref soundTarget);
                //Add it to the dictionary
                eventDictionary.Add(uniqueIdentifier, r.Activate);
            }
        }
    }//End InitResponses

    //Invokes the action of a given event
    public void Respond(string s)
    {
        if (eventDictionary.ContainsKey(s))
        {
            eventDictionary[s]?.Invoke();
        }
    }//End Respond
    
}
