using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleTombstone : MonoBehaviour, IDestructible
{
    [SerializeField]
    private GameObject DestroyedVersion;
    [SerializeField] 
    private GameObject normalVersion;

    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<PlayerEventsHandler>().OnHitGravestone += DestroyTombstone;
        normalVersion = gameObject;
    }

    public void DestroyTombstone(GameObject instance)
    {
        if(instance == gameObject)
        {
            DestroyedVersion.transform.parent = null;
            DestroyedVersion.SetActive(true);

            normalVersion.SetActive(false);
            FindObjectOfType<PlayerEventsHandler>().OnHitGravestone -= DestroyTombstone;
        }//End if
    }//End DestroyTombstone

    public void DestroyObject()
    {
        DestroyedVersion.transform.parent = null;
        DestroyedVersion.SetActive(true);
        normalVersion.SetActive(false);
        //TODO: remove this once full transition to interface is done
        FindObjectOfType<PlayerEventsHandler>().OnHitGravestone -= DestroyTombstone;
    }    
}
