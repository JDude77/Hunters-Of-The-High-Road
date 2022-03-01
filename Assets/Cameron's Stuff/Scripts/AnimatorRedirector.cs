using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class AnimatorRedirector : MonoBehaviour
{
    [Header("CamShake Values")]
    [SerializeField]
    private float Intensity, Time;

    [SerializeField]
    private PlayerAdvancedAnimations advAni;

    [SerializeField]
    private BasicPlayerController controller;

    [SerializeField]
    private TestSword sword;

    private bool isAiming = false;

    private void Update()
    {
        if(isAiming)
        {
            advAni.setAimingRig(true);
        }
        else
        {
            advAni.setAimingRig(false);
        }
    }

    public void shakeCamera()
    {
        CameraShakeScript.Instance.ShakeCamera(Intensity, Time);
    }

    public void activateAimRig()
    {
        isAiming = true;
    }

    public void deactivateAimRig()
    {
        isAiming = false;
    }

    public void stopStaticPlayer()
    {
        controller.removeStatic();
    }

    public void debugForAnimation()
    {
        Debug.Break();
    }

    public void SwordCollision()
    {
        sword.checkHit();
    }
}
