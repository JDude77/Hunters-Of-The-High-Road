using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStateStunned : BossState
{
    [Header("Sounds")]
    [SerializeField] private AK.Wwise.Event stunned;
    [Header("Animation Names")]
    [SerializeField] private string stunnedAnimation;

    private void Start()
    {
        base.Start();
        eventResponder.AddSoundEffect("StateEnter", stunned, gameObject);
        eventResponder.AddAnimation("StateEnter", stunnedAnimation, false);
        //Animation event
        eventResponder.AddAction("AnimationEnd", boss.ReturnToMainState);
    }

    public override void OnEnter()
    {
        base.OnEnter();
        eventResponder.ActivateAll("StateEnter");
    }//End OnEnter

    public override void OnExit()
    {
        base.OnExit();
    }//End OnExit

    [ContextMenu("Fill Default Values")]
    public override void SetDefaultValues()
    {
        stunnedAnimation = "Boss_Stunned";
    }//End SetDefaultValues
}
