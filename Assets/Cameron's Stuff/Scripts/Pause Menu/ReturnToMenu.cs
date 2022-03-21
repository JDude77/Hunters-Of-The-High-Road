using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnToMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject returnToMenu;
    
    public GameObject getReturnMenu()
    {
        return returnToMenu;
    }
}
