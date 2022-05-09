using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterTomeMainMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject[] Pages;

    private int currentPage = 0;
    private UISounds Sounds;

    private void Start()
    {
        Sounds = GetComponent<UISounds>();
    }

    public void changePage(int increment)
    {
        foreach (var page in Pages)
        {
            page.SetActive(false);
        }

        currentPage += increment;

        if (currentPage < 0)
        {
            currentPage = Pages.Length - 1;
        }

        if(currentPage >= Pages.Length)
        {
            currentPage = 0;
        }

        
        Pages[currentPage].SetActive(true);
        Sounds.PlayPageSound();
    }

    public void linkToPage(int link)
    {
        foreach (var page in Pages)
        {
            page.SetActive(false);
        }
        currentPage = link;
        Pages[currentPage].SetActive(true);
        Sounds.PlayPageSound();
    }

    public void returnToMenu()
    {
        foreach (var page in Pages)
        {
            page.SetActive(false);
        }
        currentPage = 0;
        Pages[currentPage].SetActive(true);
        Sounds.PlayPageSound();
    }
}
