using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public struct Particle
{
    public GameObject particleObject;
    public Transform spawnTransform;
}

[Serializable]
public struct BossEventResponse : IEventResponse
{
    public BossEvents eventName;
    public string nameAsString { get { return eventName.ToString(); } }

    public List<Particle> particles;
    public List<AudioClip> soundEffects;
    public List<string> animationNames;

    private Animator animator;
    private AudioSource audio;

    public void Init(Animator a, AudioSource s)
    {
        this.animator = a;
        this.audio = s;
    }

    public void Activate()
    {
        foreach (Particle particle in particles)
        {
            MonoBehaviour.Instantiate(particle.particleObject, particle.spawnTransform.position, particle.spawnTransform.rotation);
        }

        foreach (AudioClip sound in soundEffects)
        {
            audio.clip = sound;
            audio.Play();
        }

        foreach (string animation in animationNames)
        {
            animator.Play(animation);
        }
    }
}

public class BossEventResponses : EventResponder
{

    [SerializeField] public List<BossEventResponse> responses;

    private void Start()
    {
        foreach (BossEventResponse obj in responses)
        {
            obj.Init(animator, audioSource);
            eventDictionary.Add(obj.nameAsString, obj.Activate);
        }
    }

    private void OnDestroy()
    {
        foreach (BossEventResponse obj in responses)
        {
            eventDictionary[obj.nameAsString] -= obj.Activate;
            eventDictionary.Remove(obj.nameAsString);
        }
    }
}
