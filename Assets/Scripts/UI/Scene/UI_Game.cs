using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_Game : UI_Scene
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
    }



    void Update()
    {
        CreateIngredient();
    }

    const float INGREDIENT_DELAY = 1.0f;
    float _currentTime = 0.0f;
    void CreateIngredient()
    {
        _currentTime += Time.deltaTime;
        if (_currentTime < INGREDIENT_DELAY)
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
        item.gameObject.GetComponent<Image>().sprite = sprite;
        item.GetComponent<RectTransform>().position = GetObject((int)GameObjects.IngredientBar).transform.position;
    }
}
