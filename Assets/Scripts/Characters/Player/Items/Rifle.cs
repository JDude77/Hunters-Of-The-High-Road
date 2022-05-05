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
    public float GetShotHeight() { return shotHeight; }//End GetShotHeight

    [SerializeField]
    [Tooltip("The radius of impact for an individual shot.")]
    private float shotRadius;

    [SerializeField]
    [Tooltip("The time taken to reload the gun in seconds.")]
    private float reloadTime;

    [SerializeField]
    private DeadshotManager deadshot;
    public DeadshotManager GetDeadshotManager() { return deadshot; }//End GetDeadshotManager

    private bool isReloaded = true;
    private bool isBeingAimed = false;
    public void SetIsBeingAimed(bool isBeingAimed) { this.isBeingAimed = isBeingAimed; }//End SetIsBeingAimed

    private Animator gunAnimator;
    public Animator GetGunAnimator() { return gunAnimator; }//End GetGunAnimator

    #region FX/Polish Variables
    private LineRenderer shotPathLineRenderer;
    //Need to alter the alpha directly on the material
    private Material lineRendererMaterial;

    public LineRenderer GetShotLineRenderer() { return shotPathLineRenderer; }//End ShotPathLineRenderer
    [Header("FX Settings")]
    [SerializeField]
    private Gradient shotLineDefaultColour;
    [SerializeField]
    private Gradient shotLineDeadshotColour;
    [SerializeField]
    [Tooltip("The time taken for the line representing where the gun has shot alpha to fade out in seconds.")]
    private float shotLineFadeTime;
    [SerializeField]
    [Tooltip("The delay before the shot line starts to fade out in seconds.")]
    private float shotLineFadeDelay;
    private IEnumerator fadeFunctionCopy;

    private ParticleSystem shotParticleSystem;

    [SerializeField]
    private GameObject muzzleFlashObject;
    [SerializeField]
    [Tooltip("The length of time to show the muzzle flash for in seconds.")]
    private float muzzleFlashTime;
    [SerializeField]
    private float shotScreenShakeIntensity;
    [SerializeField]
    private float shotScreenShakeTime;
    [SerializeField]
    private float deadShotScreenShakeIntensity;
    [SerializeField]
    private float deadShotScreenShakeTime;
    #endregion

    protected override void Awake()
    {
        base.Awake();

        isEquipped = true;
        weaponType = WeaponType.Ranged;
    }//End Awake

    protected void Start()
    {
        playerState = (PlayerState)GetComponentInParent(PlayerState.stateDictionary[Player.State.RifleAimedShot]);
        deadshot = FindObjectOfType<DeadshotManager>();
        shotPathLineRenderer = GetComponent<LineRenderer>();
        //Getting the material from the shot renderer to directly change the alpha
        lineRendererMaterial = shotPathLineRenderer.material;
        shotParticleSystem = GetComponentInChildren<ParticleSystem>();
        gunAnimator = GetComponentInChildren<Animator>();
    }//End Start

    //Not ideal to use this here, but it'll fix an issue with aiming after shooting until I can refactor it elsewhere
    private void Update()
    {
        if (isBeingAimed && fadeFunctionCopy != null)
        {
            StopCoroutine(fadeFunctionCopy);
            fadeFunctionCopy = null;
        }//End if

        if (deadshot.DeadshotSkillCheckPassed())
        {
            gunAnimator.SetBool("CanDeadshot", true);
        }//End if
        else
        {
            gunAnimator.SetBool("CanDeadshot", false);
        }//End else
    }//End Update

    public void DeactivateDeadshot()
    {
        shotPathLineRenderer.colorGradient = shotLineDefaultColour;
        shotPathLineRenderer.enabled = false;
        deadshot.DeactivateDeadshot();
    }//End DeactivateDeadshot

    public void Deadshot() 
    {
        lineRendererMaterial.SetFloat("_Alpha", 1);
        shotPathLineRenderer.enabled = true;
        deadshot.Deadshot();
    }//End Deadshot

    private Vector3 GetShotLocation()
    {
        Vector3 start = muzzle.position;
        start.y = shotHeight;

        //Get the clicked position on the screen
        Vector2 mouseClickPosition = Input.mousePosition;

        //Create a ray from the camera to the clicked point
        Ray cameraRay = Camera.main.ScreenPointToRay(mouseClickPosition);

        //Get world location of clicked point from ray hit position
        Physics.Raycast(cameraRay, out RaycastHit cameraRayHitInfo);
        Vector3 mouseClickWorldPosition = cameraRayHitInfo.point;

        //Fix the player's rotation to face the shot location
        FixPlayerRotation(mouseClickWorldPosition);

        //Fix world location of clicked point to be desired y-position
        mouseClickWorldPosition.y = shotHeight;

        //Get the direction from the start point to the mouse click
        Vector3 shotDirection = (mouseClickWorldPosition - start).normalized;

        //Create ray going in direction from gun to mouse click location
        Ray gunToMousePointRay = new Ray(start, shotDirection);

        //Get first hit location of gun to mouse ray
        if(Physics.Raycast(gunToMousePointRay, out RaycastHit shotHitInfo, maxRange))
        {
            return shotHitInfo.point;
        }//End if

        //Return the shot direction * max range if nothing is hit
        return shotDirection * maxRange;
    }//End GetShotLocationNew

    public override void Use()
    {
        //Gets the location hit by the gun's shot
        Vector3 shotHitLocation = GetShotLocation();

        //Gets everything hit within a certain radius of that hit location
        Collider[] shotHits = Physics.OverlapSphere(shotHitLocation, shotRadius);

        //Prevents the program from stopping if line renderer isn't set
        if(shotPathLineRenderer)
        {
            //Updates the line renderer and stops old behaviours if running
            if (fadeFunctionCopy != null)
            {
                StopCoroutine(fadeFunctionCopy);
            }//End if
            StartCoroutine(UpdateShotLineRenderer(shotHitLocation));
            fadeFunctionCopy = FadeShotLine();
            StartCoroutine(fadeFunctionCopy);
        }//End if

        //As above, but for the particle system 
        if(shotParticleSystem)
        {
            shotParticleSystem.Play();
        }//End if

        //Again, for the muzzle flash
        if(muzzleFlashObject)
        {
            StartCoroutine(MuzzleFlash());
        }//End if

        //Set-up for handling deadshot particulars of the rifle
        bool hitEnemy = false;
        GameObject enemyInstance = null;

        //Looping through everything hit by the shot
        foreach (Collider hit in shotHits)
        {
            //Extra checking to make sure the gameObject isn't somehow null to prevent errors
            if (hit.gameObject != null)
            {
                switch (hit.tag)
                {
                    case "Chain":
                        PlayerEventsHandler.current.HitChain(hit.gameObject);
                        break;
                    case "Tombstone":
                        PlayerEventsHandler.current.HitGravestone(hit.gameObject);
                        break;
                    case "Bottle":
                        PlayerEventsHandler.current.HitBottle(hit.gameObject);
                        break;
                    case "Enemy":
                        break;
                    case "Boss":
                        float damageToDo = isBeingAimed && deadshot.DeadshotSkillCheckPassed() ? aimedShotDamage : damage;
                        PlayerEventsHandler.current.HitEnemy(hit.gameObject, damageToDo);
                        //Below pieces for deadshot
                        hitEnemy = true;
                        enemyInstance = hit.gameObject;
                        //Camera shake if hit is landed on the boss
                        if (CameraShakeScript.Instance)
                        {
                            CameraShakeScript.Instance.ShakeCamera(shotScreenShakeIntensity, shotScreenShakeTime);
                        }//End if
                        break;
                }//End switch
            }//End if
        }//End foreach

        //Does deadshot functionality if an enemy was hit
        Deadshot(hitEnemy, enemyInstance);

        //Starts delay before next shot
        isReloaded = false;
        StartCoroutine(ReloadGun());
    }//End Use

    private void Deadshot(bool hitWasEnemy, GameObject enemyInstance)
    {
        //Deadshot functionality only works on enemies
        //Checks to make sure that deadshot is present to prevent errors
        if (deadshot)
        {
            //Checks the criteria for token addition or subtraction
            bool tokenisable = isBeingAimed && deadshot.DeadshotSkillCheckPassed();
            //If the hit is actually a deadshot hit
            if(tokenisable)
            {
                //If the deadshot hit an enemy
                if (hitWasEnemy)
                {
                    //Applies screenshake if object allowing it is present
                    if (CameraShakeScript.Instance)
                    {
                        CameraShakeScript.Instance.ShakeCamera(deadShotScreenShakeIntensity, deadShotScreenShakeTime);
                    }//End if

                    //Staggers the enemy if token count allows for it
                    if (deadshot.CanStagger())
                    {
                        PlayerEventsHandler.current.StaggerEnemy(enemyInstance);
                        deadshot.ResetTokens();
                    }//End if
                    //Adds a token if token count is not at staggerable level
                    else if (!deadshot.CanStagger())
                    {
                        deadshot.AddToken();
                    }//End else if
                }//End if
                //If the deadshot didn't hit an enemy, removes token
                else
                {
                    deadshot.RemoveToken();
                }//End else
            }//End if
        }//End if
    }//End Deadshot

    //Could be updated to take in start and end line colour
    public IEnumerator UpdateShotLineRenderer(Vector3 shotHitLocation)
    {
        //Disable line renderer while updating
        shotPathLineRenderer.enabled = false;

        //Delay line appearance by a frame, making it align with muzzle better
        yield return new WaitForEndOfFrame();
        //Change colour in accordance with whether deadshot can happen
        if(gunAnimator.GetBool("CanDeadshot"))
        {
            shotPathLineRenderer.colorGradient = shotLineDeadshotColour;
        }//End if
        else
        {
            shotPathLineRenderer.colorGradient = shotLineDefaultColour;
        }//End else
        //Update line renderer position to match gun
        shotPathLineRenderer.SetPosition(0, muzzle.position);
        shotPathLineRenderer.SetPosition(1, shotHitLocation);
        //Re-enable line renderer
        shotPathLineRenderer.enabled = true;
    }//End UpdateShotLineRenderer

    //Updates player rotation based on direction of shot
    private static void FixPlayerRotation(Vector3 shotLocation)
    {
        Vector3 playerLookAtLocation = shotLocation - playerReference.transform.position;
        playerLookAtLocation.y = 0;
        playerTransform.rotation = Quaternion.LookRotation(playerLookAtLocation);
    }//End FixPlayerRotation

    public void UpdateLinePosition()
    {
        Vector3 shotHitLocation = GetShotLocation();
        shotPathLineRenderer.SetPosition(0, muzzle.position);
        shotPathLineRenderer.SetPosition(1, shotHitLocation);
    }//End UpdateLinePosition

    private IEnumerator ReloadGun()
    {
        yield return new WaitForSeconds(reloadTime);
        isReloaded = true;
    }//End ReloadGun

    private IEnumerator FadeShotLine()
    {
        shotPathLineRenderer.colorGradient =  shotLineDefaultColour;
        //Sets the alpha to 1 immediately after firing
        lineRendererMaterial.SetFloat("_Alpha", 1); 

        yield return new WaitForSeconds(shotLineFadeDelay);

        Color oldStartColour = shotPathLineRenderer.startColor;
        Color oldEndColour = shotPathLineRenderer.endColor;
        //Getting the alpha from the oldStartColour - if we add an alpha gradient, this will only affect the first colour in the gradient
        float oldAlpha = oldStartColour.a; 

        Color newStartColour = new Color(shotPathLineRenderer.startColor.r, shotPathLineRenderer.startColor.g, shotPathLineRenderer.endColor.b, 0);
        Color newEndColour = new Color(shotPathLineRenderer.endColor.r, shotPathLineRenderer.endColor.g, shotPathLineRenderer.endColor.b, 0);
        float newAlpha = newStartColour.a;

        float secondsElapsed = 0.0f;

        while(secondsElapsed < shotLineFadeTime)
        {
            shotPathLineRenderer.startColor = Color.Lerp(oldStartColour, newStartColour, secondsElapsed / shotLineFadeTime);
            shotPathLineRenderer.endColor = Color.Lerp(oldEndColour, newEndColour, secondsElapsed / shotLineFadeTime);
            //Because shader graph is amazing, we need to set a custom float to the desired alpha value
            lineRendererMaterial.SetFloat("_Alpha", Mathf.Lerp(oldAlpha, newAlpha, secondsElapsed / shotLineFadeDelay)); 

            secondsElapsed += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }//End while

        shotPathLineRenderer.startColor = newStartColour;
        shotPathLineRenderer.endColor = newEndColour;
        lineRendererMaterial.SetFloat("_Alpha", newStartColour.a);

        shotPathLineRenderer.enabled = false;
    }//End FadeShotLine

    private IEnumerator MuzzleFlash()
    {
        muzzleFlashObject.SetActive(true);
        yield return new WaitForSeconds(muzzleFlashTime);
        muzzleFlashObject.SetActive(false);
    }//End MuzzleFlash

    public bool GetIsReloaded()
    {
        return isReloaded;
    }//End GetIsReloaded
}