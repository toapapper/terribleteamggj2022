using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinousMovement : MonoBehaviour
{
    [SerializeField] bool movingLeft;
    [SerializeField][Range(0,3f)] float speed;
    Sprite sprite;
    float spriteWidth;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>().sprite;
        spriteWidth = sprite.bounds.size.x * sprite.pixelsPerUnit;
    }

    // Update is called once per frame
    void Update()
    {
        float newPositionX = movingLeft ? transform.position.x + speed * 0.01f : transform.position.x - speed * 0.01f;
        transform.position = new Vector3(newPositionX, transform.position.y, transform.position.z);

        
    }
}
