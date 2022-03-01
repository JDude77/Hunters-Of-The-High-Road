using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    [SerializeField]
    private float MaxHealth, MaxStamina, MaxFaith;

    [SerializeField]
    private float StaminaRefillBoost, FaithRefillBoost;

    [SerializeField]
    private Image staminaBar, faithBar;

    [SerializeField]
    private float rollingCost;

    private float currentHealth, currentStamina, currentFaith;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = MaxHealth;
        currentFaith = 0;
    }

    // Update is called once per frame
    void Update()
    {
        staminaBar.fillAmount = getImageValue(MaxStamina, currentStamina);
        staminaTimer();

        faithBar.fillAmount = getImageValue(MaxFaith, currentFaith);
        faithTimer();
    }

    private float getImageValue(float maxValue, float currentValue)
    {
        float returnVal =  currentValue / maxValue;
        return returnVal;
    }

    private void staminaTimer()
    {
        if (currentStamina < MaxStamina)
        {
            currentStamina += StaminaRefillBoost * Time.deltaTime;
        }
        else
        {
            currentStamina = MaxStamina;
        }
    }

    private void faithTimer()
    {
        if (currentFaith < MaxFaith)
        {
            currentFaith += FaithRefillBoost * Time.deltaTime;
        }
        else
        {
            currentFaith = MaxFaith;
        }
    }

    public void useStamina(float staminaUsage)
    {
        currentStamina -= staminaUsage;
    }

    public float getCurrentHealth()
    {
        return currentStamina;
    }

    public float getCurrentStamina()
    {
        return currentStamina;
    }

    public float getCurrentFaith()
    {
        return currentFaith;
    }

    public float getRollingCost()
    {
        return rollingCost;
    }
}
