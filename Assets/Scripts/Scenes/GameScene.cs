using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{
    public override void Clear()
    {
        Debug.Log("GameScene Clear!!!");
    }

    void Start()
    {
        Managers.UI.ShowSceneUI<UI_Head>(true);
        Managers.UI.ShowSceneUI<UI_Tail>(false);
        Managers.Sound.Play("BGM/Main", Define.Sound.Bgm);
    }

    void Update()
    {
        
    }
}
