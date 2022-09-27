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

    //save
    public MakeStatus CurrentStatus { get; private set; } = MakeStatus.Nothing;
    public string PotionFileName { get; private set; } = null;
    public int CurrentPrice { get; private set; } = 0;
    public int CurrentLevel { get; private set; } = 0;


    //don't save
    private float reinforceDelay = 2.5f;
    private float deltaTime = 0f;

    private RectTransform gaugeRect = null;

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
    public void SaveData(SaveData saveData)
    {
        saveData.currentStatus = (int)CurrentStatus;
        saveData.potionFileName = PotionFileName;
        saveData.currentPrice = CurrentPrice;
        saveData.currentLevel = CurrentLevel;
    }

    public void LoadData(SaveData saveData)
    {
        CurrentStatus = (MakeStatus)saveData.currentStatus;
        PotionFileName = saveData.potionFileName;
        CurrentPrice = saveData.currentPrice;
        CurrentLevel = saveData.currentLevel;

        if (CurrentStatus == MakeStatus.Making)
        {
            Potion potion = Managers.Data.PotionDict[PotionFileName];
            StartCook(potion);
        }
        else
        {
            Clear();
        }
    }

    public void PutIngredient(string ingredientName)
    {
        Ingredient ingredient = Managers.Data.IngredientDict[ingredientName];

        switch (CurrentStatus)
        {
            case MakeStatus.Nothing:
                Potion potion = Managers.Data.PotionDict[ingredient.potionName];
                PotionFileName = potion.fileName;
                CurrentPrice = potion.price;
                CurrentLevel = 1;
                StartCook(potion);
                break;
            case MakeStatus.Making:
                ProceedCook(ingredient);
                break;
            default:
                Debug.Log("PutIngredient error!!");
                break;
        }

        Managers.Save.SaveData();
    }

    void StartCook(Potion potion)
    {
        Sprite[] potionSprites = Resources.LoadAll<Sprite>($"Art/Potion/Potions");
        Sprite potionSprite = null;
        foreach (Sprite sprite in potionSprites)
        {
            if (sprite.name == PotionFileName)
            {
                potionSprite = sprite;
                break;
            }
        }

        Get<GameObject>((int)GameObjects.Potion).SetActive(true);
        Get<GameObject>((int)GameObjects.Sell).SetActive(true);

        if (CurrentLevel > potion.upgrades.Count)
        {
            Get<GameObject>((int)GameObjects.Reinforce).SetActive(false);
            GetText((int)Texts.PotionLevelText).text = $"Lv.MAX";
        }
        else
        {
            Get<GameObject>((int)GameObjects.Reinforce).SetActive(true);
            GetText((int)Texts.PotionLevelText).text = $"Lv.{CurrentLevel}";
            GetText((int)Texts.SuccessRatioText).text = $"성공 확률 : {potion.upgrades[CurrentLevel - 1].ratio}%";
            GetText((int)Texts.SuccessBonusText).text = $"성공 배율 : x{potion.upgrades[CurrentLevel - 1].bonus}";
        }

        GetImage((int)Images.PotionImage).sprite = potionSprite;
        GetText((int)Texts.NameText).text = potion.name;
        GetText((int)Texts.PriceText).text = $"{CurrentPrice}";

        CurrentStatus = MakeStatus.Making;
    }

    void ProceedCook(Ingredient ingredient)
    {
        CurrentPrice += ingredient.price;
        GetText((int)Texts.PriceText).text = $"{CurrentPrice}";
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
        Potion potion = Managers.Data.PotionDict[PotionFileName];
        int seed = UnityEngine.Random.Range(0, 100);

        if (seed < potion.upgrades[CurrentLevel - 1].ratio)
        {
            CurrentPrice = (int)(CurrentPrice * potion.upgrades[CurrentLevel - 1].bonus + 0.51f);
            GetText((int)Texts.PriceText).text = $"{CurrentPrice}";

            CurrentLevel++;
            if (CurrentLevel > potion.upgrades.Count)
            {
                GetText((int)Texts.PotionLevelText).text = $"Lv.MAX";
                Get<GameObject>((int)GameObjects.Reinforce).SetActive(false);
            }
            else
            {
                GetText((int)Texts.PotionLevelText).text = $"Lv.{CurrentLevel}";
                GetText((int)Texts.SuccessRatioText).text = $"성공 확률 : {potion.upgrades[CurrentLevel - 1].ratio}%";
                GetText((int)Texts.SuccessBonusText).text = $"성공 배율 : x{potion.upgrades[CurrentLevel - 1].bonus}";
            }
        }
        else
        {
            Managers.UI.SceneHead.SellPotion(0);
            Clear();
        }

        Managers.Save.SaveData();
    }

    void ClickSellButton(PointerEventData evt)
    {
        if (CurrentStatus == MakeStatus.Nothing)
            return;

        Managers.UI.SceneHead.SellPotion(CurrentPrice);
        Clear();
        Managers.Save.SaveData();
    }

    void Clear()
    {
        Get<GameObject>((int)GameObjects.Potion).SetActive(false);
        Get<GameObject>((int)GameObjects.Sell).SetActive(false);
        Get<GameObject>((int)GameObjects.Reinforce).SetActive(false);
        Get<GameObject>((int)GameObjects.Gauge).SetActive(false);

        PotionFileName = null;
        CurrentPrice = 0;
        CurrentLevel = 0;
        CurrentStatus = MakeStatus.Nothing;
    }
}
