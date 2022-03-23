using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAnimator : MonoBehaviour
{
    [Header("Particles")]
    [SerializeField]
    private GameObject clawParticles;

    [SerializeField]
    private GameObject stompParticles;

    [SerializeField]
    private GameObject burrowParticles;

    [Header("Claw particles will be applied to these transforms")]
    [SerializeField]
    private Transform[] claws;

    [Header("Foot for stomping")]
    [SerializeField]
    private Transform foot;

    [Header("Burrowing Particles location")]
    [SerializeField]
    private Transform burrowParticleLocation;

    private Animator ani;

    private BossParticlesSystems bossparticles;

    // Start is called before the first frame update
    void Start()
    {
        ani = GetComponentInChildren<Animator>();
        bossparticles = GetComponentInChildren<BossParticlesSystems>();

        //creates a Claw Particle at each claw and assigns its parent
        foreach (var claw in claws)
        {
            GameObject particles = Instantiate(clawParticles);

            SetAndResetParent(claw, particles.transform);

            bossparticles.AddClawLines(particles);
        }

        //creates the stomp particle and adds it to the foot transform
        GameObject stompPart = Instantiate(stompParticles);
        SetAndResetParent(foot, stompPart.transform);
        bossparticles.SetStompParticles(stompPart.GetComponent<ParticleSystem>());

        //creates the burrow particle and adds it to the burrowing position (probably its hands idk)
        GameObject burrowPart = Instantiate(burrowParticles);
        SetAndResetParent(burrowParticleLocation, burrowPart.transform);
        bossparticles.SetBurrowParticles(burrowPart.GetComponent<ParticleSystem>());
    }

    //takes in a parent transform and adds an object to its hierachy 
    private void SetAndResetParent(Transform parent, Transform toAdd)
    {
        toAdd.transform.parent = parent;
        toAdd.transform.localPosition = Vector3.zero;
        toAdd.transform.localEulerAngles = Vector3.zero;
    }

    //used for buttons - takes in a animation ID and plays the corresponding animation
    public void PlayAnimation(string animationID)
    {
        ani.Play(animationID);
    }
}
