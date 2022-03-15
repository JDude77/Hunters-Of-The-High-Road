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

    //Adds a list of event responses to the dictionary
    public void InitResponses<T>(List<T> responses) where T : EventResponse
    {
        foreach (EventResponse r in responses)
        {
            //Gives each response a reference to the animator and audiosource
            r.InitDependencies(ref animator, ref audioSource);
            //Add it to the dictionary
            eventDictionary.Add(r.GetEventName(), r.Activate);
        }
    }

    //Invokes the action of a given event
    public void Respond(string s)
    {
        eventDictionary[s]?.Invoke();
    }
    
}
