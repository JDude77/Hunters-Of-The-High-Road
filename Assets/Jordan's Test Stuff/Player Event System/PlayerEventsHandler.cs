using System;
using UnityEngine;

public class PlayerEventsHandler : MonoBehaviour
{
    public static PlayerEventsHandler current;

    private void Awake()
    {
        if(current == null) current = this;
        else Destroy(this);
    }//End Awake

    public event Action<string> OnHit;
    public void Hit(string tag)
    {
        OnHit?.Invoke(tag);
    }//End Hit
}