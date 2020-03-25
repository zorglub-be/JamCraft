using UnityEngine;

public class ObjectsToggler : MonoBehaviour
{
    [SerializeField] private GameObject[] _objects;
    [SerializeField] private bool _toggled;
    [SerializeField] private AudioClip _toggledOnSound;
    [SerializeField] private AudioClip _toggledOffSound;
    
    
    public void ToggleAll()
    {
        _toggled = !_toggled;
        var clip = _toggled ? _toggledOnSound : _toggledOffSound;
        if (clip)
            GameState.Instance.AudioSource.PlayOneShot(clip);
        for (int i = 0; i < _objects.Length; i++)
        {
            _objects[i].SetActive(!_objects[i].activeSelf);
        }
    }
}
