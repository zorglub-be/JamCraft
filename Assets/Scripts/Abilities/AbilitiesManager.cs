using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(RecipeBook))]
public class AbilitiesManager : MonoBehaviour
{
    [SerializeField] private RecipeBook _recipeBook;


    [SerializeField] private Image _primaryAbilityUI;
    [SerializeField] private Image _secondaryAbilityUI;
    [SerializeField] private Image _specialAbilityUI;
    [SerializeField] private Item[] _abilities;
    [SerializeField] private AbilitySlot[] _abilitySlots;

    public UnityEvent OnChange;

    private Sprite _defaultSpecialSprite;

    private bool _initialized;

    private Image _primaryAbilityCooldown;
    private Image _secondaryAbilityCooldown;
    private Image _specialAbilityCooldown;

    private int _selectedPrimaryIndex = -1;
    private int _selectedSecondaryIndex = -1;

    private Item _primaryAbility;
    private Item _secondaryAbility;
    private Item _specialAbility;

    public Item[] Abilities
    {
        get => _abilities;
        set { _abilities = value; }
    }

    public int SelectedPrimaryIndex
    {
        get => _selectedPrimaryIndex;
        set => _selectedPrimaryIndex = value;
    }

    public int SelectedSecondaryIndex
    {
        get => _selectedSecondaryIndex;
        set => _selectedSecondaryIndex = value;
    }

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

        if (SelectedPrimaryIndex >= 0)
            SetPrimary(SelectedPrimaryIndex);
        if (SelectedSecondaryIndex >= 0)
            SetSecondary(SelectedSecondaryIndex);
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

        UpdateSpecial();
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

        UpdateSpecial();
    }

    private void UpdateSpecial()
    {
        SetSpecial(null);
        if (_primaryAbility != null && _secondaryAbility != null)
        {
            var specialRecipe = _recipeBook.GetRecipe(_primaryAbility, _secondaryAbility);
            specialRecipe?.Execute(gameObject);
        }
    }

    public void Unlock(Item ability, int index)
    {
        if (_abilities[index] == null || _abilities[index] != ability)
        {
            _abilities[index] = ability;
            _abilitySlots[index].SetItem(ability);
        }
        if (SelectedPrimaryIndex < 0)
            SetPrimary(index);
        SetSecondary(index);
    }

    [ContextMenu("Next Primary")]
    public void NextPrimary()
    {
        NextAbility(SelectedPrimaryIndex, SetPrimary);
    }

    [ContextMenu("Next Secondary")]
    public void NextSecondary()
    {
        NextAbility(SelectedSecondaryIndex, SetSecondary);
    }
    
    public void PreviousPrimary()
    {
        PreviousAbility(SelectedPrimaryIndex, SetPrimary);
    }
    public void PreviousSecondary()
    {
        PreviousAbility(SelectedSecondaryIndex, SetSecondary);
    }

    public void SetPrimary(int newIndex)
    {
        SetAbility(newIndex, ref _selectedPrimaryIndex, SelectedSecondaryIndex, ref _primaryAbility,
            ref _primaryAbilityUI);
    }

    public void SetSecondary(int newIndex)
    {
        SetAbility(newIndex, ref _selectedSecondaryIndex, SelectedPrimaryIndex, ref _secondaryAbility,
            ref _secondaryAbilityUI);
    }

    public void SetAbility(int newIndex, ref int selectedIndex, int otherSelectedIndex, ref Item activeAbility,
        ref Image abilityImage)
    {
        if (newIndex < 0)
            return;
        var changed = selectedIndex != newIndex;
        if (otherSelectedIndex != selectedIndex && selectedIndex >= 0)
            _abilitySlots[selectedIndex].Selected = false;
        selectedIndex = newIndex;
        _abilitySlots[newIndex].Selected = true;
        activeAbility = _abilities[newIndex];
        abilityImage.sprite = activeAbility.Icon;
        if (changed)
            OnChange?.Invoke();
    }

    public void SetSpecial(Item ability)
    {
        bool changed;
        if (ReferenceEquals(ability, null) == true)
        {
            changed = _specialAbility != null;
            _specialAbility = null;
            _specialAbilityUI.sprite = _defaultSpecialSprite;
        }
        else
        {
            changed = _specialAbility != ability;
            _specialAbility = ability;
            _specialAbilityUI.sprite = ability.Icon;
        }
        if (changed)
            OnChange.Invoke();
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

    public bool IsSelected(Item itemToTrack)
    {
        return (_primaryAbility == itemToTrack || _secondaryAbility == itemToTrack);
    }
}