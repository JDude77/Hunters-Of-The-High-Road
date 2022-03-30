using System.Collections.Generic;
using UnityEngine;
using System;

public class EventResponder<T>
{
    //Stores sound, and sound target
    public struct SoundEffect
    {
        public AK.Wwise.Event sound { get; private set; }
        public GameObject target { get; private set; }

        public SoundEffect(AK.Wwise.Event sound, GameObject target)
        {
            this.sound = sound;
            this.target = target;
        }//End Constructor
    }//End SoundEffect

    //Stores particle, spawn position and spawn rotation
    public struct ObjectInfo
    {
        public GameObject effect { get; private set; }
        public Vector3 position { get; private set; }
        public Quaternion rotation { get; private set; }

        public ObjectInfo(GameObject effect, Vector3 position, Quaternion rotation)
        {
            this.effect = effect;
            this.position = position;
            this.rotation = rotation;
        }//End Constructor
    }//End ObjectInfo

    //Stores animation name and if it is a trigger
    public struct Animation
    {
        public string name { get; private set; }
        public bool isTrigger { get; private set; }

        public Animation(string name, bool isTrigger)
        {
            this.name = name;
            this.isTrigger = isTrigger;
        }//End Constructor
    }//End Animation

    //Initialise with a type to use for the dictionary key
    private Dictionary<T, ObjectInfo> objectDictionary = new Dictionary<T, ObjectInfo>();
    private Dictionary<T, SoundEffect> soundDictionary = new Dictionary<T, SoundEffect>();
    private Dictionary<T, Animation> animationDictionary = new Dictionary<T, Animation>();
    private Dictionary<T, Action> actionDictionary = new Dictionary<T, Action>();

    private Animator animator;

    public EventResponder(Animator animator)
    {
        this.animator = animator;
    }//End Constructor

    //Adds a particle effect to the particle dictionary
    public void AddInstantiateObject(T key, GameObject objectToAdd, Vector3 spawnPosition, Quaternion rotation)
    {
        ObjectInfo obj = new ObjectInfo(objectToAdd, spawnPosition, rotation);

        if (!objectDictionary.ContainsKey(key)) 
        {
            objectDictionary.Add(key, obj);
        }
    }//End AddParticleEffect

    //Adds a sound to the sound dictionary
    public void AddSoundEffect(T key, AK.Wwise.Event soundEffect, GameObject target)
    {
        SoundEffect sound = new SoundEffect(soundEffect, target);

        if (!soundDictionary.ContainsKey(key))
        {
            soundDictionary.Add(key, sound);
        }
    }//End AddSoundEffect

    //Adds an animation to the animation dictionary
    public void AddAnimation(T key, string animationName, bool isTrigger)
    {
        Animation animation = new Animation(animationName, isTrigger);
            
        if (!animationDictionary.ContainsKey(key))
        {
            animationDictionary.Add(key, animation);
        }
    }//End AddSAnimation

    //Adds an action to the action dictionary
    public void AddAction(T key, Action action)
    {
        if (!actionDictionary.ContainsKey(key))
        {
            actionDictionary.Add(key, action);
        }//End if
    }//End AddSAnimation

    //Invoke a key 
    public void Activate(T key)
    {
        if (soundDictionary.ContainsKey(key))
        {
            SoundEffect s = soundDictionary[key];
            s.sound.Post(s.target);
        }//End if

        if (objectDictionary.ContainsKey(key))
        {
            ObjectInfo p = objectDictionary[key];
            MonoBehaviour.Instantiate(p.effect, p.position, p.rotation);
        }//End if

        if (animationDictionary.ContainsKey(key) && animator)
        {
            Animation a = animationDictionary[key];
            if (a.isTrigger)
            {
                animator.SetTrigger(a.name);
            }
            else
            {
                animator.Play(a.name);
            }
        }//End if

        //Invoke the action
        if (actionDictionary.ContainsKey(key))
        {
            actionDictionary[key]?.Invoke();
        }//End if
    }//End Activate
}
