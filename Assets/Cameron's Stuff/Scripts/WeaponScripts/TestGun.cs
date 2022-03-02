using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGun : MonoBehaviour
{
    [SerializeField]
    private float shakeIntensity, shakeTime;

    [SerializeField]
    private float reloadTime;

    [SerializeField]
    private Transform muzzle;

    [SerializeField]
    private Transform player;

    [SerializeField]
    private DeadshotReticle Reticle;

    [SerializeField]
    private float shootHeight;

    [SerializeField]
    private Color aimLineColor;

    [SerializeField]
    private GameObject flash;

    private LineRenderer Line;

    [SerializeField]
    private LineRenderer aimLine;

    private BasicPlayerController playerController;

    private Animator ani;

    private ParticleSystem partSystem;

    private bool isReloaded = true;

    private bool isAiming = false, canDeadshot = false;

    // Start is called before the first frame update
    void Start()
    {
        Line = GetComponent<LineRenderer>();
        playerController = GetComponentInParent<BasicPlayerController>();
        ani = GetComponent<Animator>();
        partSystem = GetComponentInChildren<ParticleSystem>();

        Reticle.GetGunInfo(aimLine, ani, aimLineColor);
    }

    private void Update()
    {
        if(isAiming && isReloaded)
        {
            SetAimingPos(getShootPos());
            Reticle.DeadShot();
        }
        else
        {
            Reticle.DeactivateReticle();
        }
    }

    private void SetAimingPos(Vector3 position)
    {
        aimLine.SetPosition(0, muzzle.position);
        aimLine.SetPosition(1, position);
    }

    public void SetAiming(bool isaiming)
    {
        isAiming = isaiming;
    }

    public void Shoot()
    {
        if (playerController.IsRolling() == false && isReloaded)
        {
            Vector3 shootHere = getShootPos();
            Line.SetPosition(1, shootHere);
            Line.SetPosition(0, muzzle.position);

            partSystem.Play();
            StartCoroutine(flashMuzzle());

            if(canDeadshot)
            {
                CameraShakeScript.Instance.ShakeCamera(shakeIntensity * 2, shakeTime);
            }
            else
            {
                CameraShakeScript.Instance.ShakeCamera(shakeIntensity, shakeTime);
            }

            Collider[] hits = Physics.OverlapSphere(getShootPos(), 0.5f);
            hitCheck(hits);

            isReloaded = false;
            StartCoroutine(reloadGun());
        }

        if (isAiming)
        {
            SetAiming(false);
        }
    }

    private static void hitCheck(Collider[] hits)
    {
        foreach (var hit in hits)
        {
            if (hit.tag == "Chain")
            {
                hit.GetComponentInParent<ChainDoorScript>().openDoor();
                hit.gameObject.SetActive(false);
            }

            if (hit.tag == "Tombstone")
            {
                hit.GetComponentInParent<DestructibleTombstone>().destroyTombstone();
            }

            if (hit.tag == "Bottle")
            {
                hit.GetComponentInParent<TutorialBottle>().shootBottle();
            }
        }
    }

    private IEnumerator reloadGun()
    {
        yield return new WaitForSeconds(reloadTime);
        isReloaded = true;
    }

    private IEnumerator flashMuzzle()
    {
        flash.SetActive(true);
        yield return new WaitForSeconds(0.08f);
        flash.SetActive(false);
    }

    private Vector3 getShootPos()
    {
        Vector3 shootHere = Vector3.zero;

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out hit);

        if (hit.transform != null)
        {
            shootHere = hit.point;
        }

        shootHere.y = shootHeight;

        Vector3 lookat = shootHere - player.position;
        lookat.y = 0;
        player.rotation = Quaternion.LookRotation(lookat);

        Ray ray2 = new Ray(muzzle.position, shootHere - muzzle.position);

        if (Physics.Raycast(ray2, out hit, 100))
        {
            shootHere = hit.point;
            shootHere.y = shootHeight;
        }

        return shootHere;
    }

    public void SetAimLineColor(Color color)
    {
        aimLineColor = color;
    }

    public float getAimHeight()
    {
        return shootHeight;
    }
}
