using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BossEvent
{    
    public enum MiscEvents
    {
        OnBossStunned
    }

    public enum ChargeEvents
    {
        GettingInRange,
        ChargeStart,
        ChargeEnd,
        WindUp,
        WindDown,
        Swipe
    }

    public enum UprootEvents
    {
        WindUp,
        Attack,
        WindDown
    }

    public enum CircleSwipe
    {
        Attack
    }
}
