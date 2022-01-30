using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    GameObject credits;
    [SerializeField]
    GameObject options;
    [SerializeField]
    GameObject buttons;

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
