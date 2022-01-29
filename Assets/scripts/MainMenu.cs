using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    GameObject credits;
    [SerializeField]
    GameObject buttons;

    public void StartGame()
    {
        SceneManager.LoadScene("TestMagnetScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void CreditsButton()
    {
        buttons.SetActive(!buttons.activeInHierarchy);
        credits.SetActive(!credits.activeInHierarchy);
    }
}
