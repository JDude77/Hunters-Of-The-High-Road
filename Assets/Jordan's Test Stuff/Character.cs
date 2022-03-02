using UnityEngine;

public class Character : MonoBehaviour
{
    #region Health
    
    [Header("Health Options")]
    [SerializeField]
    protected float health;

    [SerializeField]
    protected float maxHealth = 100.0f;

    public void SetHealth(float health)
    {
        this.health = health;

        //Stop health going below zero
        if (this.health < 0.0f) this.health = 0.0f;

        //Stop health going above max health
        else if (this.health > maxHealth) this.health = maxHealth;
    }//End SetHealth

    //Shortcut function, equivalent to SetHealth(health - x)
    public void ReduceHealthByAmount(float health)
    {
        SetHealth(this.health - Mathf.Abs(health));
    }//End ReduceHealthByAmount

    //Shortcut function, equivalent to SetHealth(health + x)
    public void IncreaseHealthByAmount(float health)
    {
        SetStamina(this.health + Mathf.Abs(health));
    }//End IncreaseHealthByAmount

    public float GetHealth()
    {
        return health;
    }//End GetHealth

    //Shortcut function, equivalent to "SetHealth(maxHealth)"
    [ContextMenu("Restore Health Fully", false, 1)]
    public void RestoreAllHealth()
    {
        health = maxHealth;
    }//End RestoreAllHealth

    //Shortcut function, equivalent to "SetHealth(0)"
    [ContextMenu("Drain All Health", false, 3)]
    public void DrainAllHealth()
    {
        health = 0.0f;
    }//End DrainAllHealth

    //Shortcut function, gets the normalized health value
    public float GetNormalizedHealth()
    {
        return health / maxHealth;
    }//End GetNormalizedHealth
    #endregion

    #region Stamina
    [Header("Stamina Options")]
    [SerializeField]
    protected float stamina;

    [SerializeField]
    protected float maxStamina = 100.0f;

    [SerializeField]
    [Tooltip("The rate of increase is on a per-second basis, independent of framerate.")]
    protected float staminaRegenerationRate = 1.0f;

    [SerializeField]
    protected bool regenerateStamina = true;

    public void SetStamina(float stamina)
    {
        this.stamina = stamina;

        //Stop stamina from going below zero
        if (this.stamina < 0.0f) stamina = 0.0f;

        //Stop stamina from going above max stamina
        else if (this.stamina > maxStamina) this.stamina = maxStamina;
    }//End SetStamina

    //Shortcut function, equivalent to SetStamina(stamina - x)
    public void ReduceStaminaByAmount(float stamina)
    {
        SetStamina(this.stamina - Mathf.Abs(stamina));
    }//End ReduceStaminaByAmount

    //Shortcut function, equivalent to SetStamina(stamina + x)
    public void IncreaseStaminaByAmount(float stamina)
    {
        SetStamina(this.stamina + Mathf.Abs(stamina));
    }//End IncreaseStaminaByAmount

    public float GetStamina()
    {
        return stamina;
    }//End GetStamina

    //Shortcut function, equivalent to "SetStamina(maxStamina)"
    [ContextMenu("Restore Stamina Fully", false, 2)]
    public void RestoreAllStamina()
    {
        stamina = maxStamina;
    }//End RestoreAllStamina

    //Shortcut function, equivalent to "SetStamina(0)"
    [ContextMenu("Drain All Stamina", false, 4)]
    public void DrainAllStamina()
    {
        stamina = 0.0f;
    }//End DrainAllStamina

    //Shortcut function, gets the normalized stamina value
    public float GetNormalizedStamina()
    {
        return stamina / maxStamina;
    }//End GetNormalizedStamina

    //Function for regenerating stamina over time
    protected virtual void RegenerateStamina()
    {
        if(regenerateStamina)
        {
            if(stamina < maxStamina)
            {
                stamina = Mathf.Clamp(stamina, stamina + (staminaRegenerationRate * Time.deltaTime), maxStamina);
            }//End if
        }//End if
    }//End RegenerateStamina
    #endregion

    protected virtual void Start()
    {
        //Set current health to max health on game start
        health = maxHealth;

        //Set current stamina to max stamina on game start
        stamina = maxStamina;
    }//End Start

    protected virtual void Update()
    {
        //If set to regenerate stamina, make sure that stamina is in fact being regenerated
        RegenerateStamina();
    }//End Update
}