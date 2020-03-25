using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInventory : MonoBehaviour
{
    [SerializeField] private Inventory _inventory;
    public Inventory Inventory => _inventory;
}
