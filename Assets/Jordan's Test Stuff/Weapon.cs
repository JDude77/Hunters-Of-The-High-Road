using UnityEngine;

public class Weapon : Item
{
    [Header("Weapon Settings")]
    [SerializeField]
    protected float damage;

    protected enum WeaponType
    {
        Ranged,
        Melee,
        UNDEFINED
    }//End WeaponType enum

    [SerializeField]
    protected WeaponType weaponType = WeaponType.UNDEFINED;
}