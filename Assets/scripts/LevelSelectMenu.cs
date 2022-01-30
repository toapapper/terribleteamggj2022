using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LevelSelectMenu : MonoBehaviour
{
    [SerializeField]
    List<TMP_Text> highscores;
    [SerializeField]
    List<Button> levelButtons;

    private void Start()
    {
        for(int i=0; i<highscores.Count; i++)
        {
            highscores[i].text ="Highscore: " + PlayerPrefs.GetFloat("Highscore" + (i+1)).ToString();
            if(PlayerPrefs.GetFloat("Highscore" + i) == 0 && i != 0)
            {
                levelButtons[i].interactable = false;
            }
            else
            {
                levelButtons[i].interactable = true;
            }
        }
    }
    public void ButtonPress(int level)    
    {
        Debug.Log("load level");
        SceneManager.LoadScene(level);
        GameManager.Instance.ResetTimer();
        GameManager.Instance.currentScene = level;
    }
}