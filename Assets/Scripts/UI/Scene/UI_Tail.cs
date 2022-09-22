using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        Bind<Image>(typeof(Images));
        Bind<Text>(typeof(Texts));
        Clear();
    }

    void Update()
    {

    }

    void Clear()
    {
        GetImage((int)Images.PotionImage).sprite = null;
        GetImage((int)Images.SellImage).color = new Color32(136, 136, 136, 100);
        GetImage((int)Images.ReinforceImage).color = new Color32(136, 136, 136, 100);

        GetText((int)Texts.NameText).text = "";
        GetText((int)Texts.SellText).text = "포션 판매";
        GetText((int)Texts.ReinforceText).text = "강화 도전";
        GetText((int)Texts.PotionLevelText).text = "";
        GetText((int)Texts.SuccessRatioText).text = "";
    }
}
