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

    public event Action OnBossStunned;
    public void BossStunned() { OnBossStunned?.Invoke(); }

    #region Charge Attack Events
    public event Action OnChargeStart;
    public void ChargeStart() { OnChargeStart?.Invoke(); }
    public event Action OnGetInRange;
    public void GetInRange() { OnGetInRange?.Invoke(); }
    public event Action OnChargeWindUp;
    public void ChargeWindUp() { OnChargeWindUp?.Invoke(); }
    public event Action OnChargeWindDown;
    public void ChargeWindDown() { OnChargeWindDown?.Invoke(); }
    public event Action OnChargeSwipe;
    public void ChargeSwipe() { OnChargeWindDown?.Invoke(); }
    #endregion

    public event Action<int> OnHitPlayer;
    public void HitPlayer(int damageValue) { OnHitPlayer?.Invoke(damageValue); }
}
