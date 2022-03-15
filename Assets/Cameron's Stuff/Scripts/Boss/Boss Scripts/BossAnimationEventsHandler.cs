using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BossAnimationEventsHandler : MonoBehaviour
{
    public static BossAnimationEventsHandler current;

    private void Awake()
    {
        current = this;
    }


    //TODO

    //Circular Swipe

    //Running

    //Charge wind up

    //Charge

    //Charge wind down / swipe

    //Lands roots wind up

    //Lands roots wind down

    //Uproot wind up

    //Uproot

    //Uproot wind down

    //Burrow down finished
    public event Action OnBurrowDownFinished;
    public void BurrowDownFinished() { OnBurrowDownFinished?.Invoke(); }
    //Burrow up finished
    public event Action OnBurrowUpFinished;
    public void BurrowUpFinished() { OnBurrowUpFinished?.Invoke(); }

}
