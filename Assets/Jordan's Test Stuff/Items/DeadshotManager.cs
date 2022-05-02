using System;
using UnityEngine;
using System.Collections.Generic;
public class DeadshotManager : MonoBehaviour
{
    private bool deadshotActive = false;
    [Header("Reticle Overrides")]
    [SerializeField] private bool overrideReticleValues;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float skillCheckAngleSize;
    private Reticle reticle;

    [Header("Stagger Shot Settings")]
    [SerializeField]
    private int deadshotTokensRequiredToStagger;
    private int currentDeadshotTokens = 0;
    public int GetTokenCount()
    {
        return currentDeadshotTokens;
    }//End GetTokenCount

    private void Start() {
        if (!reticle) {
            reticle = FindObjectOfType<Reticle>();
        } else {
            Debug.LogError("No reticle HUD object found");
        }

        if (overrideReticleValues) {
            reticle.OverrideValues(rotationSpeed, skillCheckAngleSize);
        }
    }

    public void DeactivateDeadshot()
    {
        deadshotActive = false;
        reticle.Deactivate();
    }//End DeactivateDeadshot

    public void Deadshot()
    {
        deadshotActive = true;    
        reticle.Activate();        
    }//End Deadshot    

    public bool DeadshotSkillCheckPassed()
    {
        return reticle.IsSuccessful();
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
        if (currentDeadshotTokens > 0) {
            currentDeadshotTokens--;
        }
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

    //private void DeadshotSkillCheck() {
    //    deadshotSkillCheckPassed = reticle.IsSuccessful();

    //    if (deadshotSkillCheckPassed) {
    //        shotLineRenderer.colorGradient = deadshotLineColour;
    //    }//End if
    //    else {
    //        shotLineRenderer.colorGradient = defaultLineColour;
    //    }//End else
    //}//End DeadshotSkillCheck      
    
    //public void SetShotLineRenderer(LineRenderer shotLineRenderer)
    //{
    //    this.shotLineRenderer = shotLineRenderer;
    //}//End SetShotLineRenderer

    //public void SetDefaultLineColour(Gradient defaultLineColour)
    //{
    //    this.defaultLineColour = defaultLineColour;
    //}//End SetDefaultLineColour

    //public void SetDeadshotLineColour(Gradient deadshotLineColour)
    //{
    //    this.deadshotLineColour = deadshotLineColour;
    //}//End SetDeadshotLineColour
}