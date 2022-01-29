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
    private GameObject player;
    int currentScene;
    float timer;


    // Start is called before the first frame update
    void Awake()
    {
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
        player = FindObjectOfType<CharacterController2D>().gameObject;

    }

    private void Update()
    {
        timer += Time.deltaTime;
    }

    public void PlayerDeath()
    {
        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        player.GetComponent<SpriteRenderer>().enabled = false;
        player.GetComponent<MagnetController>().enabled = false;
        foreach (Collider2D collider in player.GetComponents<Collider2D>())
        {
            collider.enabled = false;
        }

        StartCoroutine(Die(1));
    }

    private IEnumerator Die(int deathDelay)
    {
        yield return new WaitForSeconds(deathDelay);
        SceneManager.LoadScene(scene.name);
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
        //Debug.Log("Save time: " + timer + " Save current level: " + currentScene);
        //Debug.Log("Best Playerpref time:" + PlayerPrefs.GetFloat("Highscore" + currentScene));
        ChangeToNextScene();
    }

    public void ChangeToNextScene()
    {
        currentScene++;
        SceneManager.LoadScene(currentScene);
        ResetTimer();
    }

    void ResetTimer()
    {
        timer = 0;
    }

    public float GetTimer()
    {
        return timer;
    }
}
