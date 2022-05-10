using UnityEngine;

public class TutorialBottle : MonoBehaviour
{
    private void Start()
    {
        FindObjectOfType<PlayerEventsHandler>().OnHitBottle += ShootBottle;
    }//End Start

    public void ShootBottle(GameObject instance)
    {
        if(instance == gameObject)
        {
            Destroy(gameObject);
            FindObjectOfType<PlayerEventsHandler>().OnHitBottle -= ShootBottle;
        }//End if
    }//End ShootBottle
}
