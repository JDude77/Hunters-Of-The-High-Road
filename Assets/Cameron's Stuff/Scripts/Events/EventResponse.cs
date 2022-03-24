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
    [SerializeField] private string Label;
    [Space(10)]
    [HideInInspector] public string uniqueIdentifier;
    //List of things that should activate on the specific event
    public List<Particle> particles;
    [Space(10)]
    public List<AK.Wwise.Event> soundEffects;
    [Space(5)]
    public string animationToPlay;
    public bool IsTrigger;
    [Space(10)]

    //Dependencies
    protected Animator animator;
    protected GameObject soundTarget;

    private Action<string> doAnimation;

    //Provides the event response with a reference to the audio source and animator
    public virtual void InitDependencies(ref Animator animator, ref GameObject soundTarget)
    {
        this.animator = animator;
        this.soundTarget = soundTarget;

        if (animator != null)
        {
            if (IsTrigger)
                doAnimation = animator.SetTrigger;
            else
                doAnimation = animator.Play;
        }
    }//End InitDependencies

    //Loops through each list, instantiates particles, plays sounds, and starts animations
    public void Activate()
    {
        particles.ForEach(p => MonoBehaviour.Instantiate(p.particleObject, p.spawnTransform.position, p.spawnTransform.rotation));

        foreach (AK.Wwise.Event sound in soundEffects)
        {
            sound.Post(soundTarget);
        }//End foreach

        doAnimation?.Invoke(animationToPlay);
    }// End Activate

    //Throws error if function is not overriden 
    public virtual string GetEventName() {
        Debug.LogError("GetEventName not overridden on event response");
        return " ";
    }//End GetEventName
}
