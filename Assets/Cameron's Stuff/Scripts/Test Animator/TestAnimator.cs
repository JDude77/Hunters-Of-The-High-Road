using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAnimator : MonoBehaviour
{
    [SerializeField]
    private GameObject CamMain, CamSecond;

    [Header("Particles")]
    [SerializeField]
    private GameObject ClawParticles;

    [SerializeField]
    private GameObject stompParticles;

    [Header("Claw particles will be applied to these transforms")]
    [SerializeField]
    private Transform[] claws;

    [Header("Foot for stomping")]
    [SerializeField]
    private Transform foot;

    private Animator ani;
    private bool switchCam = true;

    private BossParticlesSystems bossparticles;

    // Start is called before the first frame update
    void Start()
    {
        ani = GetComponentInChildren<Animator>();
        bossparticles = GetComponentInChildren<BossParticlesSystems>();

        //creates a Claw Particle at each claw and assigns its parent
        foreach (var claw in claws)
        {
            GameObject particles = Instantiate(ClawParticles);

            SetAndResetParent(claw, particles.transform);

            bossparticles.AddClawLines(particles);
        }

        //creates the stomp particle and adds it to the foot
        GameObject stompPart = Instantiate(stompParticles);
        SetAndResetParent(foot, stompPart.transform);
        bossparticles.SetStompParticles(stompPart.GetComponent<ParticleSystem>());

    }

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

    //toggles the active camera
    public void ToggleCameraPos()
    {
        switchCam = !switchCam;

        CamMain.SetActive(switchCam);
        CamSecond.SetActive(!switchCam);
    }
}
