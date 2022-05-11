using System;
using UnityEngine;

public class PlayerEventsHandler : MonoBehaviour
{
    public static PlayerEventsHandler current;

    private void Awake()
    {
        //Enforce singleton
        if (current == null)
        {
            current = this;
        }//End if
        else
        {
            Destroy(this);
        }//End else
    }//End Awake

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

    public event Action<GameObject, float> OnHitEnemy;
    public void HitEnemy(GameObject instance, float damage)
    {
        OnHitEnemy?.Invoke(instance, damage);
    }//End HitEnemy

    public event Action<GameObject> OnStaggerEnemy;
    public void StaggerEnemy(GameObject instance)
    {
        OnStaggerEnemy?.Invoke(instance);
    }//End StaggerEnemy
}