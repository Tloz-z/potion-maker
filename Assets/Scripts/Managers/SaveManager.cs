using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveManager
{
    public void SaveData()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/value.fun";
        FileStream stream = new FileStream(path, FileMode.Create);

        SaveData saveData = new SaveData();
        Managers.UI.SceneHead.SaveData(saveData);
        Managers.UI.SceneTail.SaveData(saveData);

        formatter.Serialize(stream, saveData);
        stream.Close();
    }

    public void LoadData()
    {
        string path = Application.persistentDataPath + "/value.fun";
        SaveData saveData = null;

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            saveData = formatter.Deserialize(stream) as SaveData;
            stream.Close();
        }

        if (saveData == null)
            return;

        Managers.UI.SceneHead.LoadData(saveData);
        Managers.UI.SceneTail.LoadData(saveData);
    }
}
