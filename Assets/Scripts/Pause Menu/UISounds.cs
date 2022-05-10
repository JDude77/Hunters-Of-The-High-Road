using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISounds : MonoBehaviour
{
    [SerializeField]
    private AK.Wwise.Event UISound, PageSound, CloseSound;

    public void PlayUISound()
    {
        UISound.Post(gameObject);
    }//End PlayUISound

    public void PlayPageSound()
    {
        PageSound.Post(gameObject);
    }//End PlayUISound

    public void PlayCloseSound()
    {
        CloseSound.Post(gameObject);
    }//End PlayCloseSound
}
