using UnityEngine;
using UnityEngine.UI;

public class Heart : MonoBehaviour
{
    [SerializeField] private Image _fill;

    [SerializeField] private Image _backgroundFill;
    // Start is called before the first frame update

    public void SetFill(float fill)
    {
        _fill.fillAmount = fill;
    }

    public void SetBackgroundFill(float fill)
    {
        _backgroundFill.fillAmount = fill;
    }
}
