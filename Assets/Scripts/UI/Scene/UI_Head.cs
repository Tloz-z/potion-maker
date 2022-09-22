﻿using System.Collections;
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

        float scaleHeight = ((float)Screen.width / Screen.height) / ((float)9 / 16);

        if (scaleHeight > 1)
            return;

        float offsetY = Math.Abs(((Screen.width * 1080 / Screen.width) * (Screen.height * 1080 / Screen.width)) - (1080 * 1920)) / 2 / 1080;
        Get<GameObject>((int)(GameObjects.Panel)).GetComponent<RectTransform>().anchoredPosition += new Vector2(0, offsetY);
    }

    void Update()
    {
        CreateIngredient();
    }

    float _ingredientDelay = 1.0f;
    float _currentTime = 0.0f;
    void CreateIngredient()
    {
        _currentTime += Time.deltaTime;
        if (_currentTime < _ingredientDelay)
        {
            return;
        }
        _currentTime = 0.0f;

        int idx = UnityEngine.Random.Range(0, (int)IngredientNames.max_count);
        string name = Enum.GetName(typeof(IngredientNames), idx);

        Ingredient ingredient = Managers.Data.IngredientDict[name];
        Sprite sprite = Managers.Resource.Load<Sprite>($"Art/Ingredient/{name}");
        UI_Ingredient item = Managers.UI.makeSubItem<UI_Ingredient>(GetObject((int)GameObjects.IngredientBar).transform);
        item.Ingredient = ingredient;
        item.transform.position = GetObject((int)GameObjects.IngredientBar).transform.position;
        item.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        item.gameObject.GetComponent<Image>().sprite = sprite;
    }
}