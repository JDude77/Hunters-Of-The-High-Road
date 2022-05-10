using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossParticlesSystems : MonoBehaviour
{
    private List<GameObject> clawLines = new List<GameObject>();

    private ParticleSystem stompParticles;

    private ParticleSystem burrowingParticles;

    private CameraShakeScript camShake;

    // Start is called before the first frame update
    void Start()
    {
        camShake = CameraShakeScript.Instance;
    }

    public void AddClawLines(GameObject lineRender)
    {
        clawLines.Add(lineRender);
        lineRender.SetActive(false);
    }

    public void SetStompParticles(ParticleSystem stomp)
    {
        stompParticles = stomp; 
    }

    public void SetBurrowParticles(ParticleSystem burrow)
    {
        burrowingParticles = burrow;
    }

    //Used by animators, they can't use booleans :|
    public void SetClawLines(int isActive)
    {
        foreach (var claw in clawLines)
        {
            claw.SetActive(isActive >= 1);
        }
    }

    //used by animators
    public void PlayStompParticles()
    {
        stompParticles.Play();
        camShake.ShakeCamera(1, 0.5f);
    }

    public void PlayBurrowParticles()
    {
        burrowingParticles.Play();
        camShake.ShakeCamera(2, 0.5f);
    }
}
