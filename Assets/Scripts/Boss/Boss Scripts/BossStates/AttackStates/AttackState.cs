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
}
