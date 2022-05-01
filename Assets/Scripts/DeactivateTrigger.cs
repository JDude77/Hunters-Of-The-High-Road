using UnityEngine;

public class DeactivateTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            GetComponentInChildren<BoxCollider>().enabled = false;
        }//End if
    }//End OnTriggerEnter
}