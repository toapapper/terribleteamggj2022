using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    GameObject credits;
    [SerializeField]
    GameObject options;
    [SerializeField]
    GameObject buttons;

    [SerializeField] AudioMixer mixer;
    private void Awake()
    {
        DelayedUpdateVolume();
    }

    async void DelayedUpdateVolume()
    {
        await Task.Delay(10);
        UpdateMixer();
    }

    private void UpdateMixer()
    {
        float masterValue = PlayerPrefs.GetFloat("MasterVolume");
        float musicValue = PlayerPrefs.GetFloat("MusicVolume");
        float sfxValue = PlayerPrefs.GetFloat("SFXVolume");
        mixer.SetFloat("MasterVolume", Mathf.Log10(masterValue) * 30f);
        mixer.SetFloat("MusicVolume", Mathf.Log10(musicValue) * 30f);
        mixer.SetFloat("SFXVolume", Mathf.Log10(sfxValue) * 30f);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
        GameManager.Instance.ResetTimer();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ChangeToLevelSelect()
    {
        SceneManager.LoadScene("levelselect");
    }

    public void CreditsButton()
    {
        buttons.SetActive(!buttons.activeInHierarchy);
        credits.SetActive(!credits.activeInHierarchy);
    }

    public void OptionsButton()
    {
        buttons.SetActive(!buttons.activeInHierarchy);
        options.SetActive(!options.activeInHierarchy);
    }
}
