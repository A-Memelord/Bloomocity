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

        // TODO: Anthony, this gets called twice for some reason, the first time it works and the second time it only serializes 2 values. Use breakpoints to figure out why.
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
