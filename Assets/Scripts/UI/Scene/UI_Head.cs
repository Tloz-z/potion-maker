using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_Head : UI_Scene
{
    enum IngredientNames
    {
        leaf_green,
        leaf_red,
        leaf_blue,
        max_count
    }

    enum Texts
    {
        GoldText
    }

    enum Buttons
    {
        MenuButton
    }

    enum GameObjects
    {
        Panel,
        IngredientBar
    }

    public int Gold { get; private set; } = 0;
    public float OffsetY { get; private set; } = 0.0f;

    void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        Bind<Text>(typeof(Texts));
        Bind<Button>(typeof(Buttons));
        Bind<GameObject>(typeof(GameObjects));
        Gold = 0;
        GetText((int)Texts.GoldText).text = $"{Gold}";

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

    public void SellPotion(int price)
    {
        Gold += price;
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

        int idx = UnityEngine.Random.Range(0, (int)IngredientNames.max_count);
        string ingredientName = Enum.GetName(typeof(IngredientNames), idx);

        Sprite sprite = Managers.Resource.Load<Sprite>($"Art/Ingredient/{ingredientName}");
        UI_Ingredient item = Managers.UI.makeSubItem<UI_Ingredient>(GetObject((int)GameObjects.IngredientBar).transform);
        item.Init();
        item.IngredientName = ingredientName;
        item.transform.position = GetObject((int)GameObjects.IngredientBar).transform.position;
        item.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        item.gameObject.GetComponent<Image>().sprite = sprite;
    }
}
