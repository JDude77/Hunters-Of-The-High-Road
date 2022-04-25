using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BossStateIdle : BossState
{
    // Start is called before the first frame update

    float retryTime = 0.5f;

    public override void OnEnter()
    {
        base.OnEnter();
        StartCoroutine(exitState());
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
    }

    IEnumerator exitState() {
        yield return new WaitForSeconds(retryTime);
        boss.ReturnToMainState();
    }
}
