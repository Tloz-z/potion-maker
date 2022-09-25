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
    public string potionName;
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

#region Potion
[Serializable]
public class Potion
{
    public string fileName;
    public string name;
    public int price;
    public List<Upgrade> upgrades = new List<Upgrade>();
}

[Serializable]
public class Upgrade
{
    public int level;
    public int ratio;
    public float bonus;
}

[Serializable]
public class PotionData : ILoader<String, Potion>
{
    public List<Potion> potions = new List<Potion>();

    public Dictionary<String, Potion> MakeDict()
    {
        Dictionary<String, Potion> dict = new Dictionary<String, Potion>();

        foreach (Potion potion in potions)
            dict.Add(potion.fileName, potion);
        return dict;
    }
}
#endregion