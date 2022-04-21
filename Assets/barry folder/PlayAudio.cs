using UnityEngine;

public class PlayAudio : MonoBehaviour
{
    private AK.Wwise.Event sound;
    public AK.Wwise.Event footsteps, sword, hipfire, roll, slash, bossFootsteps;

    public void SelectSound(string id)
    {
        //Long story short, this line of code converts the ID to the variable name, then gets the value of the variable.
        sound = (AK.Wwise.Event) GetType().GetField(id).GetValue(this);
        PlaySound();
    }//End SelectSound

    private void PlaySound()
    {
        sound.Post(gameObject);
    }//End PlaySound
}