using UnityEngine;
using UnityEngine.SceneManagement;

public class ingame_ui : MonoBehaviour
{
    public bool paused;
    public GameObject pause_menu;
    public GameObject settings_menu;
    public GameObject exit_menu;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && paused == false)
        {
            PauseGame();
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && paused == true)
        {
            ResumeGame();
        }
    }

    public void OpenSettings()
    {
        pause_menu.SetActive(false);
        settings_menu.SetActive(true);
    }

    public void CloseSettings()
    {
        pause_menu.SetActive(true);
        settings_menu.SetActive(false);
    }

    public void OpenExitMenu()
    {
        pause_menu.SetActive(false);
        exit_menu.SetActive(true);
    }
    public void CloseExitMenu()
    {
        pause_menu.SetActive(true);
        exit_menu.SetActive(false);
    }

    public void CloseGame()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void PauseGame()
    {
        paused = true;
        pause_menu.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void ResumeGame()
    {
        paused = false;
        pause_menu.SetActive(false);
        settings_menu.SetActive(false);
        exit_menu.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
