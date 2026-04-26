using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class MenuManager : MonoBehaviour
{
    private bool isPaused;
    [SerializeField] private Canvas pauseMenu, mainMenu, instructionsMenu, endMenu, gameHUD;
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private Texture2D cursorImg;
    [SerializeField] private Transform[] instructPanels = new Transform[4];
    [SerializeField] private GameObject backBTN;
    [SerializeField] private GameObject menuBTN;
    [SerializeField] private GameObject nextBTN;
    [SerializeField] private TextMeshProUGUI counter;
    private int currentInst = 0;

    public bool isMain, canPause;
    // Start is called before the first frame update
    void Start()
    {
        Vector2 offset = new Vector2(10,10);
        Cursor.SetCursor(cursorImg, offset, CursorMode.Auto);
        canPause = true;
        Cursor.visible = true;
        

        if(isMain)
        {
            OnMain();
        }
        else
        {
            endMenu.enabled = false;
            optionsPanel.SetActive(false);
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
        gameHUD.enabled = false;
    }

    private void Pause()
    {
        Time.timeScale = 0f;
        Cursor.visible = true;
        gameHUD.enabled = false;
        pauseMenu.enabled = true;
        isPaused = true;
    }

    private void Unpause()
    {
        Time.timeScale = 1f;
        Cursor.visible = false;
        gameHUD.enabled = true;
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
        SceneManager.LoadScene("EllenTesting");
    }

    public void OnQuit()
    {
        Application.Quit();
    }

    public void OnInstructions()
    {
        instructionsMenu.enabled = true;
        mainMenu.enabled = false;
    }

    public void OnMain()
    {
        instructionsMenu.enabled = false;
        mainMenu.enabled = true;
    }

    public void OnPlay()
    {
        //unveil loading screen
        SceneManager.LoadScene("EllenTesting");
    }

    public void OnGameOptions()
    {
        optionsPanel.SetActive(true);
    }

    public void OnExitOptions()
    {
        optionsPanel.SetActive(false);
    }
    public void OnBack()
    {
        if (currentInst > 0)
        {
            instructPanels[currentInst].gameObject.SetActive(false);
            currentInst--;
            instructPanels[currentInst].gameObject.SetActive(true);
        }


        if (currentInst == 0)
        {
            backBTN.SetActive(false);
            menuBTN.SetActive(true);
        }
        else
        {
            backBTN.SetActive(true);
            menuBTN.SetActive(false);
        }
        counter.text = (currentInst + 1).ToString() + "/4";
    }
    public void OnNext()
    {
        if (currentInst < instructPanels.Length - 1)
        {
            instructPanels[currentInst].gameObject.SetActive(false);
            currentInst++;
            instructPanels[currentInst].gameObject.SetActive(true);
        }
        
        if (currentInst == instructPanels.Length - 1)
        {
            nextBTN.SetActive(false);
        }
        else
        {
            nextBTN.SetActive(true);
        }
        menuBTN.SetActive(false);
        counter.text = (currentInst + 1).ToString() + "/4";
    }
}
