using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BossStateIdle : BossState
{
    private float retryTime = 0.5f;

    public override void OnEnter()
    {
        base.OnEnter();
        StartCoroutine(TryExit());
    }//End OnEnter

    public override void OnExit()
    {
        base.OnExit();
    }//End OnExit

    private IEnumerator TryExit() {
        yield return new WaitForSeconds(retryTime);
        boss.ReturnToMainState();
    }//End ExitState
    
}//End BossStateIdle
