using UnityEngine;
using UnityEngine.SceneManagement;

public class mainMenu : MonoBehaviour
{
    [SerializeField] private GameObject optionsPanel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void showOptions()
    {
        optionsPanel.SetActive(true);
    }    
    public void hideOptions()
    {
        optionsPanel.SetActive(false);
    }
    public void playGame()
    {
        SceneManager.LoadScene("SpikeBlockOutPause");
    }
}
