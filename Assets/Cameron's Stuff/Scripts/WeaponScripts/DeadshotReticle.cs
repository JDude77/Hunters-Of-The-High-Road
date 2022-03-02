using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadshotReticle : MonoBehaviour
{
    [SerializeField]
    private float minSuccessfulAngle, maxSuccessfulAngle, timerBoost;

    [SerializeField]
    private Transform rotatingBit;

    private Transform cameraTransform;
    private Vector3 rotationAngle = Vector3.zero;

    private LineRenderer aimLine;
    private Animator ani;

    private Color aimColor;

    // Start is called before the first frame update
    void Start()
    {
        //a disgusting line of code deprived from the ancient ones. DO NOT SPEAK!
        cameraTransform = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(cameraTransform, transform.up);
    }

    public void DeadShot()
    {
        gameObject.SetActive(true);

        MoveToMouse();
        SkillCheck();
    }

    public void DeactivateReticle()
    {
        gameObject.SetActive(false);
        rotationAngle.y = 0;
    }   

    private void SkillCheck()
    {
        RotateSkillCheck();

        if(rotationAngle.y >= minSuccessfulAngle && rotationAngle.y <= maxSuccessfulAngle)
        {
            aimLine.startColor = Color.red;
            aimLine.endColor = Color.red;
        }
        else
        {
            aimLine.startColor = aimColor;
            aimLine.endColor = aimColor;

            ani.SetBool("CanDeadshot", false);
        }
    }

    private void RotateSkillCheck()
    {
        if (rotationAngle.y > 180)
        {
            rotationAngle.y = 0;
        }
        else
        {
            rotationAngle.y += timerBoost * Time.deltaTime;
        }

        rotatingBit.localEulerAngles = rotationAngle;
    }

    private void MoveToMouse()
    {
        Vector3 MousePos = Vector3.zero;

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out hit);

        if (hit.transform != null)
        {
            MousePos = hit.point;
        }

        transform.position = MousePos;
    }

    public void GetGunInfo(LineRenderer line, Animator animator, Color aimColor)
    {
        aimLine = line;
        ani = animator;
        this.aimColor = aimColor;
    }
}
