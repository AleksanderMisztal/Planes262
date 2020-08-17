using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class DisableScripReloadInPlayMode
{
    static DisableScripReloadInPlayMode()
    {
        EditorApplication.playModeStateChanged
            += OnPlayModeStateChanged;
    }

    static void OnPlayModeStateChanged(PlayModeStateChange stateChange)
    {
        switch (stateChange)
        {
            case (PlayModeStateChange.EnteredPlayMode):
                {
                    EditorApplication.LockReloadAssemblies();
                    Debug.Log("Assembly Reload locked as entering play mode");
                    break;
                }
            case (PlayModeStateChange.ExitingPlayMode):
                {
                    Debug.Log("Assembly Reload unlocked as exiting play mode");
                    EditorApplication.UnlockReloadAssemblies();
                    break;
                }
        }
    }

}