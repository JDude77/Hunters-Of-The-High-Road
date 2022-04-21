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

    private DeadshotManager deadshotManager;
    [SerializeField]
    private int numOfDeadshotTokens = 0;
    [SerializeField]
    private Image[] deadshotTokens;

    private Player playerReference;

    private void Start()
    {
        playerReference = FindObjectOfType<Player>();
        deadshotManager = FindObjectOfType<DeadshotManager>();
    }//End Start

    private void Update()
    {
        healthBar.fillAmount = playerReference.GetNormalizedHealth();

        staminaBar.fillAmount = playerReference.GetNormalizedStamina();

        faithBar.fillAmount = playerReference.GetNormalizedFaith();

        //TODO: Optimise this so it isn't needlessly updating every frame
        numOfDeadshotTokens = deadshotManager.GetTokenCount();

        for (int i = 0; i < deadshotTokens.Length; i++)
        {
            //Enable token if count is 1 greater than the index
            deadshotTokens[i].enabled = numOfDeadshotTokens >= (i + 1);
        }//End for
    }//End Update
}