using UnityEngine;
using System;
using System.Collections.Generic;

public class PlayerState : MonoBehaviour
{
    #region Static Variables
    //Dictionary of state enums and their related scripts, preventing as much runtime conversion
    public static readonly Dictionary<Player.State, Type> stateDictionary = new Dictionary<Player.State, Type>
    {
        { Player.State.Idle, typeof(PlayerStateIdle) },
        { Player.State.Running, typeof(PlayerStateRunning) },
        { Player.State.Dodging, typeof(PlayerStateDodging) },
        { Player.State.ExecutionerSwordAttack, typeof(PlayerStateExecutionerSwordAttack) },
        { Player.State.RifleAimedShot, typeof(PlayerStateRifleAimedShot) },
        { Player.State.GameStart, typeof(PlayerStateGameStart) }
    };

    //All states reference the same player
    protected static Player playerReference;

    //Reference to the player's animator
    protected static Animator playerAnimator;

    //Reference to advanced animation handler script
    protected static PlayerAdvancedAnimations playerAdvancedAnimations;
    #endregion

    protected Player.State stateID;

    protected virtual void Awake()
    {
        playerReference = FindObjectOfType<Player>();
        playerAnimator = playerReference.GetComponentInChildren<Animator>();
        playerAdvancedAnimations = playerReference.GetComponentInChildren<PlayerAdvancedAnimations>();
    }//End Awake

    public Player.State GetStateID()
    {
        return stateID;
    }//End GetStateID

    public virtual void UpdateState()
    {
        UpdateStateInputs();
    }//End UpdateState

    protected virtual void UpdateStateInputs()
    {

    }//End UpdateStateInputs

    public virtual bool ExitState()
    {
        return true;
    }//End ExitState

    public virtual bool EnterState()
    {
        return true;
    }//End EnterState
}