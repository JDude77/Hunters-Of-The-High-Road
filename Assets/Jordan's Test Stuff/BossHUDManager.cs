using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class BossHUDManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField]
    private Image healthBar;

    [SerializeField]
    private GameObject bossHUD;

    private BossTrigger bossTrigger;
    private Boss boss;

    // Start is called before the first frame update
    void Start()
    {
        bossTrigger = FindObjectOfType<BossTrigger>();
        bossTrigger.TriggerActivated += ActivateBossHUD;

        boss = FindObjectOfType<Boss>();
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.fillAmount = boss.GetNormalizedHealth();
    }

    private void ActivateBossHUD()
    {
        bossHUD.SetActive(true);
    }
}
