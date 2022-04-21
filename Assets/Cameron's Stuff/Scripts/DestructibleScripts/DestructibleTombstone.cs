using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleTombstone : MonoBehaviour
{
    [SerializeField]
    private GameObject[] graveVariants;

    [SerializeField]
    private GameObject DestroyedVersion;
    private GameObject normalVersion;

    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<PlayerEventsHandler>().OnHitGravestone += DestroyTombstone;

        foreach (var grave in graveVariants)
        {
            grave.SetActive(false);
        }

        int ranNum = Random.Range(0, graveVariants.Length);
        graveVariants[ranNum].SetActive(true);

        normalVersion = graveVariants[ranNum];
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
}
