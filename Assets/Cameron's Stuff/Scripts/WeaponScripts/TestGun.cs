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
    private float shootHeight;

    [SerializeField]
    private Color normalColor, flashingColor;

    [SerializeField]
    private GameObject flash;

    private LineRenderer Line;

    [SerializeField]
    private LineRenderer leftLine, rightLine;

    private BasicPlayerController playerController;

    private Animator ani;

    private ParticleSystem partSystem;

    private bool isReloaded = true;

    // Start is called before the first frame update
    void Start()
    {
        Line = GetComponent<LineRenderer>();
        playerController = GetComponentInParent<BasicPlayerController>();
        ani = GetComponent<Animator>();
        partSystem = GetComponentInChildren<ParticleSystem>();
    }

    public void drawLines()
    {
        ani.Play("closeAimAngle");

        leftLine.SetPosition(0, leftLine.transform.position);
        rightLine.SetPosition(0, rightLine.transform.position);

        //left line stuff
        Ray leftray = new Ray(leftLine.transform.position, leftLine.transform.forward);
        RaycastHit hit;

        if(Physics.Raycast(leftray, out hit, 100))
        {
            Vector3 positionWithOffset = hit.point;
            positionWithOffset.y = getAimHeight();

            leftLine.SetPosition(1, positionWithOffset);
        }
        else
        {
            leftLine.SetPosition(1, leftLine.transform.TransformDirection(Vector3.forward) * 1000);
        }

        //right line stuff
        Ray rightray = new Ray(rightLine.transform.position, rightLine.transform.forward);
        RaycastHit hit2;

        if (Physics.Raycast(rightray, out hit2, 100))
        {
            Vector3 positionWithOffset = hit2.point;
            positionWithOffset.y = getAimHeight();

            rightLine.SetPosition(1, positionWithOffset);
        }
        else
        {
            rightLine.SetPosition(1, rightLine.transform.TransformDirection(Vector3.forward) * 1000);
        }
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
            CameraShakeScript.Instance.ShakeCamera(shakeIntensity, shakeTime);

            Collider[] hits = Physics.OverlapSphere(getShootPos(), 0.5f);

            foreach (var hit in hits)
            {
                if (hit.tag == "Chain")
                {
                    hit.GetComponentInParent<ChainDoorScript>().openDoor();
                    hit.gameObject.SetActive(false);
                }

                if(hit.tag == "Tombstone")
                {
                    hit.GetComponentInParent<DestructibleTombstone>().destroyTombstone();
                }

                if (hit.tag == "Bottle")
                {
                    hit.GetComponentInParent<TutorialBottle>().shootBottle();
                }
            }

            isReloaded = false;
            StartCoroutine(reloadGun());
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

    public float getAimHeight()
    {
        return shootHeight;
    }

    public void setWhiteLine()
    {
        leftLine.endColor = normalColor;
        leftLine.startColor = normalColor;

        rightLine.endColor = normalColor;
        rightLine.startColor = normalColor;
    }

    public void setRedLine()
    {
        leftLine.endColor = flashingColor;
        leftLine.startColor = flashingColor;

        rightLine.endColor = flashingColor;
        rightLine.startColor = flashingColor;
    }

    public void activateCanDeadshot()
    {

    }
}
