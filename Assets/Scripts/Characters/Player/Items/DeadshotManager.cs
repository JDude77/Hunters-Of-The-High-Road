using UnityEngine;
public class DeadshotManager : MonoBehaviour
{
    [Header("Reticle Overrides")]
    [SerializeField]
    private bool overrideReticleValues;
    [SerializeField]
    private float rotationSpeed;
    [SerializeField]
    private float skillCheckAngleSize;

    [Header("Reticle")]
    [SerializeField]
    private Reticle reticle;

    [Header("Stagger Shot Settings")]
    [SerializeField]
    private int deadshotTokensRequiredToStagger;
    private int currentDeadshotTokens = 0;
    [HideInInspector] public bool deadShotReady { get { return currentDeadshotTokens >= deadshotTokensRequiredToStagger; } }

    public int GetTokenCount()
    {
        return currentDeadshotTokens;
    }//End GetTokenCount

    private void Start()
    {
        if (!reticle)
        {
            reticle = FindObjectOfType<Reticle>();
        }//End if

        if (overrideReticleValues)
        {
            reticle.OverrideValues(rotationSpeed, skillCheckAngleSize);
        }//End if
    }//End Start

    public void DeactivateDeadshot()
    {
        reticle.Deactivate();
    }//End DeactivateDeadshot

    public void Deadshot()
    {  
        reticle.Activate();        
    }//End Deadshot    

    public bool DeadshotSkillCheckPassed()
    {
        if(reticle)
        {
            return reticle.IsSuccessful();
        }//End if

        return false;
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
        if (currentDeadshotTokens > 0)
        {
            currentDeadshotTokens--;
        }//End if
    }//End RemoveToken
}