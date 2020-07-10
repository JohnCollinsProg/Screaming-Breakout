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
    private float minSpeed;
    private float rotation;
    private int rotationDirection = -1;

    private Rigidbody2D rb;
    private GameObject gContObj;
    private GameController gCont;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gContObj = GameObject.Find("Game Controller");
        if (gContObj != null)
            gCont = gContObj.GetComponent<GameController>();
        defaultPos = transform.position;
        basePos = new Vector3(0f, defaultPos.y, defaultPos.z);
        mode = 0;
    }

    public void StartModeOne()
    {
        mode = 1;
    }

    // Update is called once per frame
    void Update()
    {
        /* 
            mode 0 == ball on paddle
            mode 1 == ball releasing from paddle
            mode 2 == ball bouncing normally
        */
        if (mode == 0 && Input.GetMouseButtonDown(0))   // Release the ball from the paddle
        {
            mode = 1;
            // Give the ball a velocity, with direction based on the position of the paddle relative to screen center. 
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float xVelo = transform.position.x / 2f;
            rb.velocity = new Vector2(xVelo, baseSpeed);
            minSpeed = baseSpeed;
        }
        if (mode == 0)  // Initially the ball is attached to the paddle, until left mouse is clicked. 
        {
            if (paddle != null) 
                transform.position = basePos + new Vector3(paddle.transform.position.x, Camera.main.transform.position.y, 0f);    // I am using this instead of parenting because it is easier to detach and preserves the scaling. 
            rotation += rotationRate * rotationDirection * 1.2f * Time.deltaTime;
            transform.rotation = Quaternion.Euler(0f, 0f, rotation);
        }

        if (mode == 1 || mode == 2)  // Ball is releasing from paddle or bouncing around normally
        {
            rotation += rotationRate * rotationDirection * Time.deltaTime;
            transform.rotation = Quaternion.Euler(0f, 0f, rotation);
            if (mode == 1)
                mode = 2;
            if (rb.velocity.magnitude > minSpeed)
                minSpeed = rb.velocity.magnitude;
            if (rb.velocity.magnitude < minSpeed)
            {
                rb.velocity = rb.velocity.normalized * minSpeed;
            }
        }

        if (Input.GetKeyDown(KeyCode.G))    // To be pressed when the ball gets stuck. 
        {
            mode = 0;
            gCont.HitHazzard();
            gCont.BallReset();
        }
        if ((Input.GetMouseButtonDown(0) && Input.GetMouseButtonDown(1)) || (Input.GetMouseButtonDown(0) && Input.GetMouseButton(1)) || (Input.GetMouseButton(0) && Input.GetMouseButtonDown(1))){
            mode = 0;
            gCont.HitHazzard();
        }
    }

    private void FixedUpdate()
    {
        lastVelocity = rb.velocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Hazzard")  // Ball has hit the board below the paddle, return to paddle
        {
            mode = 0;
            gCont.HitHazzard(); // Tell the game controller that the ball has hit the wall under the paddle
        }
        else if (collision.gameObject.layer == 9 || collision.gameObject.layer == 10 || collision.gameObject.layer == 12)   // Ball hit either the side or back walls,  or a block, or boss
        {
            float speed = lastVelocity.magnitude;
            Vector3 direction = Vector3.Reflect(lastVelocity.normalized, collision.GetContact(0).normal);
            rb.velocity = direction * Mathf.Max(speed);
            rotationDirection *= -1;// Reverse direction of ball rotation
            gCont.Bounce();
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
            if (mode == 2) {
                gCont.HitPaddle();
            }
        }

        if (collision.gameObject.layer == 10)   // Ball hit a block, tell game controller to score
        {
            gCont.HitBlock(collision.gameObject);
        }

        if (collision.gameObject.layer == 9)    // Ball hit the walls/ roof
        {
            if (gCont != null)
                gCont.HitWall(collision.gameObject.name);
        }

        if (collision.gameObject.layer == 12)   // Ball hit the boss
        {
            gCont.HitBoss(collision.GetContact(0).point);
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
