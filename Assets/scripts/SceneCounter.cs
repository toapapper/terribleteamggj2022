using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SceneCounter : MonoBehaviour
{
    [SerializeField]
    TMP_Text levelText;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.GameChangeLevel += UpdateLevelCounter;
    }

    void UpdateLevelCounter(int currentLevel)
    {
        levelText.text = "Level " + currentLevel.ToString();
    }
}
