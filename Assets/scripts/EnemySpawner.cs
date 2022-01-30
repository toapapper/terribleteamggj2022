using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    [SerializeField] private GameObject prefab;
    [SerializeField] private float cooldown = 10;
    private float timer;
    // Update is called once per frame

        // Start is called before the first frame update
    void Start()
     {
        timer = cooldown;

     }

        // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= cooldown)
        {
            timer = 0;
            GameObject GO = Instantiate<GameObject>(prefab, transform);
            GO.transform.localScale = new Vector3(2, 2, 2);
        }
    }
}
