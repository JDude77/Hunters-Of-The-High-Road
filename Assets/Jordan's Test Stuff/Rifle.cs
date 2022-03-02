using System.Collections;
using UnityEngine;

public class Rifle : Weapon
{
    [Header("Rifle Settings")]
    [SerializeField]
    private Transform muzzle;

    [SerializeField]
    private float aimedShotDamage;

    [SerializeField]
    private float maxRange = 100.0f;

    [SerializeField]
    [Tooltip("The height at which all shots should fire.")]
    private float shotHeight;

    [SerializeField]
    [Tooltip("The radius of impact for an individual shot.")]
    private float shotRadius;

    [SerializeField]
    [Tooltip("The time taken to reload the gun in seconds.")]
    private float reloadTime;

    private bool isReloaded = true;

    protected override void Awake()
    {
        base.Awake();

        isEquipped = true;
        weaponType = WeaponType.Ranged;
    }//End Awake

    protected void Start()
    {
        playerState = (PlayerState)GetComponentInParent(PlayerState.stateDictionary[Player.State.RifleAimedShot]);
    }//End Start

    public override void Use()
    {
        Collider[] shotHits = Physics.OverlapSphere(GetShotLocation(), shotRadius);

        foreach (Collider hit in shotHits)
        {
            switch (hit.tag)
            {
                case "Chain":
                    hit.GetComponentInParent<ChainDoorScript>().openDoor();
                    hit.gameObject.SetActive(false);
                    break;

                case "Tombstone":
                    hit.GetComponentInParent<DestructibleTombstone>().destroyTombstone();
                    break;

                case "Bottle":
                    hit.GetComponentInParent<TutorialBottle>().shootBottle();
                    break;
            }//End switch
        }//End foreach

        isReloaded = false;
        StartCoroutine(ReloadGun());
    }//End Use

    private Vector3 GetShotLocation()
    {
        //Currently just Cameron's code copied and pasted in with variables renamed
        Vector3 shotLocation = Vector3.zero;

        RaycastHit hitLocation;
        Ray hitRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(hitRay, out hitLocation);

        if (hitLocation.transform != null)
        {
            shotLocation = hitLocation.point;
        }//End if

        Vector3 playerLookAtLocation = shotLocation - playerReference.transform.position;
        playerLookAtLocation.y = 0;
        playerReference.transform.rotation = Quaternion.LookRotation(playerLookAtLocation);

        Ray shotRay = new Ray(muzzle.position, shotLocation - muzzle.position);

        if (Physics.Raycast(shotRay, out hitLocation, maxRange))
        {
            shotLocation = hitLocation.point;
            shotLocation.y = shotHeight;
        }//End if

        return shotLocation;
    }//End GetShotLocation

    private IEnumerator ReloadGun()
    {
        yield return new WaitForSeconds(reloadTime);
        isReloaded = true;
    }//End ReloadGun

    public bool GetIsReloaded()
    {
        return isReloaded;
    }//End GetIsReloaded
}