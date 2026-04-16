using UnityEngine;
using UnityEngine.UI;

public class pauseMenu : MonoBehaviour
{
    public static pauseMenu instance;
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private bool paused = false;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    public void OnShow()
    {
        //Stops time and shows pause panel
        //Time.timeScale = 0f;
        pausePanel.SetActive(true);
        optionsPanel.SetActive(false);
        GetComponent<Image>().enabled = true;
        paused = true;
    }
    public void OnHide()
    {
        //Resumes time and hides both pause and option menus
        //Time.timeScale = 1f;
        pausePanel.SetActive(false);
        optionsPanel.SetActive(false);
        GetComponent<Image>().enabled = false;
        paused = false;
    }
    public void OptionsMenu()
    {
        //Hides pause elements and displays options panels
        optionsPanel.SetActive(true);
        pausePanel.SetActive(false);
    }
    public void Menu()
    {
        //Go to menu Scene
    }
    public void QuitGame()
    {
        //Quit the game
    }
    public bool isPaused()
    {
        return paused;
    }
}
