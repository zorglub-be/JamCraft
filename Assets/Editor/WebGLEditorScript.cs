using UnityEditor;

public class WebGLEditorScript
{
    [MenuItem("WebGL/Enable Embedded Resources")]
    public static void EnableErrorMessageTesting()
    {
        PlayerSettings.SetPropertyBool("useEmbeddedResources", true, BuildTargetGroup.WebGL);
    }
}