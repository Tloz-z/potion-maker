using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Store : UI_Popup
{

    enum GameObjects
    {
        Block,
        Cancel,
        Items
    }

    public override void Init()
    {
        base.Init();

        Bind<GameObject>(typeof(GameObjects));

        Get<GameObject>((int)GameObjects.Block).BindEvent(ClickCancel);
        Get<GameObject>((int)GameObjects.Cancel).BindEvent(ClickCancel);

        SetItems();
    }

    void Start()
    {
        Init();
    }

    void Update()
    {
        
    }

    void ClickCancel(PointerEventData evt)
    {
        Managers.UI.ClosePopupUI();
    }

    void SetItems()
    {
        HashSet<string> buyItemSet = Managers.UI.SceneHead.BuyItemSet;

        foreach(StoreItem storeItem in Managers.Data.StoreData.ingredientItems)
        {
            if (buyItemSet.Contains(storeItem.name))
                continue;

            UI_StoreItem ui = Managers.UI.makeSubItem<UI_StoreItem>(Get<GameObject>((int)GameObjects.Items).transform);
            ui.Init(storeItem);
        }
    }
}
