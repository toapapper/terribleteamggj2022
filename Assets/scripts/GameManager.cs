using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using TMPro;


public class GameManager : MonoBehaviour
{

    public static GameManager Instance { get; private set; }

    private Scene scene;
    public GameObject player;
    public int currentScene = 1;
    float timer;


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
    }

    public void PlayerDeath()
    {
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
        SceneManager.LoadScene(currentScene);
        ResetTimer();
    }

    public void ResetTimer()
    {
        timer = 0;
    }

    public float GetTimer()
    {
        return timer;
    }
}
