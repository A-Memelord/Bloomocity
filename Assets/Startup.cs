using UnityEngine;

public class Startup : MonoBehaviour
{
    public SaveDataController saveDataController;
    void Start()
    {
        LoadFullGame();
    }

    public void SaveFullGame()
    {
        saveDataController.Save();
        SaveGameManager.SaveInvData();
    }

    public void LoadFullGame()
    {
        saveDataController.Load();
        SaveGameManager.TryLoadData();
    }

    public void OnDestroy()
    {
        SaveFullGame();
    }

}
