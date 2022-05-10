using UnityEngine;

public class AnimatorRedirector : MonoBehaviour
{
    [Header("CamShake Values")]
    [SerializeField]
    private float Intensity, Time;

    [SerializeField]
    private PlayerAdvancedAnimations advAni;

    private bool isAiming = false;

    private void Update()
    {
        if(isAiming)
        {
            advAni.SetAimingRig(true);
        }
        else
        {
            advAni.SetAimingRig(false);
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

    public void debugForAnimation()
    {
        Debug.Break();
    }
}