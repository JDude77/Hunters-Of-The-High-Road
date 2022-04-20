using UnityEngine;
using UnityEngine.UI;
public class PlayerHUDManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField]
    private Image healthBar;
    [SerializeField]
    private Image staminaBar;
    [SerializeField]
    private Image faithBar;

    private Player playerReference;

    private void Start()
    {
        playerReference = FindObjectOfType<Player>();
    }//End Start

    private void Update()
    {
        healthBar.fillAmount = playerReference.GetNormalizedHealth();

        staminaBar.fillAmount = playerReference.GetNormalizedStamina();

        faithBar.fillAmount = playerReference.GetNormalizedFaith();
    }//End Update
}