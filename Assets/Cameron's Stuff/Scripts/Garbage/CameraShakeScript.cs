using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShakeScript : MonoBehaviour
{
    public static CameraShakeScript Instance { get; private set; }

    private CinemachineVirtualCamera virtualCamera;
    private CinemachineBasicMultiChannelPerlin perlinChannel;

    private float shakeTimer, shakeTimerTotal;
    private float startingIntensity;

    private Vector3 normalRotation;

    private void Awake()
    {
        Instance = this;

        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        perlinChannel = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        normalRotation = transform.eulerAngles;
    }

    //shakes the camera using cinemachines perlin gain channel
    public void ShakeCamera(float intensity, float time)
    {
        perlinChannel.m_AmplitudeGain = intensity;
        startingIntensity = intensity;

        shakeTimer = time;
        shakeTimerTotal = time;
    }

    private void Update()
    {
        if(shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            if(shakeTimer <= 0f)
            {
                perlinChannel.m_AmplitudeGain = Mathf.Lerp(startingIntensity, 0f, (1 - (shakeTimer / shakeTimerTotal)));
            }
        }
        else
        {
            if(transform.eulerAngles != normalRotation)
            {
                transform.eulerAngles = normalRotation;
            }
        }
    }
}
