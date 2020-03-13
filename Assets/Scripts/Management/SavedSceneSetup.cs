using System;
using UnityEditor.SceneManagement;
using UnityEngine;

[Serializable]
public class SavedSceneSetup : ScriptableObject
   {
       public SceneSetup[] setups;
   }