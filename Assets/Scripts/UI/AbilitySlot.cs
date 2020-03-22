using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class AbilitySlot : MonoBehaviour, IPointerClickHandler
{
    [FormerlySerializedAs("AbilityPopupMenuPrefab")] 
    [SerializeField] private GameObject _abilityPopupMenuPrefab;
    [SerializeField] private bool _selected;
    [SerializeField] private Image _slotHighlight;
    [SerializeField] private Image _slotImage;
    [SerializeField] private Sprite _defaultSprite;
    public Item Item { get; private set; }
    public bool IsEmpty => Item == null;

    public bool Selected
    {
        get => _selected;
        set
        {
            _selected = value;
            _slotHighlight.gameObject.SetActive(_selected);
        } 
    }

    private GameObject _objectToDestroy;

    public void Start()
    {
        if (ReferenceEquals(_slotImage, null))
            _slotImage = GetComponentInChildren<Image>();
        _defaultSprite = _slotImage?.sprite;
        _slotHighlight.gameObject.SetActive(_selected);
    }

    public void SetItem(Item item)
    {
        Item = item;
        _slotImage.sprite = (item == null) ? _defaultSprite : item.Icon;
        if (_slotImage.gameObject.activeSelf == false)
            _slotImage.gameObject.SetActive(true);
        item.PlaySound();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Scene _currentScene = SceneManager.GetActiveScene();
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Inventory"));
        _objectToDestroy =  Instantiate(_abilityPopupMenuPrefab , eventData.pressPosition * 2, Quaternion.identity);
        _objectToDestroy.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform,false);
        Debug.Log(eventData.pointerPress);
        SceneManager.SetActiveScene(_currentScene);

    }
}
