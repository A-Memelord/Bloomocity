using System;
using UnityEngine;

public class SaveGameManager : MonoBehaviour
{
    public static SaveInvData data;

    private void Awake()
    {
        data = new SaveInvData();
        SaveLoad.OnLoad += LoadData;
    }

    public void DeleteData()
    {
        SaveLoad.DeleteSaveInvData();
    }

    public static void SaveInvData()
    {
        var saveInvData = data;

        SaveLoad.Save(saveInvData);
    }

    public static void LoadData(SaveInvData _data)
    {
        data = _data;
    }

    public static void TryLoadData()
    {
        SaveLoad.Load();
    }
}
