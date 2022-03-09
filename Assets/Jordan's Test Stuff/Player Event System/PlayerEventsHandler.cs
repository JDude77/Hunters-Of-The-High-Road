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

    public event Action<string, GameObject> OnHit;
    public void Hit(string tag, GameObject instance)
    {
        OnHit?.Invoke(tag, instance);
    }//End Hit

    public event Action<GameObject> OnHitBottle;
    public void HitBottle(GameObject instance)
    {
        OnHitBottle?.Invoke(instance);
    }//End HitBottle

    public event Action<GameObject> OnHitChain;
    public void HitChain(GameObject instance)
    {
        OnHitChain?.Invoke(instance);
    }//End HitChain

    public event Action<GameObject> OnHitGravestone;
    public void HitGravestone(GameObject instance)
    {
        OnHitGravestone?.Invoke(instance);
    }//End HitGravestone
}