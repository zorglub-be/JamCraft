using UnityEngine;
using UnityEngine.UI;

public class CraftingUI: MonoBehaviour
{
    [SerializeField] private Image _leftItemIcon;
    [SerializeField] private Image _rightItemIcon;
    [SerializeField] private Image _activeRecipeIcon;

    private int _leftItemIndex;
    private int _rightItemIndex;
    private RecipeBook RecipeBook => GameState.Instance.Player.GetComponentInChildren<RecipeBook>();
    private Inventory Inventory => GameState.Instance.Inventory;
    private Recipe _activeRecipe;

    public void Clear()
    {
        _leftItemIndex = -1;
        _rightItemIndex = -1;
        _activeRecipe = null;
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
    }
    
    public void UpdateImages(int cursorIndex)
    {
        if (_leftItemIndex >= 0)
            _leftItemIcon.sprite = Inventory[_leftItemIndex].Item.Icon;
        if (_rightItemIndex >= 0)
            _rightItemIcon.sprite = Inventory[_rightItemIndex].Item.Icon;
        if (_activeRecipe != null)
            _activeRecipeIcon.sprite = _activeRecipe.Icon;
    }

    public void Combine()
    {
        var recipe = RecipeBook?.GetRecipe(Inventory[_leftItemIndex].Item, Inventory[_rightItemIndex].Item);
        if (recipe == null)
            return;
        recipe.Execute(GameState.Instance.Player);
        Inventory.TryRemoveAt(_leftItemIndex, 1);
        Inventory.TryRemoveAt(_rightItemIndex, 1);
        Clear();
    }
}