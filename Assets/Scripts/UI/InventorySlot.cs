using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private TextMeshProUGUI _text;
    private Item _item;

    public void AddItem(Item item, int count)
    {
        _item = item;
        _icon.sprite = _item.Icon;
        _icon.gameObject.SetActive(true);
        _text.text = count.ToString();
        _text.gameObject.SetActive(true);

    }

    public void ClearSlot()
    {
        _item = null;

        _icon.sprite = null;
        _icon.gameObject.SetActive(false);
        _text.text = null;
        _text.gameObject.SetActive(false);
    }
}
