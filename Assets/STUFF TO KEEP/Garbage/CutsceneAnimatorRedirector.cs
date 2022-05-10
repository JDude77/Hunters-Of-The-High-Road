using UnityEngine;
public class CutsceneAnimatorRedirector : MonoBehaviour
{
    [SerializeField]
    private Camera mainCam, cutsceneCam;

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