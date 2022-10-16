using UnityEditor;
using UnityEditor.SceneManagement;

//https://forum.unity.com/threads/executing-first-scene-in-build-settings-when-pressing-play-button-in-editor.157502/#post-4152451

[InitializeOnLoad]
public class AutoPlayModeSceneSetup
{
    //--- Menu Toggle ---
    #region MenuToggle
    private const string MenuName = "Tools/Auto Scene Loader/Auto load Scene with build index 0 on enter playmode";
    private const string SettingName = "UseAutoSceneLoader";
 
    public static bool IsEnabled
    {
        get => EditorPrefs.GetBool(SettingName, true);
        set => EditorPrefs.SetBool(SettingName, value);
    }
          
    [MenuItem(MenuName)]
    private static void ToggleAction() => IsEnabled = !IsEnabled;

    [MenuItem(MenuName, true)]
    private static bool ToggleActionValidate()
    {
        Menu.SetChecked(MenuName, IsEnabled);
        return true;
    }
    #endregion
    //--- ---
    
    static AutoPlayModeSceneSetup()
    {
        //subscribe to playmodeStateChanged event
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }
 
    private static void OnPlayModeStateChanged(PlayModeStateChange change)
    {
        if (change != PlayModeStateChange.ExitingEditMode) return;
        
        // Set Play Mode scene to first scene defined in build settings.
        if (IsEnabled)
        {
            // Ensure at least one build scene exist.
            if (EditorBuildSettings.scenes.Length == 0) SetNoScene();
            else SetFirstScene();
        }
        //don't set any scene to first scene
        else
        {
            SetNoScene();
        }
    }

    private static void SetFirstScene() => EditorSceneManager.playModeStartScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(EditorBuildSettings.scenes[0].path);
    private static void SetNoScene() => EditorSceneManager.playModeStartScene = null;
}