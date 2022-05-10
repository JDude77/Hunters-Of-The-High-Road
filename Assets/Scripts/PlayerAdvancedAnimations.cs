using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerAdvancedAnimations : MonoBehaviour
{
    [SerializeField]
    private Transform weaponHolster, gunHandPos, leftHandIKPos, rifle, weaponGrip, swordSheath, swordHandPos, sword;

    [SerializeField]
    private Rig leftHandRig;

    [SerializeField]
    private Rig aimRig;

    private bool isUsingGun = false, isAiming = false, isUsingSword = false;

    private Animator ani;

    private float gunTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        ani = GetComponentInChildren<Animator>();        
    }

    // Update is called once per frame
    void Update()
    {
        leftHandIKPos.position = weaponGrip.position;
        leftHandIKPos.rotation = weaponGrip.rotation;

        if(isUsingGun)
        {
            SetWeaponPosition(rifle, gunHandPos);
            leftHandRig.weight = 1;

            GunTimer();
        }
        else
        {
            SetWeaponPosition(rifle, weaponHolster);
            ani.SetLayerWeight(1, 0);
            leftHandRig.weight = 0;
        }

        if(isUsingSword)
        {
            SetWeaponPosition(sword, swordHandPos);
            gunTime = 5;
        }
        else
        {
            SetWeaponPosition(sword, swordSheath);
        }
    }

    private void GunTimer()
    {
        gunTime += Time.deltaTime;

        if(gunTime >=5)
        {
            isUsingGun = false;
        }
    }

    private void SetWeaponPosition(Transform weapon, Transform toGo)
    {
        weapon.transform.parent = toGo;
        weapon.transform.localEulerAngles = Vector3.zero;
        weapon.transform.localPosition = Vector3.zero;
    }

    public void SetIsUsingGun(bool isUsingGun, float weight)
    {
        this.isUsingGun = isUsingGun;

        ani.SetLayerWeight(1, weight);

        if (isUsingGun == true)
        {
            gunTime = 0;
        }
    }

    public void SetAimingRig(bool isaiming)
    {
        if(isaiming)
        {
            aimRig.weight = 1; 
        }
        else
        {
            aimRig.weight = 0;
        }
    }

    public void SetIsUsingSword(bool isUsingSword)
    {
        this.isUsingSword = isUsingSword;
    }
}
