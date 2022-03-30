using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BossAnimationEventsHandler : MonoBehaviour
{    
    public void ActivateEvent(string eventName)
    {
        GetComponentInParent<Boss>().currentState.eventResponder.Activate(eventName);
    }
}
