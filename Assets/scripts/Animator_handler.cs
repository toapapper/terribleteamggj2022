using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animator_handler : MonoBehaviour
{
    [SerializeField] Animator blue;
    [SerializeField] Animator red;

    [SerializeField] SpriteRenderer blueSprite;
    [SerializeField] SpriteRenderer redSprite;

    public bool redActive = true;

    private void Start()
    {
        SetActiveColour(redActive);
    }

    public void SetActiveColour(bool red)
    {
        if (red)
        {
            redSprite.enabled = true;
            blueSprite.enabled = false;
        }
        else
        {
            redSprite.enabled = false;
            blueSprite.enabled = true;
        }
    }

    public void SetBool(string id,bool value)
    {
        blue.SetBool(id, value);
        red.SetBool(id, value);
    }

    public void SetTrigger(string id)
    {
        blue.SetTrigger(id);
        red.SetTrigger(id);
    }
}
