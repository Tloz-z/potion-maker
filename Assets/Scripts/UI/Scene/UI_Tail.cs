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
        PotionImage,
        GoldImage,
        FullImage
    }

    enum Texts
    {
        SellText,
        ReinforceText,
        PotionLevelText,
        NameText,
        SuccessRatioText,
        SuccessBonusText,
        PriceText
    }

    enum GameObjects
    {
        Potion,
        Sell,
        Reinforce,
        Gauge
    }

    public enum MakeStatus
    {
        Nothing,
        Making,
        Reinforcing
    }
    public MakeStatus CurrentStatus { get; private set; } = MakeStatus.Nothing;

    private string potionFileName = null;
    private int currentPrice = 0;
    private int currentLevel = 0;
    private float reinforceDelay = 2.5f;
    private float deltaTime = 0f;

    private RectTransform gaugeRect = null;

    void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        Bind<Image>(typeof(Images));
        Bind<Text>(typeof(Texts));
        Bind<GameObject>(typeof(GameObjects));
        GetText((int)Texts.SellText).gameObject.BindEvent(ClickSellButton);
        GetText((int)Texts.ReinforceText).gameObject.BindEvent(ClickReinforceButton);

        gaugeRect = GetImage((int)Images.FullImage).gameObject.GetComponent<RectTransform>();

        Clear();
    }

    void Update()
    {
        if (CurrentStatus == MakeStatus.Reinforcing)
        {
            deltaTime += Time.deltaTime;
            gaugeRect.sizeDelta = new Vector2((deltaTime / reinforceDelay) * 920f, 100f);
            if (deltaTime > reinforceDelay)
            {
                CurrentStatus = MakeStatus.Making;
                deltaTime = 0f;
                Get<GameObject>((int)GameObjects.Gauge).SetActive(false);
                Reinforce();
            }
        }
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
        Sprite[] potionSprites = Resources.LoadAll<Sprite>($"Art/Potion/Potions");
        Sprite potionSprite = null;
        foreach (Sprite sprite in potionSprites)
        {
            if (sprite.name == potionFileName)
            {
                potionSprite = sprite;
                break;
            }
        }

        Get<GameObject>((int)GameObjects.Potion).SetActive(true);
        Get<GameObject>((int)GameObjects.Sell).SetActive(true);
        Get<GameObject>((int)GameObjects.Reinforce).SetActive(true);

        GetImage((int)Images.PotionImage).sprite = potionSprite;
        GetText((int)Texts.NameText).text = potion.name;
        GetText((int)Texts.PotionLevelText).text = $"Lv.1";
        GetText((int)Texts.SuccessRatioText).text = $"성공 확률 : {potion.upgrades[0].ratio}%";
        GetText((int)Texts.SuccessBonusText).text = $"성공 배율 : x{potion.upgrades[0].bonus}";
        GetText((int)Texts.PriceText).text = $"{currentPrice}";

        CurrentStatus = MakeStatus.Making;
    }

    void ProceedCook(Ingredient ingredient)
    {
        currentPrice += ingredient.price;
        GetText((int)Texts.PriceText).text = $"{currentPrice}";
    }

    void ClickReinforceButton(PointerEventData evt)
    {
        if (CurrentStatus == MakeStatus.Nothing)
            return;

        CurrentStatus = MakeStatus.Reinforcing;
        Get<GameObject>((int)GameObjects.Gauge).SetActive(true);
        gaugeRect.sizeDelta = new Vector2(0f, 100f);
    }

    void Reinforce()
    {
        Potion potion = Managers.Data.PotionDict[potionFileName];
        int seed = UnityEngine.Random.Range(0, 100);

        if (seed < potion.upgrades[currentLevel - 1].ratio)
        {
            currentPrice = (int)(currentPrice * potion.upgrades[currentLevel - 1].bonus + 0.51f);
            GetText((int)Texts.PriceText).text = $"{currentPrice}";

            currentLevel++;
            if (currentLevel > potion.upgrades.Count)
            {
                GetText((int)Texts.PotionLevelText).text = $"Lv.MAX";
                Get<GameObject>((int)GameObjects.Reinforce).SetActive(false);
            }
            else
            {
                GetText((int)Texts.PotionLevelText).text = $"Lv.{currentLevel}";
                GetText((int)Texts.SuccessRatioText).text = $"성공 확률 : {potion.upgrades[currentLevel - 1].ratio}%";
                GetText((int)Texts.SuccessBonusText).text = $"성공 배율 : x{potion.upgrades[currentLevel - 1].bonus}";
            }
        }
        else
        {
            Managers.UI.SceneHead.SellPotion(0);
            Clear();
        }
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
        Get<GameObject>((int)GameObjects.Potion).SetActive(false);
        Get<GameObject>((int)GameObjects.Sell).SetActive(false);
        Get<GameObject>((int)GameObjects.Reinforce).SetActive(false);
        Get<GameObject>((int)GameObjects.Gauge).SetActive(false);

        potionFileName = null;
        currentPrice = 0;
        currentLevel = 0;
        CurrentStatus = MakeStatus.Nothing;
    }
}
