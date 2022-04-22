using UnityEditor;
using UnityEditor.SceneManagement;

public class UsefulEditorShortcuts : Editor
{
    #region Scenes
    [MenuItem("Useful Shortcuts/Open Scene/Main Scene")]
    private static void MainScene()
    {
        if(EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            EditorSceneManager.OpenScene("Assets/Scenes/MainScene.unity", OpenSceneMode.Single);
        }//End if
    }//End MainScene

    [MenuItem("Useful Shortcuts/Open Scene/Cams Screen Scene")]
    private static void CamsScreen()
    {
        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            EditorSceneManager.OpenScene("Assets/Cameron's Stuff/Scene/CamsScreen.unity", OpenSceneMode.Single);
        }//End if
    }//End CamsScreen

    [MenuItem("Useful Shortcuts/Open Scene/Art Development Scene")]
    private static void ArtScene()
    {
        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            EditorSceneManager.OpenScene("Assets/Scenes/environment_art_scene.unity", OpenSceneMode.Single);
        }//End if
    }//End ArtScene
    #endregion

    #region Player
    [MenuItem("Useful Shortcuts/Player/Move To Boss Arena")]
    private static void MovePlayerToBossArena()
    {
        if (FindObjectOfType<Player>())
        {
            FindObjectOfType<Player>().transform.position = new UnityEngine.Vector3(20.0f, 8.0f, 145.0f);
        }//End if
        else
        {
            UnityEngine.Debug.LogError("Player not found in scene, command ignored.");
        }//End else
    }//End MovePlayerToBossArena

    [MenuItem("Useful Shortcuts/Player/Move To Start Position")]
    private static void MovePlayerToStartPosition()
    {
        if (FindObjectOfType<Player>())
        {
            FindObjectOfType<Player>().transform.position = new UnityEngine.Vector3(7.45f, 8.0f, -1.9f);
        }//End if
        else
        {
            UnityEngine.Debug.LogError("Player not found in scene, command ignored.");
        }//End else
    }//End MovePlayerToStartPosition
    #endregion
}