using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    GameObject pauseMenu;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.GamePaused += ShowPauseMenu;
        GameManager.Instance.GameUnPaused += ClosePauseMenu;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowPauseMenu()
    {
        pauseMenu.SetActive(true);
    }

    public void ClosePauseMenu()
    {
        pauseMenu.SetActive(false);
    }

    private void OnDestroy()
    {
        GameManager.Instance.GamePaused -= ShowPauseMenu;
        GameManager.Instance.GameUnPaused -= ClosePauseMenu;
    }
}
