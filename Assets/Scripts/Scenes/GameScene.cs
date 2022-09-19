using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{
    public override void Clear()
    {
        Debug.Log("asdf");
    }

    void Start()
    {
        Managers.UI.ShowSceneUI<UI_Game>();
    }

    void Update()
    {
        
    }
}
