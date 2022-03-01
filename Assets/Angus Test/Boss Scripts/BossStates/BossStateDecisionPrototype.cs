using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStateDecisionPrototype : BossState
{
    private Boss.State prevState;
    [SerializeField] private List<Boss.State> desiredStates;

    public void Start()
    {
        base.Start();
    }

    public override void OnEnter()
    {
        base.OnEnter();
        prevState = boss.state;

        boss.ChangeState(ChooseAttack());
    }

    public override void OnExit()
    {
        base.OnExit();              
    }

    Boss.State ChooseAttack()
    {
        int rand = Random.Range(0, 3);

        switch (rand)
        {
            case 0:
                return Boss.State.Charging;
            case 1:
                return Boss.State.Uproot;
            case 2:
                return Boss.State.LandsRoots;
        }

        return Boss.State.Idle;
    }
}
