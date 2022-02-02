using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeCounter : MonoBehaviour
{
    [SerializeField]
    TMP_Text timeText;

    private void Update()
    {
        //timeText.text = (Mathf.Round(GameManager.Instance.GetTimer() * 100f) * 0.01f).ToString();
        int minutes  = (int)GameManager.Instance.GetTimer() / 60;
        int seconds  = (int)GameManager.Instance.GetTimer() % 60;
        int fraction  = (int)(GameManager.Instance.GetTimer() * 1000) % 1000;

        timeText.text = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, fraction);
        
    }
}
