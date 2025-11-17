using Newtonsoft.Json;
using System;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveDataController : MonoBehaviour
{
    [SerializeField] private string filePath;
    [SerializeField] private string fileName;

    public SaveDataObject defaultData;
    public static SaveData currentData;

    //public TMP_InputField resetField;
    //private string resetKey = "RESET";
    //private string currentResetKey = "";

    private void Awake() => Load();

    private void OnDestroy() => Save();
    private void OnApplicationQuit() => Save();

    private void OnApplicationPause(bool pause)
    {
        if (pause) Save();
        else
        {
            Load();
            //HandleOfflineEarnings();
        }
    }

    public void Load()
    {
        SaveData loadedData = Serializer.Load(defaultData.defaultData, Path.Combine(Application.persistentDataPath, filePath), fileName);
        currentData = JsonConvert.DeserializeObject<SaveData>(JsonConvert.SerializeObject(loadedData));
    }

    public void Save()
    {
        if (currentData == null) return;
        Serializer.Save(currentData, Path.Combine(Application.persistentDataPath, filePath), fileName);
    }

    //public void OnResetValueChange() => currentResetKey = resetField.text.Trim().ToUpper();

    //public void DeleteData()
    //{
    //    OnResetValueChange();
    //    if (currentResetKey != resetKey) return;

    //    string fullPath = Path.Combine(Application.persistentDataPath, filePath, fileName);

    //    try
    //    {
    //        if (File.Exists(fullPath))
    //            File.Delete(fullPath);

    //        currentData = defaultData.defaultData;
    //        Save();
    //        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    //    }
    //    catch (Exception ex)
    //    {
    //        Debug.LogError($"Failed to delete save data: {ex.Message}");
    //    }
    //}
}
