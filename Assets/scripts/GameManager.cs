using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    private Scene scene;
    private GameObject player;

    // Start is called before the first frame update
    void Awake()
    {
        scene = SceneManager.GetActiveScene();
        player = FindObjectOfType<CharacterController2D>().gameObject;
    }

	public void PlayerDeath()
	{
        //Deathanimation on player position
        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        player.GetComponent<SpriteRenderer>().enabled = false;
        player.GetComponent<MagnetController>().enabled = false;
        foreach(Collider2D collider in player.GetComponents<Collider2D>())
		{
            collider.enabled = false;
		}

        StartCoroutine(Die(1));
    }

    private IEnumerator Die(int deathDelay)
    {
        yield return new WaitForSeconds(deathDelay);
        SceneManager.LoadScene(scene.name);
        yield return null;
    }
}
