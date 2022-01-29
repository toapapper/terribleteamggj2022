using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectMenu : MonoBehaviour
{
    public void ButtonPress(int level)    
    {
        SceneManager.LoadScene(level);
    }
}