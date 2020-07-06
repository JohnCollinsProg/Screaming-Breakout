using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public GameObject paddle;
    public float baseSpeed;
    public float speedIncrement;
    public float maxSpeed = 10f;
    public float rotationRate;
    public float maxPaddleAngle = 70f;

    // 0 = follow cursor, 1 =  bouncing
    private int mode;
    private Vector3 defaultPos;
    private Vector3 basePos;
    private Vector3 lastVelocity;
    private float rotation;
    private int rotationDirection = -1;

    private Rigidbody2D rb;
    private GameController gCont;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gCont = GameObject.Find("Game Controller").GetComponent<GameController>();
        defaultPos = transform.position;
        basePos = new Vector3(0f, defaultPos.y, defaultPos.z);
        mode = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (mode == 0 && Input.GetMouseButtonDown(0))   // Release the ball from the paddle
        {
            mode = 1;
            // Give the ball a velocity, with direction based on the position of the paddle relative to screen center. 
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float xVelo = transform.position.x / 2f;
            rb.velocity = new Vector2(xVelo, baseSpeed);
        }
        if (mode == 0)  // Initially the ball is attached to the paddle, until left mouse is clicked. 
        {
            transform.position = basePos + new Vector3(paddle.transform.position.x, 0f, 0f);    // I am using this instead of parenting because it is easier to detach and preserves the scaling. 
            rotation += rotationRate * rotationDirection * 1.2f * Time.deltaTime;
            transform.rotation = Quaternion.Euler(0f, 0f, rotation);
        }

        if (mode == 1)  // Ball is released and bouncing around normally
        {
            rotation += rotationRate * rotationDirection * Time.deltaTime;
            transform.rotation = Quaternion.Euler(0f, 0f, rotation);
        }
        lastVelocity = rb.velocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Hazzard")  // Ball has hit the board below the paddle, return to paddle
        {
            mode = 0;
            gCont.HitHazzard(); // Tell the game controller that the ball has hit the wall under the paddle
        }
        else if (collision.gameObject.layer == 9 || collision.gameObject.layer == 10)   // Ball hit either the side or back walls,  or a block
        {
            float speed = lastVelocity.magnitude;
            Vector3 direction = Vector3.Reflect(lastVelocity.normalized, collision.GetContact(0).normal);
            rb.velocity = direction * Mathf.Max(speed);
            rotationDirection *= -1;// Reverse direction of ball rotation
        }
        if (collision.gameObject.layer == 8)    // Ball hit the paddle
        {
            float speed = lastVelocity.magnitude * speedIncrement;
            float xDifference = paddle.transform.position.x - transform.position.x;
            // float outAngle = maxPaddleAngle * (xDifference / 4) * Mathf.Deg2Rad;
            // Vector3 outAngleV = new Vector3(0f, 0f, outAngle);

            // Vector3 difference = paddle.transform.position - transform.position;
            // Vector3 direction = Vector3.Reflect(lastVelocity.normalized, difference);
            // rb.velocity = speed * direction;

            float outAngle = xDifference / 2;      // Scale from -1 to 1 of where the ball hit the paddle
            // Dunno what the z value should be for the vector
            Vector3 direction = new Vector3(-Mathf.Sin(outAngle), Mathf.Cos(outAngle), 0);      // Calculate vector for new ball direction
            rb.velocity = speed * direction;
            checkSpeedLimit();

            rotationDirection *= -1;
        }

        if (collision.gameObject.layer == 10)   // Ball hit a block, tell game controller to score
        {
            gCont.HitBlock(collision.gameObject);
        }

        if (collision.gameObject.layer == 9)
        {
            gCont.HitWall();
        }
        //gCont.collisionFX(collision);
    }

    private void checkSpeedLimit()
    {
        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
    }

    public float GetSpeed()
    {
        return rb.velocity.magnitude;
    }
}
