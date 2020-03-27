using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CharacterSetup))]
public class CharacterSetupEditor : Editor {

    public override void OnInspectorGUI()
    {
        if (!Application.isPlaying)
        {
            CharacterSetup characterSetup = target as CharacterSetup;
            if (GUILayout.Button("Update Sprites"))
            {
                characterSetup.UpdateSprites(false);
            }
        }

        base.OnInspectorGUI();
    }
}
