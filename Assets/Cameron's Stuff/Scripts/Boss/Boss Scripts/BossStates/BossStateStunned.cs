using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class BossStateStunned : BossState
{
    [Header("Sounds")]
    [SerializeField] private AK.Wwise.Event stunned;
    [SerializeField] private float stunnedTime;

    private void Start()
    {
        base.Start();
    }

    public override void OnEnter()
    {
        base.OnEnter();
        stunned.Post(gameObject);
        boss.animator.SetTrigger("DoStun");
        StartCoroutine(timer());
    }//End OnEnter

    public override void OnExit()
    {
        base.OnExit();
    }//End OnExit

    IEnumerator timer()
    {
        yield return new WaitForSeconds(stunnedTime);
        boss.animator.SetTrigger("DoEndStun");
        boss.ReturnToMainState();
    }

    [ContextMenu("Fill Default Values")]
    public override void SetDefaultValues()
    {
        stunnedTime = 2f;
    }//End SetDefaultValues
}
