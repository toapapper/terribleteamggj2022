using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magneta : MonoBehaviour
{
    [SerializeField]SpriteRenderer blue;
    [SerializeField]SpriteRenderer red;

    GameObject player;
    MagnetController playerMagn;

    MagneticObject magn;

    bool positiveCharge = false;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        playerMagn = player.GetComponent<MagnetController>();
        magn = GetComponent<MagneticObject>();

        positiveCharge = playerMagn.PositiveCharge;
    }

    public void UpdateMagnetism()
    {
        if(playerMagn.PositiveCharge && positiveCharge)
        {
            positiveCharge = false;
            red.enabled = false;
            blue.enabled = true;
        }
        else if(!playerMagn.PositiveCharge && !positiveCharge)
        {
            positiveCharge = true;
            red.enabled = true;
            blue.enabled = false;
        }
        magn.PositiveCharge = positiveCharge;
    }
    void Update()
    {
        UpdateMagnetism();
    }
}
