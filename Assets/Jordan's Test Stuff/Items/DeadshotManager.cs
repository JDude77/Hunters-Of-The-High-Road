using System;
using UnityEngine;

public class DeadshotManager : MonoBehaviour
{
    private Camera mainCamera;
    private bool deadshotActive = false;
    [Header("Required Reticle Reference Objects")]
    [SerializeField]
    private GameObject deadshotReticleObject;
    [SerializeField]
    private Transform rotatingSkillCheckPiece;

    private Vector3 currentReticleRotation = Vector3.zero;

    [Header("Skill Angle Settings")]
    [SerializeField]
    private float skillCheckAngleCentrePoint;
    [SerializeField]
    private float skillCheckAngleSize;
    [SerializeField]
    private float rotationSpeed;

    private float maxSuccessAngle, minSuccessAngle;

    private LineRenderer shotLineRenderer;
    private Gradient defaultLineColour;
    private Gradient deadshotLineColour;
    private Animator gunAnimator;

    [Header("Stagger Shot Settings")]
    [SerializeField]
    private int deadshotTokensRequiredToStagger;
    private int currentDeadshotTokens = 0;
    public int GetTokenCount()
    {
        return currentDeadshotTokens;
    }//End GetTokenCount
    private bool deadshotSkillCheckPassed = false;

    private void Awake()
    {
        mainCamera = Camera.main;

        maxSuccessAngle = skillCheckAngleCentrePoint + (skillCheckAngleSize / 2.0f);
        minSuccessAngle = skillCheckAngleCentrePoint - (skillCheckAngleSize / 2.0f);
    }//End Awake

    private void Update()
    {
        deadshotReticleObject.transform.LookAt(mainCamera.transform, transform.up);

        if(deadshotActive)
        {
            UpdateDeadshot();
        }//End if
    }//End Update

    public void DeactivateDeadshot()
    {
        deadshotActive = false;

        shotLineRenderer.enabled = false;
        deadshotReticleObject.SetActive(false);
    }//End DeactivateDeadshot

    public void Deadshot()
    {
        deadshotActive = true;

        shotLineRenderer.enabled = true;
        deadshotReticleObject.SetActive(true);
    }//End Deadshot

    private void UpdateDeadshot()
    {
        MoveReticleToMousePosition();
        RotateReticle();
        DeadshotSkillCheck();
    }//End UpdateDeadshot

    private void MoveReticleToMousePosition()
    {
        Vector3 mousePos = Vector3.zero;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out RaycastHit hit);

        if (hit.transform != null)
        {
            mousePos = hit.point;
        }//End if

        deadshotReticleObject.transform.position = mousePos;
    }//End MoveReticleToMousePosition

    private void DeadshotSkillCheck()
    {
        deadshotSkillCheckPassed = currentReticleRotation.y >= minSuccessAngle && currentReticleRotation.y <= maxSuccessAngle;

        if(deadshotSkillCheckPassed)
        {
            shotLineRenderer.colorGradient = deadshotLineColour;
        }//End if
        else
        {
            shotLineRenderer.colorGradient = defaultLineColour;

            gunAnimator.SetBool("CanDeadshot", false);
        }//End else
    }//End DeadshotSkillCheck

    private void RotateReticle()
    {
        //Keep rotation bound between 0 and 180
        currentReticleRotation.y = currentReticleRotation.y > 180 ? currentReticleRotation.y - 180 : currentReticleRotation.y;
        //Continue rotation
        currentReticleRotation.y += rotationSpeed * Time.deltaTime;

        //Update object's rotation
        rotatingSkillCheckPiece.localEulerAngles = currentReticleRotation;
    }//End RotateReticle

    public void SetShotLineRenderer(LineRenderer shotLineRenderer)
    {
        this.shotLineRenderer = shotLineRenderer;
    }//End SetShotLineRenderer

    public void SetDefaultLineColour(Gradient defaultLineColour)
    {
        this.defaultLineColour = defaultLineColour;
    }//End SetDefaultLineColour

    public void SetDeadshotLineColour(Gradient deadshotLineColour)
    {
        this.deadshotLineColour = deadshotLineColour;
    }//End SetDeadshotLineColour

    public void SetGunAnimator(Animator gunAnimator)
    {
        this.gunAnimator = gunAnimator;
    }//End SetGunAnimator

    public bool DeadshotSkillCheckPassed()
    {
        bool checkPassed = deadshotSkillCheckPassed;
        deadshotSkillCheckPassed = false;
        return checkPassed;
    }//End DeadshotSkillCheckPassed

    public bool CanStagger()
    {
        return currentDeadshotTokens == deadshotTokensRequiredToStagger;
    }//End CanStagger

    public void ResetTokens()
    {
        currentDeadshotTokens = 0;
    }//End ResetTokens

    [ContextMenu("Add Token")]
    public void AddToken()
    {
        currentDeadshotTokens++;
    }//End AddToken

    [ContextMenu("Remove Token")]
    public void RemoveToken()
    {
        currentDeadshotTokens -= 1;
        if (currentDeadshotTokens < 0) currentDeadshotTokens = 0;
    }//End RemoveToken
}