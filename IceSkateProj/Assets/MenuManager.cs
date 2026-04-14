using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    private bool isPaused;
    public Canvas pauseMenu, mainMenu, settingsMenu, endMenu;
    public bool isMain, canPause;
    // Start is called before the first frame update
    void Start()
    {
        canPause = true;
        Cursor.visible = true;
        

        if(isMain)
        {
            OnMain();
        }
        else
        {
            endMenu.enabled = false;
            Unpause();
        }
       
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.M) && !isMain)
        {
            if(!isPaused)
            {
                Pause();
            }
            else if(isPaused)
            {
                Unpause();
            }
        }
        
    }

    public void OnFail()
    {
        Cursor.visible = true;
        canPause = false;
        Time.timeScale = 0f;
        endMenu.enabled = true;
    }

    private void Pause()
    {
        Time.timeScale = 0f;
        Cursor.visible = true;
        pauseMenu.enabled = true;
        isPaused = true;
    }

    private void Unpause()
    {
        Time.timeScale = 1f;
        Cursor.visible = false;
        pauseMenu.enabled = false;
        isPaused = false;
    }

    public void OnUnpause() //button variant
    {
        Unpause();
    }

    public void OnBackToMenu()
    {
        //unveil loading screen
        SceneManager.LoadScene("MainMenu");
    }

    public void OnReplay()
    {
        //unveil loading screen
        SceneManager.LoadScene("EdTestingScene");
    }

    public void OnQuit()
    {
        Application.Quit();
    }

    public void OnSettings()
    {
        settingsMenu.enabled = true;
        mainMenu.enabled = false;
    }

    public void OnMain()
    {
        settingsMenu.enabled = false;
        mainMenu.enabled = true;
    }

    public void OnPlay()
    {
        //unveil loading screen
        SceneManager.LoadScene("EdTestingScene");
    }
}
