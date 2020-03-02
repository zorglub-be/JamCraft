using System;
using System.Collections.Generic;

public class RecipesIndex
{
    private SortedList<string, Recipe> _index = new SortedList<string, Recipe>();

    public bool TryAdd(Recipe recipe)
    {
        try
        {
            _index.Add(recipe.Id, recipe);
            return true;
        }
        catch (ArgumentException e)
        {
            return false;
        }
    }
    public Recipe GetRecipe(Item[] ingredients)
    {
        var key = Recipe.IdFromIngredients(ingredients);
        if (_index.ContainsKey(key))
            return _index[key];
        return null;
    }
}