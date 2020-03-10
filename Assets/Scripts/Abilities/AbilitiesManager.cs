using System;
using UnityEngine;
using UnityEngine.UI;

public class AbilitiesManager : MonoBehaviour
{
    [SerializeField] private RecipeBook _recipeBook;

    [SerializeField] private AbilitySlot[] _abilitySlots;

    [SerializeField] private Image _primaryAbilityUI;
    [SerializeField] private Image _secondaryAbilityUI;
    [SerializeField] private Image _specialAbilityUI;

    private int _selectedPrimaryIndex;
    private int _selectedSecondaryIndex;

    private Item _primaryAbility;
    private Item _secondaryAbility;
    private Item _specialAbility;

    private int AbilityIndex(Item ability)
    {
        for (int i = 0; i < _abilitySlots.Length; i++)
        {
            if (ReferenceEquals(_abilitySlots[i].Item, ability))
                return i;
        }
        return -1;
    }

    private void NextAbility(Action<Item> abilitySetter)
    {
        var nbAbilities = _abilitySlots.Length;
        for (int i = 1; i < _abilitySlots.Length+1; i++)
        {
            var currentSlot = _abilitySlots[nbAbilities + i % nbAbilities];
            if (currentSlot.IsEmpty || currentSlot.Selected)
                continue;
            abilitySetter?.Invoke(currentSlot.Item);
        }

        var specialRecipe = _recipeBook.GetRecipe(_primaryAbility, _secondaryAbility);
        specialRecipe?.Execute(gameObject);
    }

    public void Unlock(Item ability)
    {
        for (int i = 0; i < _abilitySlots.Length; i++)
        {
            //we don't want to unlock an ability twice
            if (ReferenceEquals(_abilitySlots[i].Item, ability))
                return;
            if (_abilitySlots[i].IsEmpty)
            {
                _abilitySlots[i].SetItem(ability);   
            }
        }
    }
    
    public void NextPrimary()
    {
        NextAbility(SetPrimary);
    }

    public void NextSecondary()
    {
        NextAbility(SetSecondary);
    }
    public void SetPrimary(Item ability)
    {
        var newIndex = AbilityIndex(ability);
        if (newIndex < 0)
            return;
        _abilitySlots[_selectedPrimaryIndex].Selected = false;
        _selectedPrimaryIndex = newIndex;
        _abilitySlots[newIndex].Selected = true;
        _primaryAbility = ability;
        _primaryAbilityUI.sprite = ability.Icon;
    }

    public void SetSecondary(Item ability)
    {
        var newIndex = AbilityIndex(ability);
        if (newIndex < 0)
            return;
        _abilitySlots[_selectedSecondaryIndex].Selected = false;
        _selectedSecondaryIndex = newIndex;
        _abilitySlots[newIndex].Selected = true;
        _secondaryAbility = ability;
        _secondaryAbilityUI.sprite = ability.Icon;
    }
    
    public void SetSpecial(Item ability)
    {
        _specialAbility = ability;
        _specialAbilityUI.sprite = ability.Icon;
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
        if (ReferenceEquals(ability, null) == false) ability.Use(GameState.Instance.Player);
    }
}


