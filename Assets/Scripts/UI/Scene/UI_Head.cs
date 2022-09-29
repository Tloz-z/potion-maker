using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;

public class UI_Head : UI_Scene
{
    enum Texts
    {
        GoldText
    }

    enum GameObjects
    {
        Panel,
        IngredientBar,
        MenuButton
    }

    //save
    public int Gold { get; private set; } = 0;
    public List<string> Ingredients { get; private set; } = new List<string>();
    public HashSet<string> BuyItemSet { get; private set; } = new HashSet<string>();


    //don't save
    public float OffsetY { get; private set; } = 0.0f;

    public override void Init()
    {
        base.Init();

        Ingredients.Add("leaf_blue");
        Ingredients.Add("leaf_green");
        Ingredients.Add("leaf_sky");

        Bind<Text>(typeof(Texts));
        Bind<GameObject>(typeof(GameObjects));

        Gold = 0;
        PrintGold();
        Get<GameObject>((int)GameObjects.MenuButton).BindEvent(ClickMenuButton);

        float scaleHeight = ((float)Screen.width / Screen.height) / ((float)9 / 16);

        if (scaleHeight > 1)
            return;

        OffsetY = Math.Abs(((Screen.width * 1080 / Screen.width) * (Screen.height * 1080 / Screen.width)) - (1080 * 1920)) / 2 / 1080;
        Get<GameObject>((int)(GameObjects.Panel)).GetComponent<RectTransform>().anchoredPosition += new Vector2(0, OffsetY);
    }

    void Update()
    {
        CreateIngredient();
    }

    public void SaveData(SaveData saveData)
    {
        saveData.gold = Gold;
        saveData.ingredients = Ingredients.ToArray();
        saveData.buyItem = BuyItemSet.ToArray();
    }

    public void LoadData(SaveData saveData)
    {
        Gold = saveData.gold;

        foreach (string ingredient in saveData.ingredients)
            Ingredients.Add(ingredient);

        foreach (string item in saveData.buyItem)
            BuyItemSet.Add(item);

        PrintGold();
    }

    public void SellPotion(int price)
    {
        Gold += price;
        PrintGold();
    }

    public void BuyIngredients(IngredientItem item)
    {
        foreach (string ingredient in item.ingredients)
            Ingredients.Add(ingredient);

        Gold -= item.price;
        BuyItemSet.Add(item.name);
        PrintGold();
        Managers.Save.SaveData();
    }


    void PrintGold()
    {
        GetText((int)Texts.GoldText).text = $"{Gold}";
    }

    float _ingredientDelay = 0.7f;
    float _currentTime = 0.0f;
    void CreateIngredient()
    {
        if (Managers.UI.SceneTail.CurrentStatus == UI_Tail.MakeStatus.Nothing)
            _ingredientDelay = 0.7f;
        else
            _ingredientDelay = 0.1f;

        _currentTime += Time.deltaTime;
        if (_currentTime < _ingredientDelay)
        {
            return;
        }
        _currentTime = 0.0f;

        int idx = UnityEngine.Random.Range(0, Ingredients.Count);
        string ingredientName = Ingredients[idx];

        Sprite sprite = Managers.Resource.Load<Sprite>($"Art/Ingredient/{ingredientName}");
        UI_Ingredient item = Managers.UI.makeSubItem<UI_Ingredient>(GetObject((int)GameObjects.IngredientBar).transform);
        item.Init();
        item.IngredientName = ingredientName;
        item.transform.position = GetObject((int)GameObjects.IngredientBar).transform.position;
        item.gameObject.GetComponent<Image>().sprite = sprite;
    }

    void ClickMenuButton(PointerEventData evt)
    {
        Managers.UI.ShowPopupUI<UI_Store>();
    }
}
