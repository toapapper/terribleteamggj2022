using UnityEngine;
using System.Collections;

// For usage apply the script directly to the element you wish to apply parallaxing
// Based on Brackeys 2D parallaxing script http://brackeys.com/
public class Parallax : MonoBehaviour
{
    public Transform cam; // Camera reference (of its transform)
    Sprite sprite;
    float textureUnitSizeX;
    Vector3 previousCamPos;

    [SerializeField][Range(0,1)] float parallaxEffectMultiplierX = 1;
    [SerializeField][Range(0, 1)] float parallaxEffectMultiplierY = 1;

    void Awake()
    {
        cam = Camera.main.transform;
        sprite = GetComponent<SpriteRenderer>().sprite;
        textureUnitSizeX = sprite.texture.width / sprite.pixelsPerUnit;
        previousCamPos = cam.position;
    }

    void LateUpdate()
    {
        Vector3 deltaMovement = cam.position - previousCamPos;
        transform.position += new Vector3(deltaMovement.x * parallaxEffectMultiplierX, deltaMovement.y * parallaxEffectMultiplierY);
        previousCamPos = cam.position;

        if (cam.position.x - transform.position.x >= textureUnitSizeX)
        {
            float offSetPositionX = (cam.position.x - transform.position.x) % textureUnitSizeX;
            transform.position = new Vector3(cam.position.x + offSetPositionX, transform.position.y);
        }
    }
}