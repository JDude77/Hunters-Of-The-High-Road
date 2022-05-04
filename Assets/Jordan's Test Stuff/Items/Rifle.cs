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
    private Material lineRendererMaterial; //Need to alter the alpha directly on the material

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
        lineRendererMaterial = shotPathLineRenderer.material; //Getting the material from the shot renderer to directly change the alpha
        shotParticleSystem = GetComponentInChildren<ParticleSystem>();
        gunAnimator = GetComponentInChildren<Animator>();

        if (deadshot) SetDeadshotValues();
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

    public override void Use()
    {
        Vector3 shotHitLocation = GetShotLocation();

        Collider[] shotHits = Physics.OverlapSphere(shotHitLocation, shotRadius);

        //Prevents the program from stopping if line renderer isn't set
        if(shotPathLineRenderer)
        {
            if (fadeFunctionCopy != null) StopCoroutine(fadeFunctionCopy);
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

        bool hitEnemy = false;
        GameObject enemyInstance = null;

        foreach (Collider hit in shotHits)
        {
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
                    case "Boss":
                        PlayerEventsHandler.current.HitEnemy(hit.gameObject, damage);
                        //Below pieces for deadshot
                        hitEnemy = true;
                        enemyInstance = hit.gameObject;
                        break;
                }//End switch
            }//End if
        }//End foreach
        Deadshot(hitEnemy, enemyInstance);

        isReloaded = false;
        StartCoroutine(ReloadGun());
    }//End Use

    private void Deadshot(bool hitWasEnemy, GameObject enemyInstance)
    {
        //Deadshot functionality only works on enemies
        if (deadshot)
        {
            bool tokenisable = isBeingAimed && deadshot.DeadshotSkillCheckPassed();
            //If the hit is actually a deadshot hit
            if(tokenisable)
            {
                //If the deadshot hit an enemy
                if (hitWasEnemy)
                {
                    if (deadshot.CanStagger())
                    {
                        PlayerEventsHandler.current.StaggerEnemy(enemyInstance);
                        deadshot.ResetTokens();
                    }//End if
                    else if (!deadshot.CanStagger())
                    {
                        deadshot.AddToken();
                    }//End else if
                }//End if
                //If the deadshot didn't hit an enemy
                else
                {
                    deadshot.RemoveToken();
                }//End else
            }//End if
        }//End if
    }//End Deadshot

    //Could be updated to take in start and end line colour
    //Made a coroutine to delay line appearance by a frame, making it align with muzzle better
    public IEnumerator UpdateShotLineRenderer(Vector3 shotHitLocation)
    {
        yield return new WaitForEndOfFrame();
        shotPathLineRenderer.enabled = true;
        if(gunAnimator.GetBool("CanDeadshot"))
        {
            shotPathLineRenderer.colorGradient = shotLineDeadshotColour;
        }//End if
        else
        {
            shotPathLineRenderer.colorGradient = shotLineDefaultColour;
        }//End else
        shotPathLineRenderer.SetPosition(0, muzzle.position);
        shotPathLineRenderer.SetPosition(1, shotHitLocation);
    }//End UpdateShotLineRenderer

    private Vector3 GetShotLocation()
    {
        //Currently just Cameron's code copied and pasted in with variables renamed
        Vector3 shotLocation = Vector3.zero;

        Ray hitRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(hitRay, out RaycastHit hitLocation);

        if (hitLocation.transform != null)
        {
            shotLocation = hitLocation.point;
        }//End if

        Vector3 playerLookAtLocation = shotLocation - playerReference.transform.position;
        playerLookAtLocation.y = 0;
        playerTransform.rotation = Quaternion.LookRotation(playerLookAtLocation);

        Ray shotRay = new Ray(muzzle.position, shotLocation - muzzle.position);

        if (Physics.Raycast(shotRay, out hitLocation, maxRange))
        {
            shotLocation = hitLocation.point;
            shotLocation.y = shotHeight;
        }//End if

        return shotLocation;
    }//End GetShotLocation

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
        lineRendererMaterial.SetFloat("_Alpha", 1); //sets the alpha to 1 after firing

        yield return new WaitForSeconds(shotLineFadeDelay);

        Color oldStartColour = shotPathLineRenderer.startColor;
        Color oldEndColour = shotPathLineRenderer.endColor;
        float oldAlpha = oldStartColour.a; //Getting the alpha from the startcolour - if we add a alpha gradient, this will only affect the first colour in the gradient

        Color newStartColour = new Color(shotPathLineRenderer.startColor.r, shotPathLineRenderer.startColor.g, shotPathLineRenderer.endColor.b, 0);
        Color newEndColour = new Color(shotPathLineRenderer.endColor.r, shotPathLineRenderer.endColor.g, shotPathLineRenderer.endColor.b, 0);
        float newAlpha = newStartColour.a;

        float secondsElapsed = 0.0f;

        while(secondsElapsed < shotLineFadeTime)
        {
            shotPathLineRenderer.startColor = Color.Lerp(oldStartColour, newStartColour, secondsElapsed / shotLineFadeTime);
            shotPathLineRenderer.endColor = Color.Lerp(oldEndColour, newEndColour, secondsElapsed / shotLineFadeTime);
            lineRendererMaterial.SetFloat("_Alpha", Mathf.Lerp(oldAlpha, newAlpha, secondsElapsed / shotLineFadeDelay)); //Because shader graph is amazing, we need to set a custom float to the desired alpha value

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

    public void SetDeadshotValues()
    {
        //deadshot.SetShotLineRenderer(shotPathLineRenderer);
        //deadshot.SetDefaultLineColour(shotLineDefaultColour);
        //deadshot.SetDeadshotLineColour(shotLineDeadshotColour);
    }//End SetDeadshotValues
}