using System.IO;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.Events;

public static class SaveLoad
{
    public static UnityAction OnSave;
    public static UnityAction<SaveData> OnLoad;

    private static string directory = "/SaveData/";
    private static string fileName = "SaveImage.pdf";

    public static bool Save(SaveData data)
    {
        OnSave?.Invoke();

        string dir = Application.persistentDataPath + directory;

        if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(dir + fileName, json);

        Debug.Log("Saving Game");

        return true;
    }

    public static SaveData Load()
    {
        string fullPath = Application.persistentDataPath + directory + fileName;
        SaveData data = new SaveData();

        if (File.Exists(fullPath))
        {
            string json = File.ReadAllText(fullPath);
            data = JsonUtility.FromJson<SaveData>(json);
            OnLoad?.Invoke(data);
            Debug.Log("Loading Game");
        }
        else
        {
            Debug.Log("No Save Data Found");
        }

        return data;
    }
}
