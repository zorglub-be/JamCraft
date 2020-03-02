using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Object = System.Object;

[CreateAssetMenu(menuName = "Crafting/Recipe")]
public class Recipe : ScriptableObject
{
    //Inspector
    [SerializeField] private string _name;
    [SerializeField] private Sprite _icon;
    [SerializeField] private Item[] _ingredients;
    [SerializeField] private GameEffect[] _effects;
    
    //Publics
    public string Id => _id;
    
    //Privates
    private string _id;
    
    private void OnValidate()
    {
        Array.Sort(_ingredients, new Comparison<Item>((i1, i2) => String.Compare(i1.Name, i2.Name, StringComparison.Ordinal)));
        _id = IdFromIngredients(_ingredients);
    }

    /// <summary>
    /// Returns the identifier resulting from the combination of given ingredients.
    /// </summary>
    /// <param name="ingredients"></param>
    /// <returns></returns>
    public static string IdFromIngredients(Item[] ingredients)
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < ingredients.Length; i++)
        {
            sb.Append(ingredients[i].Name);
        }
        return sb.ToString();
    }
}

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
    
    public Recipe GetRecipe(Item[] ingredients)
    {
        return _recipesIndex.GetRecipe(ingredients);
    }
}


public class SoundEffect : GameEffect
{
    private AudioClip _audioClip;

    public override void Execute()
    {
        gameState.AudioSource.PlayOneShot(_audioClip);
    }
}

public abstract class GameEffect : ScriptableObject, IGameEffect
{
   public static GameState gameState;

   public void Awake()
   {
       if (gameState == null)
           FindObjectOfType<GameState>();
   }

   public abstract void Execute();
}

public class GameState: MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;

    public AudioSource AudioSource
    {
        get => _audioSource;
        set => _audioSource = value;
    }
}

[CreateAssetMenu(menuName = "Item")]
public class Item : ScriptableObject, IItem
{
    [SerializeField] private string _name;
    [SerializeField] private Sprite _icon;
    [SerializeField] private Sprite _sprite;

    public string Name => _name;
    public Sprite Icon => _icon;
    public Sprite Sprite => _sprite;
}

public class RecipesIndex
{
    private SortedList<string, Recipe> _index;

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
        return _index[Recipe.IdFromIngredients(ingredients)];
    }
}


public interface IGameEffect
{
    void Execute();
}

public interface IItem
{
    string Name { get; }
    Sprite Icon { get; }
    Sprite Sprite { get; }
}
