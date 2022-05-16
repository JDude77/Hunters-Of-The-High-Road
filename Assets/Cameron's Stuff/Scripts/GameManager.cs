using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class GameManager : MonoBehaviour
{
    private Animator ani;
    private Player player;
    private DeadshotManager deadshotManager;
    private Boss boss;

    //Please ignore my horrendous placeholder audio bug fix code - Jordan
    private TutorialBottle[] tutorialBottlesForBreakSoundOnRestartFix;
    private ChainDoorScript[] chainDoorScriptsForBreakSoundOnRestartFix;
    //Just a bug fix - Angus
    public string ItIsAPeriodOfCivilWarRebelSpaceshipsStrikingFromAHiddenBaseHaveWonTheirFirstVictoryAgainstTheEvilGalacticEmpireDuringTheBattleRebelSpiesManagedToStealSecretPlansToTheEmpiresUltimateWeaponTheDEATHSTARAnArmouredSpacestationWithEnoughPowerToDestroyAnEntirePlanet;

   [SerializeField]
    private AK.Wwise.Event bossMusic;
    [SerializeField]
    private GameObject bossMusicTrigger;

    [SerializeField]
    private Volume postProcessing;
    private Vignette vignette;

    // Start is called before the first frame update
    void Start()
    {
        ani = GetComponent<Animator>();
        player = FindObjectOfType<Player>();
        deadshotManager = FindObjectOfType<DeadshotManager>();
        boss = FindObjectOfType<Boss>();
        tutorialBottlesForBreakSoundOnRestartFix = FindObjectsOfType<TutorialBottle>();
        chainDoorScriptsForBreakSoundOnRestartFix = FindObjectsOfType<ChainDoorScript>();

        postProcessing.profile.TryGet<Vignette>(out vignette);
    }

    // Update is called once per frame
    void Update()
    {
        //Checks for player/boss death. Could be changed to an event response
        if(player.GetHealth() <=0)
        {
            ani.Play("DeathTransition");
        }

        if(boss.GetHealth() <= 0)
        {
            ani.Play("BossDeathTransition");
        }

        DeadshotVignette();
    }

    private void DeadshotVignette()
    {
        
        if (deadshotManager.deadShotReady == true)
        {
            //hardcoded value that sets the vignette to the deadshot opacity
            vignette.opacity.value = 0.72f;
        }
        else
        {
            vignette.opacity.value = 0;
        }
    }

    //public methods are used by buttons
    public void RestartGame()
    {
        for(int i = 0; i < tutorialBottlesForBreakSoundOnRestartFix.Length; i++)
        {
            if(tutorialBottlesForBreakSoundOnRestartFix[i] != null)
            {
                tutorialBottlesForBreakSoundOnRestartFix[i].GetComponentInChildren<AkGameObj>().enabled = false;
                tutorialBottlesForBreakSoundOnRestartFix[i].GetComponentInChildren<AkEvent>().data.ObjectReference = null;
                tutorialBottlesForBreakSoundOnRestartFix[i].GetComponentInChildren<AkEvent>().enabled = false;
            }//End if
        }//End for

        for (int i = 0; i < chainDoorScriptsForBreakSoundOnRestartFix.Length; i++)
        {
            if(chainDoorScriptsForBreakSoundOnRestartFix[i].GetComponentInChildren<AkGameObj>() != null)
            {
                chainDoorScriptsForBreakSoundOnRestartFix[i].GetComponentInChildren<AkGameObj>().enabled = false;
                AkEvent[] events = chainDoorScriptsForBreakSoundOnRestartFix[i].GetComponentsInChildren<AkEvent>();
                for (int j = 0; j < events.Length; j++)
                {
                    events[j].data.ObjectReference = null;
                    events[j].enabled = false;
                }//End for
            }//End if
        }//End for

        bossMusic.Stop(bossMusicTrigger);

        //reloads the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadCredits(string creditsScene)
    {
        SceneManager.LoadScene(creditsScene);
    }

    public void CloseGame()
    {
        //Closes the game - this does not work in editor
        Debug.Log("Quitting the game");
        Application.Quit();
    }
}
