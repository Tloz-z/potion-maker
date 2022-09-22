using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Tail : UI_Scene
{

    enum Images
    {
        SellImage,
        ReinforceImage,
        PotionImage
    }

    enum Texts
    {
        SellText,
        ReinforceText,
        PotionLevelText,
        NameText,
        SuccessRatioText
    }

    public enum MakeStatus
    {
        Nothing,
        Making
    }
    public MakeStatus CurrentStatus { get; private set; } = MakeStatus.Nothing;

    private string potionFileName = null;
    private int currentPrice = 0;
    private int currentLevel = 0;

    void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        Bind<Image>(typeof(Images));
        Bind<Text>(typeof(Texts));
        GetImage((int)Images.SellImage).gameObject.BindEvent(ClickSellButton);

        Clear();
    }

    void Update()
    {

    }

    public void PutIngredient(string ingredientName)
    {
        Ingredient ingredient = Managers.Data.IngredientDict[ingredientName];

        switch (CurrentStatus)
        {
            case MakeStatus.Nothing:
                StartCook(ingredient);
                break;
            case MakeStatus.Making:
                ProceedCook(ingredient);
                break;
            default:
                Debug.Log("PutIngredient error!!");
                break;
        }
    }

    void StartCook(Ingredient ingredient)
    {
        Potion potion = Managers.Data.PotionDict[ingredient.potionName];
        potionFileName = potion.fileName;
        currentPrice = potion.price;
        currentLevel = 1;
        Sprite potionSprite = Managers.Resource.Load<Sprite>($"Art/Ingredient/{potionFileName}");

        GetImage((int)Images.PotionImage).sprite = potionSprite;
        GetImage((int)Images.SellImage).color = new Color32(255, 255, 255, 255);
        GetImage((int)Images.ReinforceImage).color = new Color32(255, 255, 255, 255);

        GetText((int)Texts.NameText).text = potion.name;
        GetText((int)Texts.SellText).text = $"{currentPrice}원\n포션 판매";
        GetText((int)Texts.ReinforceText).text = "강화 도전";
        GetText((int)Texts.PotionLevelText).text = $"Lv.{currentLevel}";
        GetText((int)Texts.SuccessRatioText).text = "90%";

        CurrentStatus = MakeStatus.Making;
    }

    void ProceedCook(Ingredient ingredient)
    {
        currentPrice += ingredient.price;
        GetText((int)Texts.SellText).text = $"{currentPrice}원\n포션 판매";
    }

    void ClickSellButton(PointerEventData evt)
    {
        if (CurrentStatus == MakeStatus.Nothing)
            return;

        Managers.UI.SceneHead.SellPotion(currentPrice);
        Clear();
    }

    void Clear()
    {
        GetImage((int)Images.PotionImage).sprite = null;
        GetImage((int)Images.SellImage).color = new Color32(136, 136, 136, 255);
        GetImage((int)Images.ReinforceImage).color = new Color32(136, 136, 136, 255);

        GetText((int)Texts.NameText).text = "";
        GetText((int)Texts.SellText).text = "포션 판매";
        GetText((int)Texts.ReinforceText).text = "강화 도전";
        GetText((int)Texts.PotionLevelText).text = "";
        GetText((int)Texts.SuccessRatioText).text = "";

        potionFileName = null;
        currentPrice = 0;
        currentLevel = 0;
        CurrentStatus = MakeStatus.Nothing;
    }
}
