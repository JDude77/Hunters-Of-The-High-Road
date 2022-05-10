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
        if (!boxCollider) {
            Debug.LogWarning("Grave without boxcollider detected");
        }
    }

    public void DestroyTombstone(GameObject instance)
    {
        if(instance == gameObject)
        {
            DestroyObject();
        }//End if
    }//End DestroyTombstone

    public void DestroyObject()
    {
        DestroyedVersion.transform.parent = null;
        DestroyedVersion.SetActive(true);
        normalVersion.SetActive(false);
        boxCollider.enabled = false;
        soundOnDestroy.Post(gameObject);
        //TODO: remove this once full transition to interface is done
        FindObjectOfType<PlayerEventsHandler>().OnHitGravestone -= DestroyTombstone;
    }    
}
