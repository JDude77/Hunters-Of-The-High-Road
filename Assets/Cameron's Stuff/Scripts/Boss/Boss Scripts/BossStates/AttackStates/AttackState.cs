using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : BossState
{
    [Header("Damage settings")]
    [Tooltip("If false, min damage is used")]
    [SerializeField] private bool randomizeDamage;
    [SerializeField] private float maxDamage;
    [SerializeField] private float minDamage;
    [SerializeField] private bool checkForStunEvent;

    public void Awake()
    {
        if(checkForStunEvent)
            BossEventsHandler.current.OnBossStunned += StunnedResponse;
    }

    public void Start()
    {
        base.Start();
    }

    protected float GetDamageValue()
    {
        if (randomizeDamage)
            return Random.Range(minDamage, maxDamage);

        return minDamage;
    }

    private void StunnedResponse()
    {
        //Change the boss's state
        boss.ChangeState(Boss.State.Stunned);
    }
}
