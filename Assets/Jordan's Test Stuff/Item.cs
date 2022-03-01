using UnityEngine;
using System.Collections.Generic;

public class Item : MonoBehaviour
{
    protected bool isEquipped = false;

    protected PlayerState playerState;

    protected static Player playerReference;

    protected delegate void ItemActionDelegate();

    protected List<ItemActionDelegate> actionDelegates;

    protected virtual void Awake()
    {
        actionDelegates = new List<ItemActionDelegate>();
        playerReference = FindObjectOfType<Player>();
    }//End Awake

    public virtual void Use()
    {

    }//End Use
}