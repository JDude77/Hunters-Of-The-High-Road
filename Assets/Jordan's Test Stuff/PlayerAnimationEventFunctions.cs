using UnityEngine;

//I hate that this needs to exist with every part of my being
public class PlayerAnimationEventFunctions : MonoBehaviour
{
    //private Player playerReference;

    private ExecutionerSword sword;
    private PlayerStateExecutionerSwordAttack swordAttack;
    private PlayerStateDodging dodging;
    private PlayerAdvancedAnimations advancedAnimations;

    private bool isAiming = false;

    private void Awake()
    {
        Player playerReference = FindObjectOfType<Player>();
        sword = playerReference.GetComponentInChildren<ExecutionerSword>();
        swordAttack = playerReference.GetComponentInChildren<PlayerStateExecutionerSwordAttack>();
        advancedAnimations = playerReference.GetComponentInChildren<PlayerAdvancedAnimations>();
        dodging = playerReference.GetComponentInChildren<PlayerStateDodging>();
    }//End Awake

    //Used to check for damage when the sword hits the ground
    public void ExecutionerSwordHit()
    {
        sword.Use();
    }//End ExecutionerSwordHit

    //Used to check for the end of the sword animation to switch the player back
    public void SwordAnimationDone()
    {
        swordAttack.SetAnimationDone();
    }//End FinishSwordAnimation

    //Used to activate and deactivate Cameron's scuffed aiming fix rig
    public void ToggleAimRig(int toggle)
    {
        isAiming = toggle >= 1;
        advancedAnimations.SetAimingRig(isAiming);
    }//End ToggleAimRig

    //Used for shaking the camera during the roll animation
    public void RollCameraShake()
    {
        if(CameraShakeScript.Instance) dodging.ShakeCamera();
    }//End RollCameraShake
}
