using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuScript : MonoBehaviour
{
    [SerializeField]
    private GameObject PauseCanvas;

    [SerializeField]
    private GameObject[] AdditionalMenus;

    private bool isOnPauseMenu = false, initialPause = false;

    // Start is called before the first frame update
    void Start()
    {
        //Deactivate the canvases
        DeactivateMenus();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(initialPause == false)
            {
                PauseGame();
            }
            else
            {
                UnPauseGame();
            }
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
    }

    public void UnPauseGame()
    {
        DeactivateMenus();
        Time.timeScale = 1;
        initialPause = false;
    }

    private void DeactivateMenus()
    {
        PauseCanvas.SetActive(false);

        foreach (var menu in AdditionalMenus)
        {
            menu.SetActive(false);
        }
    }
}
