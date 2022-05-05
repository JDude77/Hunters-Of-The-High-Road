using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuScript : MonoBehaviour
{
    [SerializeField]
    private GameObject PauseCanvas;

    [SerializeField]
    private bool isMainMenu;

    [SerializeField]
    private GameObject[] AdditionalMenus;
    private GameObject CurrentMenu;

    private bool isOnPauseMenu = false, initialPause = false;

    // Start is called before the first frame update
    void Start()
    {
        //Deactivate the menus
        if(!isMainMenu)
        DeactivateMenus();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(initialPause == false && !isMainMenu)
            {
                PauseGame();
                //sets CurrentMenu to the pause menu gameobject. This is needed for checking if the current menu has a "ReturnToMenu" component
                CurrentMenu = gameObject;
            }
            else
            {
                CheckEscInput();
            }
        }
    }

    private void CheckEscInput()
    {
        //if the current menu has a ReturnToMenu component, activate the menu attached to the script
        if(CurrentMenu.GetComponent<ReturnToMenu>() == true)
        {
            ActivateMenu(CurrentMenu.GetComponent<ReturnToMenu>().getReturnMenu());
        }
        else
        {
            UnPauseGame();
        }
        
    }
    private void PauseGame()
    {
        PauseCanvas.SetActive(true);
        Time.timeScale = 0;
        initialPause = true;
    }

    //used by buttons to activate a menu
    public void ActivateMenu(GameObject menu)
    {
        DeactivateMenus();
        menu.SetActive(true);
        CurrentMenu = menu;
    }

    //Used by the "Resume" button
    public void UnPauseGame()
    {
        DeactivateMenus();
        Time.timeScale = 1;
        initialPause = false;
    }

    //deactivates all menus
    private void DeactivateMenus()
    {
        PauseCanvas.SetActive(false);

        foreach (var menu in AdditionalMenus)
        {
            menu.SetActive(false);
        }
    }
}
