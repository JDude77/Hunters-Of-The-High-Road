using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSword : MonoBehaviour
{
    [SerializeField]
    private Transform checkPos;

    [SerializeField]
    private float hitSize;

    [SerializeField]
    private float shakeIntensity, shakeTime;

    public void checkHit()
    {
        CameraShakeScript.Instance.ShakeCamera(shakeIntensity, shakeTime);

        Collider[] hits = Physics.OverlapSphere(checkPos.position, hitSize);

        foreach (var hit in hits)
        {
            if(hit.tag == "Chain")
            {
                hit.GetComponentInParent<ChainDoorScript>().OpenDoor(hit.gameObject);
                hit.gameObject.SetActive(false);
            }
        }
    }
}
