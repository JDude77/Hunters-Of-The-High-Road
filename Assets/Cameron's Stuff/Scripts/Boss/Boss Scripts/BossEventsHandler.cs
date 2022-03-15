using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class BossEventsHandler : MonoBehaviour
{
    public static BossEventsHandler current;

    private void Awake()
    {
        current = this;        
    }

    #region Boss Stunned
    public event Action OnBossStunned;
    public void BossStunned() { OnBossStunned?.Invoke(); }
    #endregion

    public event Action<float> OnHitPlayer;
    public void HitPlayer(float damageValue) { OnHitPlayer?.Invoke(damageValue); }
}
