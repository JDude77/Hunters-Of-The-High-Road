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
    //public void BossStunned() { actionToString["OnBossStunned"]?.Invoke(); }
    public event Action OnBossStunnedEnd;
    public void BossStunnedEnd() { OnBossStunnedEnd?.Invoke(); }
    #endregion

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
    public void ChargeSwipe() { OnChargeSwipe?.Invoke(); }
    #endregion

    public event Action<float> OnHitPlayer;
    public void HitPlayer(float damageValue) { OnHitPlayer?.Invoke(damageValue); }
}
