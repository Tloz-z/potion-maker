using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveData
{
    //UI_Head
    public int gold;
    public string[] ingredients;
    public string[] buyItem;

    //UI_Tail
    public int currentStatus;
    public string potionFileName;
    public int currentPrice;
    public int currentLevel;

}
