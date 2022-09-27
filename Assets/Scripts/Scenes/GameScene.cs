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
        UI_Head head = Managers.UI.ShowSceneUI<UI_Head>();
        UI_Tail tail = Managers.UI.ShowSceneUI<UI_Tail>();

        head.Init();
        tail.Init();

        Managers.Save.LoadData();

        Managers.Sound.Play("BGM/Main", Define.Sound.Bgm);
    }

    void Update()
    {
        
    }
}
