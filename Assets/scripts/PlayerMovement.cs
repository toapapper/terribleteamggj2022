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

    private Animator_handler animator;

    private void Start()
    {
        animator = GetComponent<Animator_handler>();
        GameManager.Instance.player = gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * controller.runSpeed;
        if (controller.isGroundedAndMoving)
        {
            walking = true;
            animator.SetBool("walking", true);
        }
        else
        {
            walking = false;
            animator.SetBool("walking", false);
        }
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

        if(controller.isGrounded && jump == true)
        {
            jumpSFX.Play();
        }

        if (Input.GetButtonDown("Jump"))
        {
            jump = true;

        }
        if (Input.GetButtonUp("Jump"))
        {
            jump = false;
            animator.SetBool("jump", false);
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

        animator.SetTrigger("attack");
        animator.SetActiveColour(controller.MagnetController.PositiveCharge);
    }
}
