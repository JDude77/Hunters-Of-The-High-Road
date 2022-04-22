using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private Animator ani;
    private Player player;
    private Boss boss;



    // Start is called before the first frame update
    void Start()
    {
        ani = GetComponent<Animator>();
        player = FindObjectOfType<Player>();
        boss = FindObjectOfType<Boss>();
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
    }

    //public methods are used by buttons
    public void RestartGame()
    {
        //reloads the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void CloseGame()
    {
        //Closes the game - this does not work in editor
        Debug.Log("Quitting the game");
        Application.Quit();
    }
}
