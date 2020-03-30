using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FemaleCharacterSetup))]
public class FemaleCharacterSetupEditor : Editor {

    public override void OnInspectorGUI()
    {
        if (!Application.isPlaying)
        {
			FemaleCharacterSetup characterSetup = target as FemaleCharacterSetup;
            if (GUILayout.Button("Update Sprites"))
            {
                characterSetup.UpdateSprites(false);
            }
        }

        base.OnInspectorGUI();
    }
}
