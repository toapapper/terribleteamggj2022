using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController2D controller;
    [SerializeField] AudioSource jumpSFX;
    [SerializeField] AudioSource walkSFX;
    [SerializeField] private AudioSource magnetSFX;
    bool jump = false;
    bool walking = false;
    float horizontalMove = 0f;

    [SerializeField] ParticleSystem redPole;
    [SerializeField] ParticleSystem bluePole;

    // Update is called once per frame
    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * controller.runSpeed;
        if (controller.isGroundedAndMoving)
        {
            walking = true;
        }
        else
        {
            walking = false;
        }

        Debug.Log("walking" + walking);
        if (walking == true)
        {
            if (!walkSFX.isPlaying)
            {
                if (walkSFX.isPlaying == false)
                {
                    walkSFX.Play();
                }
                else
                {
                    walkSFX.Stop();
                }
            }

        }
        else
        {
            walkSFX.Stop();
        }


        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
            jumpSFX.Play();
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
        magnetSFX.Play();
        controller.MagnetController.PositiveCharge = !controller.MagnetController.PositiveCharge;
        redPole.gameObject.SetActive(controller.MagnetController.PositiveCharge);
        bluePole.gameObject.SetActive(!controller.MagnetController.PositiveCharge);
    }
}
