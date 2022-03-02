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
            if(objectHit.CompareTag("Chain"))
            {
                objectHit.GetComponentInParent<ChainDoorScript>().openDoor();
                objectHit.gameObject.SetActive(false);
            }//End if
        }//End foreach
    }//End Use
}