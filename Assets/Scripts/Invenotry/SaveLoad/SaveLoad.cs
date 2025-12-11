using System.IO;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.Events;

public static class SaveLoad
{
    public static UnityAction OnSave;
    public static UnityAction<SaveInvData> OnLoad;

    private static string directory = "/SaveData/";
    private static string fileName = "SaveImage.json";

    public static bool Save(SaveInvData data)
    {
        OnSave?.Invoke();

        GUIUtility.systemCopyBuffer = directory;

        string dir = Application.persistentDataPath + directory;

        if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(dir + fileName, json);

        Debug.Log("Saving Game");

        return true;
    }

    public static SaveInvData Load()
    {
        string fullPath = Application.persistentDataPath + directory + fileName;
        SaveInvData data = new SaveInvData();

        if (File.Exists(fullPath))
        {
            string json = File.ReadAllText(fullPath);
            data = JsonUtility.FromJson<SaveInvData>(json);
            OnLoad?.Invoke(data);
            Debug.Log("Loading Game");
        }
        else
        {
            Debug.Log("No Save Data Found");
        }

        return data;
    }

    public static void DeleteSaveInvData()
    {
        string fullPath = Application.persistentDataPath + directory + fileName;

        if (File.Exists(fullPath)) File.Delete(fullPath);
    }
}