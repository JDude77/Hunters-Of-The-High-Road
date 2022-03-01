using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BossStateIdle : BossState
{
    // Start is called before the first frame update

    public override void OnEnter()
    {
        base.OnEnter();
        Debug.Log("Idle State Enter");
        boss.ChangeState(Boss.State.DecisionPrototype);
    }

    public override void Run()
    {

    }
    public override void FixedRun()
    {

    }

    public override void OnExit()
    {
        base.OnExit();
        Debug.Log("Idle State Exit");
    }
}
