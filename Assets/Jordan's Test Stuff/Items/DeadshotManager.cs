using System;
using UnityEngine;

public class DeadshotManager : MonoBehaviour
{
    private bool deadshotActive = false;
    [Header("Required Reticle Reference Objects")]
    [SerializeField] private Reticle reticle;

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

    private void Start() {
        if (!reticle) {
            reticle = FindObjectOfType<Reticle>();
        }
    }

    public void DeactivateDeadshot()
    {
        deadshotActive = false;

        shotLineRenderer.enabled = false;
        reticle.Deactivate();
    }//End DeactivateDeadshot

    public void Deadshot()
    {
        deadshotActive = true;

        shotLineRenderer.enabled = true;
        reticle.Activate();        
    }//End Deadshot    

    private void DeadshotSkillCheck()
    {
        deadshotSkillCheckPassed = reticle.IsSuccessful();

        if (deadshotSkillCheckPassed)
        {
            shotLineRenderer.colorGradient = deadshotLineColour;
        }//End if
        else
        {
            shotLineRenderer.colorGradient = defaultLineColour;

            gunAnimator.SetBool("CanDeadshot", false);
        }//End else
    }//End DeadshotSkillCheck    

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
        DeadshotSkillCheck();
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

    //private void MoveReticleToMousePosition()
    //{
    //    Vector3 mousePos = Vector3.zero;

    //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //    Physics.Raycast(ray, out RaycastHit hit);

    //    if (hit.transform != null)
    //    {
    //        mousePos = hit.point;
    //    }//End if

    //    deadshotReticleObject.transform.position = mousePos;
    //}//End MoveReticleToMousePosition

    //private void RotateReticle() {
    //    //Keep rotation bound between 0 and 180
    //    currentReticleRotation.y = currentReticleRotation.y > 180 ? currentReticleRotation.y - 180 : currentReticleRotation.y;
    //    //Continue rotation
    //    currentReticleRotation.y += rotationSpeed * Time.deltaTime;

    //    //Update object's rotation
    //    rotatingSkillCheckPiece.localEulerAngles = currentReticleRotation;
    //}//End RotateReticle
}