using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class BossEventsHandler : MonoBehaviour
{
    public static BossEventsHandler current;

    private void Awake()
    {
        if (current == null) current = this;
        else Destroy(this);
    }

    #region Boss Stunned
    public event Action OnBossStunned;
    public void BossStunned() { OnBossStunned?.Invoke(); }
    #endregion

    public event Action<GameObject, float> OnHitPlayer;
    public void HitPlayer(float damageValue) { OnHitPlayer?.Invoke(gameObject, damageValue); }
}
