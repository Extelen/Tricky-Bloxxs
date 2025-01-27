using System.IO;
using UnityEngine;

public static class SaveManager
{
    //Static Methods
    /// <summary>
    /// Save data on a file.
    /// </summary>
    public static void Save(object data)
    {
        string m_jsonString = JsonUtility.ToJson(data, true);
        File.WriteAllText(Application.persistentDataPath + "/save.json", m_jsonString);
    }

    /// <summary>
    /// Load a object from the data.
    /// </summary>
    public static Data Load<Data>() where Data : new()
    {
        Data data = new Data();

        if (GetIfFileExists())
        {
            string raw = File.ReadAllText(Application.persistentDataPath + "/save.json");
            JsonUtility.FromJsonOverwrite(raw, data);
        }

        return data;
    }

    /// <summary>
    /// Get if the file in the application persistent data path exists.
    /// </summary>
    /// <returns></returns>
    public static bool GetIfFileExists()
    {
        if (File.Exists(Application.persistentDataPath + "/save.json")) return true;
        return false;
    }
}