using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventResponder : MonoBehaviour
{
    private Dictionary<string, Action> eventDictionary;

    public Animator animator;

    public AudioSource audioSource;

    private void Awake()
    {
        eventDictionary = new Dictionary<string, Action>();

        animator = GetComponentInChildren<Animator>();
        audioSource = GetComponent<AudioSource>();

        if (animator == null) Debug.LogWarning("No Animator detected");

        if (audioSource == null) {
            audioSource = gameObject.AddComponent<AudioSource>();        
        }
    }
    public void InitResponses<T>(List<T> responses) where T : EventResponse
    {
        foreach (EventResponse r in responses)
        {
            r.InitDependencies(ref animator, ref audioSource);
            eventDictionary.Add(r.GetEventName(), r.Activate);
        }
    }
    public void Respond(string s)
    {
        eventDictionary[s]?.Invoke();
    }
    
}
