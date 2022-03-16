using UnityEngine;

public class ExecutionerSword : Weapon
{
    [Header("Executioner Sword Settings", order = 0)]
    [Header("Camera Shake", order = 1)]
    [SerializeField]
    private float shakeIntensity;
    [SerializeField]
    private float shakeTime;

    [Header("Attack Settings")]
    [SerializeField]
    private Transform attackCentrePoint;
    [SerializeField]
    private float attackRadius;

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

    public override void Use()
    {
        base.Use();

        if(CameraShakeScript.Instance) CameraShakeScript.Instance.ShakeCamera(shakeIntensity, shakeTime);

        Collider[] objectsHitBySword = Physics.OverlapSphere(attackCentrePoint.position, attackRadius);

        foreach(Collider objectHit in objectsHitBySword)
        {
            switch(objectHit.tag)
            {
                case "Chain":
                    PlayerEventsHandler.current.HitChain(objectHit.gameObject);
                    break;
                case "Tombstone":
                    PlayerEventsHandler.current.HitGravestone(objectHit.gameObject);
                    break;
                case "Bottle":
                    PlayerEventsHandler.current.HitBottle(objectHit.gameObject);
                    break;
                case "Enemy":
                case "Boss":
                    PlayerEventsHandler.current.HitEnemy(objectHit.gameObject, damage);
                    break;
            }//End switch
        }//End foreach
    }//End Use
}