using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class SceneSwitcher : Editor
{
    [MenuItem("Scenes/Test")]
    public static void OpenTest()
    {
        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            EditorSceneManager.OpenScene("Assets/_Game/Scenes/Test.unity");
        }
    }

    [MenuItem("Scenes/Connect #1")]
    public static void OpenConnect()
    {
        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            EditorSceneManager.OpenScene("Assets/_Game/Scenes/Connect.unity");
        }
    }

    [MenuItem("Scenes/Game #2")]
    public static void OpenGame()
    {
        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            EditorSceneManager.OpenScene("Assets/_Game/Scenes/Game.unity");
        }
    }
}
