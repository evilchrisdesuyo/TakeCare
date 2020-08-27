using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIScript : MonoBehaviour
{

    public bool gamePaused;
    public GameObject pauseUI;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            //Application.Quit();
            //pause menu
            if (gamePaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        Cursor.lockState = CursorLockMode.Locked;
        pauseUI.SetActive(false);
        Time.timeScale = 1f;
        gamePaused = false;
    }

    public void Pause()
    {
        Cursor.lockState = CursorLockMode.None;
        pauseUI.SetActive(true);
       Time.timeScale = 0f;
        gamePaused = true;
    }

    //main menu
    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    //quit
    public void quit()
    {
        Application.Quit();
    }
}
