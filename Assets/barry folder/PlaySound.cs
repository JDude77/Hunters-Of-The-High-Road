using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour
{

    private AK.Wwise.Event Sound;
    public AK.Wwise.Event footsteps, Sword;

    public void SelectSound(string id)
    {
    if (id =="footsteps")
        {
            Sound = footsteps;
            Playsound();
        }

        if (id == "sword")
        {
            Sound = Sword;
            Playsound();
        }

    }
    private void Playsound()
    {
    Sound.Post(gameObject);
    }
    
    
    }