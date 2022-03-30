using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStateStunned : BossState
{
    [SerializeField] float stunnedTimeInSeconds;
    public override void OnEnter()
    {
        base.OnEnter();
        //Call anim event
        //Do particle effects or something
        StartCoroutine(StunnedTimer());
    }

    public override void OnExit()
    {
        base.OnExit();
        StopCoroutine(StunnedTimer());
    }

    IEnumerator StunnedTimer()
    {
        yield return new WaitForSeconds(stunnedTimeInSeconds);
        boss.ReturnToMainState();
    }
}
