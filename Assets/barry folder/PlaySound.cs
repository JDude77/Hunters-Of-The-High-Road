using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour
{

    private AK.Wwise.Event Sound;
    public AK.Wwise.Event footsteps, Sword, Hipfire, Roll, Slash, BossFootsteps;

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

        if (id == "hipfire")
        {
            Sound = Hipfire;
            Playsound();
        }

        if (id == "roll")
        {
            Sound = Roll;
            Playsound();
        }

        if (id == "slash")
        {
            Sound = Slash;
            Playsound();
        }

        if (id == "bossFootsteps")
        {
            Sound = BossFootsteps;
            Playsound();
        }
    }
    private void Playsound()
    {
    Sound.Post(gameObject);
    }
    
    
    }