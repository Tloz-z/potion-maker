using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILoader<Key, Value>
{
    Dictionary<Key, Value> MakeDict();
}

public class DataManager
{
    public Dictionary<string, Ingredient> IngredientDict { get; private set; } = new Dictionary<string, Ingredient>();
    public Dictionary<string, Potion> PotionDict { get; private set; } = new Dictionary<string, Potion>();
    public StoreData StoreData { get; private set; }

    public void Init()
    {
        IngredientDict = LoadJson<IngredientData, string, Ingredient>("IngredientData").MakeDict();
        PotionDict = LoadJson<PotionData, string, Potion>("PotionData").MakeDict();
        StoreData = LoadStoreData("StoreData");
    }

    Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
    {
        TextAsset textAsset = Managers.Resource.Load<TextAsset>($"Data/{path}");
        return JsonUtility.FromJson<Loader>(textAsset.text);
    }

    StoreData LoadStoreData(string path)
    {
        TextAsset textAsset = Managers.Resource.Load<TextAsset>($"Data/{path}");
        return JsonUtility.FromJson<StoreData>(textAsset.text);
    }
}
