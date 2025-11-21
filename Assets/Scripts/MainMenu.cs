using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string GAME_SCENE_NAME = "";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayGame()
    {
        Debug.Log("Play Game button clicked");
        SceneManager.LoadScene(GAME_SCENE_NAME);
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game button clicked");
        Application.Quit();
    }
}
