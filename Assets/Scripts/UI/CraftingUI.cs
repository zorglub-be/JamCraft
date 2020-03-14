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
        if (_leftItemIndex < 0)
        {
            _leftItemIndex = itemIndex;
        }
        else if (_rightItemIndex < 0)
        {
            _rightItemIndex = itemIndex;
        }
        if (_leftItemIndex >= 0 && _rightItemIndex >= 0)
        {
            _activeRecipe = RecipeBook?.GetRecipe(Inventory[_leftItemIndex].Item, Inventory[_rightItemIndex].Item);
        }

        var sound = Inventory[itemIndex]?.Item?.Sound;
        if ( sound != null)
            AudioSource.PlayOneShot(sound);
        UpdateImages();
    }
    
    public void UpdateImages()
    {
        _leftItemIcon.enabled = _leftItemIndex >= 0;
        _rightItemIcon.enabled = _rightItemIndex >= 0;
        _activeRecipeIcon.enabled = _activeRecipe != null;
        
        if (_leftItemIcon.enabled)
            _leftItemIcon.sprite = Inventory[_leftItemIndex].Item.Icon;

        if (_rightItemIcon.enabled)
            _rightItemIcon.sprite = Inventory[_rightItemIndex].Item.Icon;

        if (_activeRecipeIcon.enabled)
            _activeRecipeIcon.sprite = _activeRecipe.Icon;
    }

    public void Combine()
    {
        if (_activeRecipe == null)
            return;
        Inventory.TryRemoveAt(_leftItemIndex, 1);
        Inventory.TryRemoveAt(_rightItemIndex, 1);
        _activeRecipe.Execute(GameState.Instance.Player);
        Clear();
    }
}