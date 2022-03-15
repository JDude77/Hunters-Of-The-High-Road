using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public struct Particle
{
    public GameObject particleObject;
    public Transform spawnTransform;
}//End Particle

[Serializable]
public class EventResponse
{
    //List of things that should activate on the specific event
    public List<Particle> particles;
    public List<AudioClip> soundEffects;
    public List<string> animationNames;
    //Dependencies
    protected Animator animator;
    protected AudioSource audio;

    public virtual void InitDependencies(ref Animator a, ref AudioSource s)
    {
        this.animator = a;
        this.audio = s;
    }//End InitDependencies

    //Loops through each list, instantiates particles, plays sounds, and starts animations
    public void Activate()
    {
        particles.ForEach(p => MonoBehaviour.Instantiate(p.particleObject, p.spawnTransform.position, p.spawnTransform.rotation));

        foreach (AudioClip sound in soundEffects)
        {
            audio.clip = sound;
            if(!audio.isPlaying)
                audio.Play();
        }

        foreach (string animation in animationNames)
        {
            animator.Play(animation);
        }
    }// End Activate

    //Throws error if function is not overriden 
    public virtual string GetEventName() {
        Debug.LogError("GetEventName not overridden on event response");
        return " ";
    }//End GetEventName
}
