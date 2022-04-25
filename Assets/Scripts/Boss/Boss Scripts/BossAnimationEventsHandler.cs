using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BossAnimationEventsHandler : MonoBehaviour
{    
    //Activates all events with the event name
    public void ActivateEvent(string eventName) { GetComponentInParent<Boss>().currentState.eventResponder.ActivateAll(eventName); }
    //Activates only sounds with the event name
    public void ActivateSound(string eventName) { GetComponentInParent<Boss>().currentState.eventResponder.ActivateSound(eventName); }
    //Activates only animations with the event name
    public void ActivateAnimation(string eventName) { GetComponentInParent<Boss>().currentState.eventResponder.ActivateAnimation(eventName); }
    //Activates only functions with the event name
    public void ActivateAction(string eventName) { GetComponentInParent<Boss>().currentState.eventResponder.ActivateAction(eventName); }
    //Activates only the instantiation of objects with the event name
    public void ActivateObject(string eventName) { GetComponentInParent<Boss>().currentState.eventResponder.ActivateInstantiate(eventName); }
}
