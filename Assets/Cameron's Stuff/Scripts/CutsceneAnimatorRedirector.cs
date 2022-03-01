using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneAnimatorRedirector : MonoBehaviour
{
    [SerializeField]
    private PlaceHolderBoss boss;

    [SerializeField]
    private Camera mainCam, cutsceneCam;
    
    public void activateBossTurning()
    {
        boss.activateDramaticTurn();
    }

    public void activateCutsceneCam()
    {
        cutsceneCam.depth = 2;
        mainCam.depth = 0;
    }
    public void activateMainCam()
    {
        cutsceneCam.depth = 0;
        mainCam.depth = 1;
    }
}
