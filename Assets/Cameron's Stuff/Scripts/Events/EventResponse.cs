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
public class EventResponse
{
    public List<Particle> particles;
    public List<AudioClip> soundEffects;
    public List<string> animationNames;

    protected Animator animator;
    protected AudioSource audio;

    public virtual void InitDependencies(ref Animator a, ref AudioSource s)
    {
        this.animator = a;
        this.audio = s;
    }

    public void Activate()
    {
        particles.ForEach(p => MonoBehaviour.Instantiate(p.particleObject, p.spawnTransform.position, p.spawnTransform.rotation));

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

    public virtual string GetEventName() {
        Debug.LogWarning("GetEventName not overridden on event response");
        return " ";
    }
}
