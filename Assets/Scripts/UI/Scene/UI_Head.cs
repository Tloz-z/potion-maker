using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_Head : UI_Scene
{

    // 재료 찾는 방식 개선 필요
    enum IngredientNames
    {
        leaf_blue,
        leaf_green,
        leaf_sky,
        tako_pink,
        tako_blue,
        tako_purple,
        neko_orange,
        neko_purple,
        neko_red
    }
    

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
    public int IngredientRange { get; private set; } = 3;
    public HashSet<string> BuyItemSet { get; private set; } = new HashSet<string>();


    //don't save
    public float OffsetY { get; private set; } = 0.0f;

    void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

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

    public void SellPotion(int price)
    {
        Gold += price;
        PrintGold();
    }

    public void BuyIngredients(IngredientItem item)
    {
        IngredientRange += item.plusRange;
        Gold -= item.price;
        BuyItemSet.Add(item.name);
        PrintGold();
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

        int idx = UnityEngine.Random.Range(0, IngredientRange);
        string ingredientName = Enum.GetName(typeof(IngredientNames), idx);

        Sprite sprite = Managers.Resource.Load<Sprite>($"Art/Ingredient/{ingredientName}");
        UI_Ingredient item = Managers.UI.makeSubItem<UI_Ingredient>(GetObject((int)GameObjects.IngredientBar).transform);
        item.Init();
        item.IngredientName = ingredientName;
        item.transform.position = GetObject((int)GameObjects.IngredientBar).transform.position;
        item.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        item.gameObject.GetComponent<Image>().sprite = sprite;
    }

    void ClickMenuButton(PointerEventData evt)
    {
        Managers.UI.ShowPopupUI<UI_Store>();
    }
}
