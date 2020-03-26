using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[ExecuteInEditMode]
public class AutoSceneLoader : MonoBehaviour
{
    public SavedSceneSetup sceneSetupAsset;
 
    [ContextMenu("Load Scene Setup")]
    void Load()
    {
        // Does this scene have a SceneSetup?
        if (sceneSetupAsset != null)
        {
            Debug.Log("Loading scene setup, found " + sceneSetupAsset.setups.Length + " scenes");
            EditorSceneManager.SaveOpenScenes();
            EditorSceneManager.RestoreSceneManagerSetup(sceneSetupAsset.setups);
        }
    }
 
    [ContextMenu("Save Scene Setup")]
    public void SaveSceneSetup()
    {
        // Save scene setup
        sceneSetupAsset = ScriptableObject.CreateInstance<SavedSceneSetup>();
        sceneSetupAsset.setups = EditorSceneManager.GetSceneManagerSetup();
        Debug.Log("Saving scene setup, found " + sceneSetupAsset.setups.Length + " scenes");
        if (AssetDatabase.Contains(sceneSetupAsset) == false)
            AssetDatabase.CreateAsset(sceneSetupAsset, "Assets/Scriptable Objects/Scene Setups/" + gameObject.scene.name + " Setup.asset");
    }

}