using UnityEngine;
using UnityEngine.UI;

public class BossHUDManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField]
    private Image healthBar;

    [SerializeField]
    private GameObject bossHUD;

    private BossTrigger bossTrigger;
    private Boss boss;

    private void Start()
    {
        bossTrigger = FindObjectOfType<BossTrigger>();
        bossTrigger.TriggerActivated += ActivateBossHUD;

        boss = FindObjectOfType<Boss>();
    }//End Start

    private void Update()
    {
        healthBar.fillAmount = boss.GetNormalizedHealth();
    }//End Update

    private void ActivateBossHUD()
    {
        bossHUD.SetActive(true);
    }//End ActivateBossHUD
}