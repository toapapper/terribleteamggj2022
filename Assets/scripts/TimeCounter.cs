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
        timeText.text = (Mathf.Round(GameManager.Instance.GetTimer() * 100f) * 0.01f).ToString();
    }
}
