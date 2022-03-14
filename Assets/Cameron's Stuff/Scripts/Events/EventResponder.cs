using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventResponder : MonoBehaviour
{
    public Dictionary<string, Action> eventDictionary;

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
}
