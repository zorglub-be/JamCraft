using System.Collections.Generic;
using UnityEngine;

public class RecipeBook : MonoBehaviour
{
    //Inspector
    [SerializeField] private List<Recipe> _availableRecipes = new List<Recipe>(10);

    //Statics
    private RecipesIndex _recipesIndex = new RecipesIndex();

    private void Awake()
    {
        for (int i = 0; i < _availableRecipes.Count; i++)
        {
            _recipesIndex.TryAdd(_availableRecipes[i]);
        }
    }

    public void Add(Recipe recipe)
    {
        if (_recipesIndex.TryAdd(recipe))
            _availableRecipes.Add(recipe);
    }
    
    public Recipe GetRecipe(params Item[] ingredients)
    {
        return _recipesIndex.GetRecipe(ingredients);
    }
}