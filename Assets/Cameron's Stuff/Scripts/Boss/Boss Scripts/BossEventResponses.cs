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
public struct ParticleOnEvent
{
    public string actionName;
    public List<Particle> particles;
    public List<GameObject> soundEffects;
    public List<GameObject> animations;

    public void function()
    {
        foreach (Particle particle in particles)
        {
            MonoBehaviour.Instantiate(particle.particleObject, particle.spawnTransform.position, particle.spawnTransform.rotation);
        }

        foreach (GameObject sound in soundEffects)
        {
            //Play sounds
        }

        foreach (GameObject animation in animations)
        {
            //Play animation
        }
    }
}

public class BossEventResponses : MonoBehaviour
{
    [SerializeField] public List<ParticleOnEvent> particleEvents;
    private List<Action> actions;
    public static BossEventResponses current;

    private void Awake()
    {
        current = this;        
    }

    private void Start()
    {
        foreach (ParticleOnEvent obj in particleEvents)
        {
            if (BossEventsHandler.current.actionToString.TryGetValue(obj.actionName, out Action action))
            {
                Debug.Log("Adding action");
                BossEventsHandler.current.actionToString[obj.actionName] += obj.function;
            }
        }
    }

    private void OnDestroy()
    {
        foreach (ParticleOnEvent obj in particleEvents)
        {
            if (BossEventsHandler.current.actionToString.TryGetValue(obj.actionName, out Action action))
            {
                BossEventsHandler.current.actionToString[obj.actionName] -= obj.function;
            }
        }
    }
}
