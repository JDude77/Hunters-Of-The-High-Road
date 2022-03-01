using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerAdvancedAnimations : MonoBehaviour
{
    [SerializeField]
    private Transform WeaponHoister, RightHand, leftHandPos, Gun, weaponGrip, swordSheath, SwordPos, Sword;

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
        leftHandPos.position = weaponGrip.position;
        leftHandPos.rotation = weaponGrip.rotation;

        if(isUsingGun)
        {
            setWeaponPosition(Gun, RightHand);
            leftHandRig.weight = 1;

            gunTimer();
        }
        else
        {
            setWeaponPosition(Gun, WeaponHoister);
            ani.SetLayerWeight(1, 0);
            leftHandRig.weight = 0;
        }

        if(isUsingSword)
        {
            setWeaponPosition(Sword, SwordPos);
            gunTime = 5;
        }
        else
        {
            setWeaponPosition(Sword, swordSheath);
        }
    }

    private void gunTimer()
    {
        gunTime += Time.deltaTime;

        if(gunTime >=5)
        {
            isUsingGun = false;
        }
    }

    private void setWeaponPosition(Transform weapon, Transform toGo)
    {
        weapon.transform.parent = toGo;
        weapon.transform.localEulerAngles = Vector3.zero;
        weapon.transform.localPosition = Vector3.zero;
    }

    public void setIsUsingGun(bool isusing, float weight)
    {
        isUsingGun = isusing;

        ani.SetLayerWeight(1, weight);

        if (isusing == true)
        {
            gunTime = 0;
        }
    }

    public void setAimingRig(bool isaiming)
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

    public void setIsUsingSword(bool isusingsword)
    {
        isUsingSword = isusingsword;
    }
}
