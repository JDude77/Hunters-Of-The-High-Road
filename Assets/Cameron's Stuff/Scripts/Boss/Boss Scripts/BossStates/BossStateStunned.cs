using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStateStunned : BossState
{
    [Header("Sounds")]
    [SerializeField] private AK.Wwise.Event stunned;
    [SerializeField] private float stunnedTime;
    [Header("Animation Names")]
    [SerializeField] private string stunnedAnimation;

    private void Start()
    {
        base.Start();
        eventResponder.AddSoundEffect("StateEnter", stunned, gameObject);
        eventResponder.AddAnimation("StateEnter", stunnedAnimation, false);
    }

    public override void OnEnter()
    {
        base.OnEnter();
        eventResponder.ActivateAll("StateEnter");
        StartCoroutine(timer());
    }//End OnEnter

    public override void OnExit()
    {
        base.OnExit();
    }//End OnExit

    IEnumerator timer()
    {
        yield return new WaitForSeconds(stunnedTime);
        boss.ReturnToMainState();
        //TODO: Call animation trigger
    }

    [ContextMenu("Fill Default Values")]
    public override void SetDefaultValues()
    {
        stunnedTime = 2f;
        stunnedAnimation = "Boss_Stunned";
    }//End SetDefaultValues
}
