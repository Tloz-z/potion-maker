using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define : MonoBehaviour
{
    public enum Scene
    {
        Unknown,
        Login,
        Game
    }

    public enum Sound
    {
        Bgm,
        Effect,
        MaxCount
    }

    public enum UIEvent
    {
        Click,
        Drag,
        ClickDown
    }

    public enum MouseEvent
    {
        Press,
        Click
    }

}
