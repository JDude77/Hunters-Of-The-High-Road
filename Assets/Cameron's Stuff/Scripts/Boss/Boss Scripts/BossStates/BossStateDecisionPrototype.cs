using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStateDecisionPrototype : BossState
{
    [SerializeField] private float decisionTime;
    [SerializeField] private List<Boss.State> desiredStates;

    public void Start()
    {
        base.Start();
    }

    public override void OnEnter()
    {
        base.OnEnter();
        StartCoroutine(Decide());
    }

    public override void OnExit()
    {
        base.OnExit();
        StopAllCoroutines();
    }

    Boss.State ChooseAttack()
    {
        int rand = Random.Range(0, desiredStates.Count);
        return desiredStates[rand];
    }

    IEnumerator Decide()
    {
        yield return new WaitForSeconds(decisionTime);
        boss.ChangeState(ChooseAttack());
    }
}
