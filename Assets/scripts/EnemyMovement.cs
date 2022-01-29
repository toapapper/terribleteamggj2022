using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private SpriteRenderer sr;

    private Rigidbody2D body;
    [SerializeField]
    float velocity;
    [SerializeField]
    int sideForce;
    [SerializeField]
    int upForce;

    int direction;

    [SerializeField]
    Transform topLeft;
    [SerializeField]
    Transform bottomRight;
    [SerializeField]
    LayerMask groundLayers;

    [SerializeField]
    Transform sideTopLeft;
    [SerializeField]
    Transform sideBottomRight;
    [SerializeField]
    LayerMask wallLayers;

    bool grounded;
    bool touchingWall;
    [SerializeField]
    int maxSpeed = 5;
    int acceleration = 2;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        //moveSpeed = new Vector2(10f, 0);
        //body.AddForce(moveSpeed, ForceMode2D.Force);
        //body.velocity = moveSpeed;
        direction = 1;
    }

    // Update is called once per frame
    void Update()
    {
        //body.velocity = new Vector2(moveSpeed, body.velocity.y);
        //body.AddForce(new Vector2(force, 0), ForceMode2D.Force);

        if(direction >= 1 && !sr.flipX)
        {
            sr.flipX = true;
        }
        else if(direction <= -1 && sr.flipX)
        {
            sr.flipX = false;
        }
    }
    private void FixedUpdate()
    {
        if (body.velocity.x < maxSpeed && body.velocity.x > -maxSpeed)
        {
            if (grounded)
            {
                body.AddForce(new Vector2(velocity, 0), ForceMode2D.Force);

            }
        }
        body.AddForce(new Vector2(sideForce, upForce), ForceMode2D.Force);

        grounded = Physics2D.OverlapArea(topLeft.position, bottomRight.position, groundLayers);

        



        //body.velocity = new Vector2(velocity, body.velocity.y);


        //if (grounded)
        //{
        //    if (velocity < maxSpeed && velocity > -maxSpeed)
        //    {
        //        velocity += maxSpeed * direction * acceleration * Time.deltaTime;
        //        //body.velocity = new Vector2(maxSpeed * direction, body.velocity.y);
        //    }
        //}
        //else
        //{
        //    Debug.Log(body.velocity);
        //    if (body.velocity.x < 1 && body.velocity.x > 1)
        //    {
        //        velocity = 0;
        //    }

        //    if (Mathf.Abs(velocity) > Mathf.Abs(direction * 0.01f))
        //    {
        //        velocity -= direction * acceleration * Time.deltaTime;

        //    }
        //body.velocity = new Vector2(0, body.velocity.y);
        //}
    }

    void TurnAround()
    {
        body.velocity = new Vector2(0, body.velocity.y);
        direction *= -1;
        velocity *= -1;
        //body.velocity = moveSpeed;
        //body.AddForce(moveSpeed, ForceMode2D.Force);
    }

    void TurnTo(float dir)
    {
        body.velocity = new Vector2(0, body.velocity.y);
        velocity *= dir;
        //body.velocity = moveSpeed;
        //body.AddForce(moveSpeed, ForceMode2D.Force);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "EnemyTurnTrigger")
        {
            TurnAround();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
		touchingWall = Physics2D.OverlapArea(sideTopLeft.position, sideBottomRight.position, groundLayers);

        if (touchingWall)
        {
            //float dir = Mathf.Sign(transform.position.x - collision.transform.position.x);
            //TurnTo(dir);
            TurnAround();
        }
        if (collision.collider.tag == "Enemy")
        {
            TurnAround();
        }
        touchingWall = false;
    }
}
