using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController2D controller;
    
    bool jump = false;
    float horizontalMove = 0f;

    [SerializeField] ParticleSystem redPole;
    [SerializeField] ParticleSystem bluePole;

    // Update is called once per frame
    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * controller.runSpeed;

        

        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
        }
        if (Input.GetButtonUp("Jump"))
        {
            jump = false;
        }

        if (Input.GetButtonDown("Fire1"))
        {
            ChangePole();
        }
    }

    private void FixedUpdate()
    {
        controller.Move(horizontalMove * Time.fixedDeltaTime, false, jump);
        jump = false;
    }

    public void ChangePole()
    {
        controller.MagnetController.PositiveCharge = !controller.MagnetController.PositiveCharge;
        redPole.gameObject.SetActive(controller.MagnetController.PositiveCharge);
        bluePole.gameObject.SetActive(!controller.MagnetController.PositiveCharge);
    }
}
