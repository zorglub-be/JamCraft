﻿using UnityEngine;

public class Loadout : MonoBehaviour
{
    private Item _primaryAbility;
    private Item _secondaryAbility;
    private Item _specialAbility;

    public void SetPrimary(Item ability)
    {
        _primaryAbility = ability;
    }
    public void SetSecondary(Item ability)
    {
        _secondaryAbility = ability;
    }
    public void SetSpecial(Item ability)
    {
        _specialAbility = ability;
    }
    
    public void UsePrimary()
    {
        if (ReferenceEquals(_primaryAbility, null) == false) _primaryAbility.Use(GameState.Instance.Player);
    }
    public void UseSecondary()
    {
        if (ReferenceEquals(_secondaryAbility, null) == false) _secondaryAbility.Use(GameState.Instance.Player);
    }
    public void UseSpecial()
    {
        if (ReferenceEquals(_specialAbility, null) == false) _specialAbility.Use(GameState.Instance.Player);
    }
}
