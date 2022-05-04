using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleTombstone : MonoBehaviour, IDestructible
{
    [SerializeField]
    private GameObject[] graveVariants;

    [SerializeField] 
    private GameObject normalVersion;

    [SerializeField]
    private AK.Wwise.Event soundOnDestroy;

    private GameObject DestroyedVersion;
    private BoxCollider boxCollider;

    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<PlayerEventsHandler>().OnHitGravestone += DestroyTombstone;

        foreach (var grave in graveVariants)
        {
            grave.SetActive(false);
        }

        if (graveVariants.Length > 0) {
            int ranNum = Random.Range(0, graveVariants.Length);
            graveVariants[ranNum].SetActive(true);

            normalVersion = graveVariants[ranNum];
            DestroyedVersion = normalVersion.transform.Find("DestroyedVersion").gameObject;
        }
        
        boxCollider = GetComponent<BoxCollider>();
    }

    public void DestroyTombstone(GameObject instance)
    {
        if(instance == gameObject)
        {
            DestroyedVersion.transform.parent = null;
            DestroyedVersion.SetActive(true);
            boxCollider.enabled = false;

            normalVersion.SetActive(false);
            soundOnDestroy.Post(gameObject);

            FindObjectOfType<PlayerEventsHandler>().OnHitGravestone -= DestroyTombstone;
        }//End if
    }//End DestroyTombstone

    public void DestroyObject()
    {
        DestroyedVersion.transform.parent = null;
        DestroyedVersion.SetActive(true);
        normalVersion.SetActive(false);
        //TODO: remove this once full transition to interface is done
        soundOnDestroy.Post(gameObject);
        FindObjectOfType<PlayerEventsHandler>().OnHitGravestone -= DestroyTombstone;
    }    
}
