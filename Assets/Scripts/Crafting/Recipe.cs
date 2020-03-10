using System;
using System.Net.Configuration;
using System.Text;
using UnityEngine;

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
        _id = IdFromIngredients(_ingredients);
        if (_name.Length == 0)
            _name = name; //set the name by default to be the name of the scriptable object instance
    }

    /// <summary>
    /// Returns the identifier resulting from the combination of given ingredients.
    /// </summary>
    /// <param name="ingredients"></param>
    /// <returns></returns>
    public static string IdFromIngredients(Item[] ingredients)
    {
        Array.Sort(ingredients, new Comparison<Item>((i1, i2) => String.Compare(i1?.Name, i2?.Name, StringComparison.Ordinal)));
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < ingredients.Length; i++)
        {
            sb.Append(ingredients[i]?.Name);
        }
        return sb.ToString();
    }

    public void Execute(GameObject source)
    {
        for (int i = 0; i < _effects.Length; i++)
        {
                _effects[i].Execute(source);
        }
    }
}