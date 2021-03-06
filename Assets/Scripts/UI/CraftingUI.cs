using UnityEngine;
using UnityEngine.UI;

public class CraftingUI: MonoBehaviour
{
    [SerializeField] private Image _leftItemIcon;
    [SerializeField] private Image _rightItemIcon;
    [SerializeField] private Image _activeRecipeIcon;

    private int _leftItemIndex = -1;
    private int _rightItemIndex = -1;
    private RecipeBook RecipeBook => GameState.Instance.Player.GetComponentInChildren<RecipeBook>();
    private AudioSource AudioSource => GameState.Instance.AudioSource;
    private Inventory Inventory => GameState.Instance.Inventory;
    private Recipe _activeRecipe;

    public void Clear()
    {
        _leftItemIndex = -1;
        _rightItemIndex = -1;
        _activeRecipe = null;
        UpdateImages();
    }

    public void AddItem(int itemIndex)
    {
        var playSound = false;
        if (_leftItemIndex < 0)
        {
            _leftItemIndex = itemIndex;
            playSound = true;
        }
        else if (_rightItemIndex < 0)
        {
            _rightItemIndex = itemIndex;
            playSound = true;
        }
        if (_leftItemIndex >= 0 && _rightItemIndex >= 0)
        {
            _activeRecipe = RecipeBook?.GetRecipe(Inventory[_leftItemIndex].Item, Inventory[_rightItemIndex].Item);
        }
        if ( playSound)
            Inventory[itemIndex]?.Item?.PlaySound();
        UpdateImages();
    }
    
    public void UpdateImages()
    {
        _leftItemIcon.enabled = _leftItemIndex >= 0;
        _rightItemIcon.enabled = _rightItemIndex >= 0;
        _activeRecipeIcon.enabled = ReferenceEquals(_activeRecipe, null) == false;
        
        if (_leftItemIcon.enabled)
            _leftItemIcon.sprite = Inventory[_leftItemIndex].Item.Icon;

        if (_rightItemIcon.enabled)
            _rightItemIcon.sprite = Inventory[_rightItemIndex].Item.Icon;

        if (_activeRecipeIcon.enabled)
            _activeRecipeIcon.sprite = _activeRecipe.Icon;
    }

    public void Combine()
    {
        if (ReferenceEquals(_activeRecipe, null) == false)
        {
            if (Inventory[_leftItemIndex].Item.IsConsumable)
                Inventory.TryRemoveAt(_leftItemIndex, 1);
            if (Inventory[_rightItemIndex].Item.IsConsumable)
                Inventory.TryRemoveAt(_rightItemIndex, 1);
            _activeRecipe.Execute(GameState.Instance.Player);
        }
        Clear();
    }
}