using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magneta : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private float cooldown = 10;
    private float timer;
    // Update is called once per frame

    private void Start()
    {
        timer = cooldown;
    }
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= cooldown)
        {
            timer = 0;
            GameObject GO = Instantiate<GameObject>(prefab,transform);
            GO.transform.localScale = new Vector3(2, 2, 2);
        }
    }
}
