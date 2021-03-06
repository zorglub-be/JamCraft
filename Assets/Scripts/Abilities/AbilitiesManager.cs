﻿using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RecipeBook))]
public class AbilitiesManager : MonoBehaviour
{
    [SerializeField] private RecipeBook _recipeBook;

    [SerializeField] private AbilitySlot[] _abilitySlots;

    [SerializeField] private Image _primaryAbilityUI;
    [SerializeField] private Image _secondaryAbilityUI;
    [SerializeField] private Image _specialAbilityUI;

    private bool _initialized;

    private Image _primaryAbilityCooldown;
    private Image _secondaryAbilityCooldown;
    private Image _specialAbilityCooldown;

    private int _selectedPrimaryIndex = -1;
    private int _selectedSecondaryIndex = -1;

    private Item _primaryAbility;
    private Item _secondaryAbility;
    private Item _specialAbility;

    private void Start()
    {
        StartCoroutine(TryInitReferences());
    }

    private IEnumerator TryInitReferences()
    {
        while (_initialized == false)
        {
            try
            {
                InitReferences();
            }
            catch (Exception e)
            {}
            yield return new WaitForSeconds(.5f);
        }
    }
    

    private void Update()
    {
        
        if (_primaryAbility)
            _primaryAbilityCooldown.fillAmount = _primaryAbility.Cooldown > 0 ? _primaryAbility.RemainingCooldown / _primaryAbility.Cooldown : 0;
        if (_secondaryAbility)
            _secondaryAbilityCooldown.fillAmount = _secondaryAbility.Cooldown > 0 ? _secondaryAbility.RemainingCooldown / _secondaryAbility.Cooldown : 0;
        if (_specialAbility)
            _specialAbilityCooldown.fillAmount = _specialAbility.Cooldown > 0 ? _specialAbility.RemainingCooldown / _specialAbility.Cooldown : 0;

    }

    //this is just for testing with InitReferences, will have to go away eventually
    [SerializeField] private Item[] _abilities;
    private Sprite _defaultSpecialSprite;

    [ContextMenu("Initialize")]
    public void InitReferences()
    {
        _recipeBook = GetComponent<RecipeBook>();
        _abilitySlots = FindObjectsOfType<AbilitySlot>();
        // we need to sort the array because Find does not guarantee hierarchical order
        Array.Sort(_abilitySlots, (as1, as2) => as1.transform.GetSiblingIndex() - as2.transform.GetSiblingIndex());
        _primaryAbilityUI = GameObject.Find("Left Power Icon").GetComponent<Image>();
        _secondaryAbilityUI = GameObject.Find("Right Power Icon").GetComponent<Image>();
        _specialAbilityUI = GameObject.Find("Centre Power Icon").GetComponent<Image>();
        _primaryAbilityCooldown = _primaryAbilityUI.transform.Find("Cooldown").GetComponent<Image>();
        _secondaryAbilityCooldown = _secondaryAbilityUI.transform.Find("Cooldown").GetComponent<Image>();
        _specialAbilityCooldown = _specialAbilityUI.transform.Find("Cooldown").GetComponent<Image>();
        _defaultSpecialSprite = _specialAbilityUI.sprite;
        for (int i = 0; i < _abilitySlots.Length; i++)
        {
            if (_abilities[i] != null)
                _abilitySlots[i].SetItem(_abilities[i]);
        }

        if (_selectedPrimaryIndex >= 0)
            SetPrimary(_selectedPrimaryIndex);
        if (_selectedSecondaryIndex >= 0)
            SetPrimary(_selectedSecondaryIndex);
        _initialized = true;
    }

    private void NextAbility(int currentIndex, Action<int> abilitySetter)
    {
        var nbAbilities = _abilitySlots.Length;
        for (int i = currentIndex + 1; i < currentIndex + nbAbilities; i++)
        {
            var abilityIndex = i % nbAbilities;
            var currentSlot = _abilitySlots[abilityIndex];
            if (currentSlot.IsEmpty)
                continue;
            abilitySetter?.Invoke(abilityIndex);
            break;
        }

        SetSpecial(null);
        var specialRecipe = _recipeBook.GetRecipe(_primaryAbility, _secondaryAbility);
        specialRecipe?.Execute(gameObject);
    }
    private void PreviousAbility(int currentIndex, Action<int> abilitySetter)
    {
        var nbAbilities = _abilitySlots.Length;
        //we use modulo to navigate backwards from currentIndex
        for (int i = currentIndex + nbAbilities - 1; i > currentIndex; i--)
        {
            var abilityIndex = i % nbAbilities;
            var currentSlot = _abilitySlots[abilityIndex];
            if (currentSlot.IsEmpty)
                continue;
            abilitySetter?.Invoke(abilityIndex);
            break;
        }

        SetSpecial(null);
        var specialRecipe = _recipeBook.GetRecipe(_primaryAbility, _secondaryAbility);
        specialRecipe?.Execute(gameObject);
    }

    public void Unlock(Item ability, int index)
    {
        if (_abilities[index] == null)
        {
            _abilities[index] = ability;
            _abilitySlots[index].SetItem(ability);
        }
        if (_selectedPrimaryIndex < 0)
            SetPrimary(index);
        else if (_selectedSecondaryIndex < 0)
            SetSecondary(index);
    }

    [ContextMenu("Next Primary")]
    public void NextPrimary()
    {
        NextAbility(_selectedPrimaryIndex, SetPrimary);
    }

    [ContextMenu("Next Secondary")]
    public void NextSecondary()
    {
        NextAbility(_selectedSecondaryIndex, SetSecondary);
    }
    
    public void PreviousPrimary()
    {
        PreviousAbility(_selectedPrimaryIndex, SetPrimary);
    }
    public void PreviousSecondary()
    {
        PreviousAbility(_selectedSecondaryIndex, SetSecondary);
    }

    public void SetPrimary(int newIndex)
    {
        SetAbility(newIndex, ref _selectedPrimaryIndex, _selectedSecondaryIndex, ref _primaryAbility,
            ref _primaryAbilityUI);
    }

    public void SetSecondary(int newIndex)
    {
        SetAbility(newIndex, ref _selectedSecondaryIndex, _selectedPrimaryIndex, ref _secondaryAbility,
            ref _secondaryAbilityUI);
    }

    public void SetAbility(int newIndex, ref int selectedIndex, int otherSelectedIndex, ref Item activeAbility,
        ref Image abilityImage)
    {
        if (newIndex < 0)
            return;
        if (otherSelectedIndex != selectedIndex && selectedIndex >= 0)
            _abilitySlots[selectedIndex].Selected = false;
        selectedIndex = newIndex;
        _abilitySlots[newIndex].Selected = true;
        activeAbility = _abilities[newIndex];
        abilityImage.sprite = activeAbility.Icon;
    }

    public void SetSpecial(Item ability)
    {
        if (ReferenceEquals(ability, null) == true)
        {
            _specialAbility = null;
            _specialAbilityUI.sprite = _defaultSpecialSprite;
        }
        else
        {
            _specialAbility = ability;
            _specialAbilityUI.sprite = ability.Icon;
        }
    }

    public void UsePrimary()
    {
        Use(_primaryAbility);
    }

    public void UseSecondary()
    {
        Use(_secondaryAbility);
    }

    public void UseSpecial()
    {
        Use(_specialAbility);
    }

    private void Use(Item ability)
    {
        if (ReferenceEquals(ability, null) == false) ability.TryUse(GameState.Instance.Player);
    }

}