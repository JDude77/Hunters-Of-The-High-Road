using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAnimator : MonoBehaviour
{
    [SerializeField]
    private GameObject CamMain, CamSecond;

    private Animator ani;
    private bool switchCam = true;

    // Start is called before the first frame update
    void Start()
    {
        ani = GetComponentInChildren<Animator>();
    }

    public void PlayAnimation(string animationID)
    {
        ani.Play(animationID);
    }

    public void ToggleCameraPos()
    {
        switchCam = !switchCam;

        CamMain.SetActive(switchCam);
        CamSecond.SetActive(!switchCam);
    }
}
