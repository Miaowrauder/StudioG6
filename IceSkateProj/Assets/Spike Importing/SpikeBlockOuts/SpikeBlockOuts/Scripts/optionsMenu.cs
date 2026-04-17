using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.Audio;

public class optionsMenu : MonoBehaviour
{
    private string[] displayModes = { "BORDERLESS", "WINDOWED", "FULLSCREEN" };
    private FullScreenMode[] modes = { FullScreenMode.FullScreenWindow, FullScreenMode.Windowed, FullScreenMode.ExclusiveFullScreen };
    [SerializeField] private int currentMode = 0;
    private string[] resolutions = {"1920x1080", "1280x720", "640x360"};
    [SerializeField] private int currentRes = 0;
    private string[] colourModes = { "NONE", "PROTANOPIA", "DEUTERANOPIA", "TRITANOPIA" };
    [SerializeField] private int currentColour = 0;

    [SerializeField] private AudioMixer masterMixer;

    [SerializeField] private Slider mstrSlider;
    [SerializeField] private TextMeshProUGUI mstrTxt;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private TextMeshProUGUI musicTxt;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private TextMeshProUGUI sfxTxt;

    [SerializeField] private TextMeshProUGUI selectedMode;
    [SerializeField] private TextMeshProUGUI selectedRes;
    [SerializeField] private TextMeshProUGUI selectedColour;

    public void ChangeDisMode(int direction)
    {
        if (currentMode == 0 &&  direction == -1)
        {
            currentMode = 2;
        }
        else if(currentMode == 2 && direction == 1)
        {
            currentMode = 0;
        }
        else
        {
            currentMode += direction;
        }

        Screen.fullScreenMode = modes[currentMode];
        selectedMode.text = displayModes[currentMode];
    }
    public void ChangeResolution(int direction)
    {
        if (currentRes == 0 && direction == -1)
        {
            currentRes = 2;
        }
        else if (currentRes == 2 && direction == 1)
        {
            currentMode = 0;
        }
        else
        {
            currentRes += direction;
        }
        string[] dimensions = resolutions[currentRes].Split('x');
        RefreshRate rate = new RefreshRate();
        Screen.SetResolution(int.Parse(dimensions[0]), int.Parse(dimensions[1]), modes[currentMode]);
        selectedRes.text = resolutions[currentRes];
    }
    public void ChangeMasterVolume()
    {
        float value = mstrSlider.value;
        masterMixer.SetFloat("msterVol", value);
        mstrTxt.text = (100f - (value * -2.5f)).ToString();
    }    
    public void ChangeMusicVolume()
    {
        float value = musicSlider.value;
        masterMixer.SetFloat("mscVol", value);
        musicTxt.text = (100f - (value * -2.5f)).ToString();
    }    
    public void ChangeSFXVolume()
    {
        float value = sfxSlider.value;
        masterMixer.SetFloat("sfxVol", value);
        sfxTxt.text = (100f - (value * -2.5f)).ToString();
    }
    public void ChangeColourFilter(int dir)
    {
        if (currentColour == 0 && dir == -1)
        {
            currentColour = 2;
        }
        else if (currentColour == 3 && dir == 1)
        {
            currentColour = 0;
        }
        else
        {
            currentColour += dir;
        }

        //Change Colour Filter
        selectedColour.text = colourModes[currentColour];
    }
    public void OnScroll()
    {
        
    }

}
