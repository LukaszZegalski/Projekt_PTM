using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider audioSlider;

    private void Start()
    {
        float volume;
        audioMixer.GetFloat("volume", out volume);
        audioSlider.value = volume;
        Screen.fullScreen = false;
        Screen.SetResolution(1280, 720, false);
    }
    //włączenie gry PvP
    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }
    //wyłaczenie gry
    public void ExitGame()
    {
        Application.Quit();
    }

    //załadowanie głównego menu
    public void Menu()
    {
        SceneManager.LoadScene(0);
    }
}
