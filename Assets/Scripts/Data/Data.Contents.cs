using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region Ingredient
[Serializable]
public class Ingredient
{
    public string name;
    public int price;
}

[Serializable]
public class IngredientData : ILoader<String, Ingredient>
{
    public List<Ingredient> ingredients = new List<Ingredient>();

    public Dictionary<String, Ingredient> MakeDict()
    {
        Dictionary<String, Ingredient> dict = new Dictionary<String, Ingredient>();

        foreach (Ingredient ingredient in ingredients)
            dict.Add(ingredient.name, ingredient);
        return dict;
    }
}
#endregion