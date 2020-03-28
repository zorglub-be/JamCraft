using UnityEngine;
using UnityEngine.UI;

public class ActionKey : MonoBehaviour
{
    public NeoInput.NeoKeyCode keyCode;
    public Text text;

    private void OnEnable()
    {
        text.text = NeoInput.keyCodesMap[keyCode][0].ToString();
    }
}
