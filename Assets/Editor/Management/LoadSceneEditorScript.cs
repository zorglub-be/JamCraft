using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
public class LoadSceneEditorScript
{
 
    [MenuItem("Scene Loader/Load Scene Setup")]
    static void Load()
    {
        var path = "Assets/Scriptable Objects/Scene Setups/" + SceneManager.GetActiveScene().name + " Setup.asset";
        var sceneSetupAsset = AssetDatabase.LoadAssetAtPath<SavedSceneSetup>(path);
        // Does this scene have a SceneSetup?
        if (sceneSetupAsset != null)
        {
            Debug.Log("Loading scene setup, found " + sceneSetupAsset.setups.Length + " scenes");
            EditorSceneManager.SaveOpenScenes();
            EditorSceneManager.RestoreSceneManagerSetup(sceneSetupAsset.setups);
        }
    }
 
    [MenuItem("Scene Loader/Save Scene Setup")]
    public static void SaveSceneSetup()
    {
        // Save scene setup
        var path = "Assets/Scriptable Objects/Scene Setups/" + SceneManager.GetActiveScene().name + " Setup.asset";
        var sceneSetupAsset = AssetDatabase.LoadAssetAtPath<SavedSceneSetup>(path);
        if (sceneSetupAsset == null)
            sceneSetupAsset = ScriptableObject.CreateInstance<SavedSceneSetup>();
        sceneSetupAsset.setups = EditorSceneManager.GetSceneManagerSetup();
        Debug.Log("Saving scene setup, found " + sceneSetupAsset.setups.Length + " scenes");
        if (AssetDatabase.Contains(sceneSetupAsset) == false)
            AssetDatabase.CreateAsset(sceneSetupAsset, "Assets/Scriptable Objects/Scene Setups/" + SceneManager.GetActiveScene().name + " Setup.asset");
        AssetDatabase.SaveAssets();
    }

}