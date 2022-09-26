using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_StoreItem : UI_Base
{
    enum Images
    {
        ItemImage
    }

    enum Texts
    {
        NameText,
        PriceText
    }

    enum GameObjects
    {
        Buy
    }

    public StoreItem StoreItem { get; private set; }

    public void Init(StoreItem storeItem)
    {
        StoreItem = storeItem;
        Init();
    }

    public override void Init()
    {
        Bind<Text>(typeof(Texts));
        Bind<GameObject>(typeof(GameObjects));
        Bind<Image>(typeof(Images));

        GetText((int)Texts.NameText).text = StoreItem.name;
        GetText((int)Texts.PriceText).text = $"{StoreItem.price}";
        Sprite sprite = Managers.Resource.Load<Sprite>($"Art/Store/{StoreItem.imagePath}");
        GetImage((int)Images.ItemImage).sprite = sprite;

        Get<GameObject>((int)GameObjects.Buy).BindEvent(ClickBuyButton);
    }

    void Update()
    {
        
    }

    void ClickBuyButton(PointerEventData evt)
    {
        if (Managers.UI.SceneHead.Gold < StoreItem.price)
            return;

        StoreItem.BuyItem();
        Managers.Resource.Destroy(gameObject);
    }
}
