using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance { get; private set; }

    private Scene scene;
    public GameObject player;
    public int currentScene = 1;
    float timer;

    public event Action GamePaused;
    public event Action GameUnPaused;
    public event Action<int> GameChangeLevel;

    [SerializeField] AudioSource playerDeathSFX;

    // Start is called before the first frame update
    void Awake()
    {
        Debug.Log("GameManager awake");

        if (Instance != null)
        {
            Debug.Log("SINGLETON - Trying to create another instace of singleton!!");
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }

        scene = SceneManager.GetActiveScene();
        player = FindObjectOfType<CharacterController2D>()?.gameObject;
    }


    private void Update()
    {
        timer += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.R))
        {
            PlayerDeath();
        }
        if (SceneManager.GetActiveScene().name != "MainMenu")
        {
            PauseGame();
            ResumeGame();

        }
    }

    public void PlayerDeath()
    {
        playerDeathSFX.Play();
        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        player.GetComponent<MagnetController>().enabled = false;
        foreach (Collider2D collider in player.GetComponents<Collider2D>())
        {
            collider.enabled = false;
        }

        StartCoroutine(Die(1));
    }

    private IEnumerator Die(float deathDelay)
    {
        yield return new WaitForSeconds(deathDelay);
        SceneManager.LoadScene(currentScene);
        player.GetComponent<PlayerMovement>().enabled = false;
        ResetTimer();
        StartCoroutine(FireChangeLevelEvent(0.05f));
        yield return null;
    }

    public void FinishLevel()
    {
        //Debug.Log("Timer: " + timer);
        if (timer < PlayerPrefs.GetFloat("Highscore" + currentScene) || PlayerPrefs.GetFloat("Highscore" + currentScene) == 0)
        {
            PlayerPrefs.SetFloat("Highscore" + currentScene, (Mathf.Round(timer * 100f) * 0.01f));
        }
        //SaveSystem.SaveTime(timer, currentScene);
        Debug.Log("Save time: " + timer + " Save current level: " + currentScene);
        Debug.Log("Best Playerpref time:" + PlayerPrefs.GetFloat("Highscore" + currentScene));
        ChangeToNextScene();
    }

    public void ChangeToNextScene()
    {
        currentScene++;
        Debug.LogWarning("Currentscene:" + currentScene);
        Debug.LogWarning("SceneCount: " + SceneManager.sceneCountInBuildSettings);
        if (SceneManager.sceneCountInBuildSettings > currentScene)
        {
            SceneManager.LoadScene(currentScene);
        }
        else
        {
            SceneManager.LoadScene(0);
            currentScene = 1;
        }
        ResetTimer();

        StartCoroutine(FireChangeLevelEvent(0.05f));
    }

    public void ChangeToScene(int scene)
    {
        currentScene = scene;
        SceneManager.LoadScene(scene);
        ResetTimer();
        StartCoroutine(FireChangeLevelEvent(0.05f));
    }
    private IEnumerator FireChangeLevelEvent(float delay)
    {
        yield return new WaitForSeconds(delay);
        GameChangeLevel?.Invoke(currentScene);
        yield return null;
    }

    public void ResetTimer()
    {
        timer = 0;
    }

    public float GetTimer()
    {
        return timer;
    }

    void PauseGame()
    {
        if (Time.timeScale == 1)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Time.timeScale = 0;
                GamePaused?.Invoke();
                return;
            }
        }
        if (Time.timeScale == 0)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                GameUnPaused?.Invoke();
                Time.timeScale = 1;
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Time.timeScale = 1;
                SceneManager.LoadScene("MainMenu");
                currentScene = 0;
            }
        }
        //if (Input.GetKeyDown(KeyCode.Escape))
        //{
        //    GamePaused?.Invoke();
        //    Debug.Log("TIME PAUSED");
        //    Time.timeScale = 0;
        //}
    }

    public void ResumeGame()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Time.timeScale = 1;
            GameUnPaused?.Invoke();

        }
    }
}
