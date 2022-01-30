using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections;

public class CharacterController2D : MonoBehaviour
{

    [SerializeField] public float runSpeed = 10f;
    [SerializeField] private float m_JumpForce = 400f;                          // Amount of force added when the player starts jumping.
    [SerializeField] private float m_AdditionalJumpForce = 100f;                // Amount of force added during the jump if holding jump key.
    [SerializeField] [Range(0, 0.5f)] float jumpLimitTime = 0.2f;
    [Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f;          // Amount of maxSpeed applied to crouching movement. 1 = 100%
    [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;  // How much to smooth out the movement
    [SerializeField] private bool m_AirControl = false;                         // Whether or not a player can steer while jumping;
    [SerializeField] [Range(0,1)] float airControlAccelleration = 0.2f;
    [SerializeField] private LayerMask m_WhatIsGround;                          // A mask determining what is ground to the character
    [SerializeField] private Transform m_GroundCheck;                           // A position marking where to check if the player is grounded.
    [SerializeField] private Transform m_CeilingCheck;                          // A position marking where to check for ceilings
    [SerializeField] private Collider2D m_CrouchDisableCollider;                // A collider that will be disabled when crouching

    const float k_GroundedRadius = .5f; // Radius of the overlap circle to determine if grounded
    private bool m_Grounded;            // Whether or not the player is grounded.
    const float k_CeilingRadius = .2f; // Radius of the overlap circle to determine if the player can stand up
    private Rigidbody2D m_Rigidbody2D;
    private bool m_FacingRight = true;  // For determining which way the player is currently facing.
    private Vector3 m_Velocity = Vector3.zero;

    bool jumpLimitReached = true;
    bool currentlyJumping = false;
    float jumpTimeCounter = 0;

    private Animator_handler m_animator;

    public bool isGroundedAndMoving
    {
        get { return m_Grounded && Input.GetButton("Horizontal"); }
    }

    public bool isGrounded
    {
        get { return m_Grounded; }
    }

    private MagnetController magnetController;

    [Header("Events")][Space] public UnityEvent OnLandEvent;

    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }

    public BoolEvent OnCrouchEvent;
    private bool m_wasCrouching = false;

    public MagnetController MagnetController { get { return magnetController; } }

    private void Awake()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        magnetController = GetComponent<MagnetController>();

        m_animator = GetComponent<Animator_handler>();

        if (OnLandEvent == null)
            OnLandEvent = new UnityEvent();

        if (OnCrouchEvent == null)
            OnCrouchEvent = new BoolEvent();
    }


    private void FixedUpdate()
    {
        bool wasGrounded = m_Grounded;
        m_Grounded = false;

        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        // This can be done using layers instead but Sample Assets will not overwrite your project settings.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                m_Grounded = true;
                if (!wasGrounded)
                    OnLandEvent.Invoke();
            }
        }

        if (m_Grounded)
        {
            m_animator.SetBool("in_air", false);
        }
        else
        {
            m_animator.SetBool("in_air", true);
        }

    }

    public void Move(float move, bool crouch, bool jump)
    {
        // If crouching, check to see if the character can stand up
        if (!crouch)
        {
            // If the character has a ceiling preventing them from standing up, keep them crouching
            if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
            {
                crouch = true;
            }
        }

        //only control the player if grounded or air movement
        if (m_Grounded || m_AirControl)
        {
            // If crouching
            if (crouch)
            {
                if (!m_wasCrouching)
                {
                    m_wasCrouching = true;
                    OnCrouchEvent.Invoke(true);
                }

                // Reduce the speed by the crouchSpeed multiplier
                move *= m_CrouchSpeed;

                // Disable one of the colliders when crouching
                if (m_CrouchDisableCollider != null)
                    m_CrouchDisableCollider.enabled = false;
            }
            else
            {
                // Enable the collider when not crouching
                if (m_CrouchDisableCollider != null)
                    m_CrouchDisableCollider.enabled = true;

                if (m_wasCrouching)
                {
                    m_wasCrouching = false;
                    OnCrouchEvent.Invoke(false);
                }
            }

            // Move the character by finding the target velocity
            Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);

            if (!m_Grounded)
            {
                targetVelocity.x *= airControlAccelleration;
            }

            // And then smoothing it out and applying it to the character
            m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

            // If the input is moving the player right and the player is facing left...
            if (move > 0 && !m_FacingRight)
            {
                // ... flip the player.
                Flip();
            }
            // Otherwise if the input is moving the player left and the player is facing right...
            else if (move < 0 && m_FacingRight)
            {
                // ... flip the player.
                Flip();
            }
        }

        // If the player should jump...
        if (m_Grounded && jump && !currentlyJumping) 
        {
            // Add a vertical force to the player.
            m_Grounded = false;
            currentlyJumping = true;
            jumpLimitReached = false;
            jumpTimeCounter = 0;
            m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
            jump = false;
        }
        // If the player is holding jump
        else if (currentlyJumping && !jumpLimitReached)
        {
            if (!Input.GetButton("Jump"))
            {
                currentlyJumping = false;
                jumpLimitReached = true;
                m_animator.SetBool("jump", false);
            }
            jumpTimeCounter += Time.deltaTime;
            if (currentlyJumping && jumpTimeCounter >= jumpLimitTime)
            {
                jumpLimitReached = true;
                jumpTimeCounter = 0;
                jump = false;
                currentlyJumping = false;
                m_animator.SetBool("jump", false);
            }
            else
            {
                m_Rigidbody2D.AddForce(new Vector2(0f, m_AdditionalJumpForce));
                m_animator.SetBool("jump", true);
            }
        }
    }

    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        m_FacingRight = !m_FacingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    #region Colliders
    /// <summary>
    /// Adds repelling force to enemy on collision
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            m_animator.SetTrigger("death");
            GameManager.Instance.PlayerDeath();
        }
        if (collision.gameObject.tag == "FinishTrigger")
        {
            GameManager.Instance.FinishLevel();
        }
    }
    #endregion
}