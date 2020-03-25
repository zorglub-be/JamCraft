using UnityEngine;

public class CraftTest : MonoBehaviour
{
    [SerializeField] private Item[] _ingredients;
    [SerializeField] private RecipeBook _recipes;

    [ContextMenu("Craft")]
    public void Craft()
    {
        _recipes.GetRecipe(_ingredients)?.Execute(gameObject);
    }
}
