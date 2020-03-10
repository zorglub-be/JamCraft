using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class AbilitySlot : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject AbilityPopupMenuPrefab;
    public Item Item { get; private set; }
    public bool IsEmpty => Item == null;
    private GameObject objectToDestroy;

    private Image _slotImage;
    private Sprite _defaultSprite;

    public void Start()
    {
        _slotImage = GetComponentInChildren<Image>();
        _defaultSprite = _slotImage?.sprite;
    }

    public void SetItem(Item item)
    {
        Item = item;
        _slotImage.sprite = (item == null) ? _defaultSprite : item.Icon;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Scene _currentScene = SceneManager.GetActiveScene();
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Inventory"));
        objectToDestroy =  Instantiate(AbilityPopupMenuPrefab , eventData.pressPosition * 2, Quaternion.identity);
        objectToDestroy.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform,false);
        Debug.Log(eventData.pointerPress);
        SceneManager.SetActiveScene(_currentScene);

    }
}
