using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStateBurrow : AttackState
{
    [SerializeField] float goundedYPosition;
    [SerializeField] float burrowYPosition;
    [SerializeField] float maxRotationSpeed;
    [SerializeField] float burrowSpeed;

    public void Start()
    {
        base.Start();
    }

    public override void OnEnter()
    {
        base.OnEnter();
    }

    public override void OnExit()
    {
        base.OnExit(); 
    }

    public override void FixedRun()
    {
        base.FixedRun();
    }

    IEnumerator StartBurrow()
    {
       yield return null;
    }
}
