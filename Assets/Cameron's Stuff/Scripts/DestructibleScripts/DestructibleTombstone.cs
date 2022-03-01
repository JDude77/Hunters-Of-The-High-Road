using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleTombstone : MonoBehaviour
{
    [SerializeField]
    private GameObject DestroyedVersion;
    private GameObject normalVersion;

    // Start is called before the first frame update
    void Start()
    {
        normalVersion = gameObject;
    }

    public void destroyTombstone()
    {
        DestroyedVersion.transform.parent = null;
        DestroyedVersion.SetActive(true);

        normalVersion.SetActive(false);
    }
}
