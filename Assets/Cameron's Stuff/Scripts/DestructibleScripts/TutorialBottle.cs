using UnityEngine;

public class TutorialBottle : MonoBehaviour
{
    private void Start()
    {
        FindObjectOfType<PlayerEventsHandler>().OnHit += ShootBottle;
    }//End Start

    public void ShootBottle(string tag)
    {
        if(tag == "Bottle")
        {
            Destroy(gameObject);
        }//End if
    }//End ShootBottle
}
