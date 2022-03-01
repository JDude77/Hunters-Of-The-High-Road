using UnityEngine;

public class ExecutionerSword : Weapon
{
    protected override void Awake()
    {
        base.Awake();

        isEquipped = true;
        weaponType = WeaponType.Melee;
    }//End Awake

    protected void Start()
    {
        playerState = (PlayerState) GetComponentInParent(PlayerState.stateDictionary[Player.State.ExecutionerSwordAttack]);
    }//End Start
}