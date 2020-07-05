using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public GameObject paddle;
    public float baseSpeed;
    public float speedIncrement;
    public float maxSpeed;
    public float rotationRate;

    // 0 = follow cursor, 1 =  bouncing
    private int mode;
    private Vector3 defaultPos;
    private Vector3 basePos;
    private float rotation;
    private int rotationDirection = -1;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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
        }
        if (mode == 0)  // Initially the ball is attached to the paddle, until left mouse is clicked. 
        {
            transform.position = basePos + new Vector3(paddle.transform.position.x, 0f, 0f);    // I am using this instead of parenting because it is easier to detach and preserves the scaling. 
            rotation += rotationRate * 1.2f * Time.deltaTime;
            transform.rotation = Quaternion.Euler(0f, 0f, rotation);
        }

        if (mode == 1)  // Ball is released and bouncing around normally
        {
            rotation += rotationRate * Time.deltaTime;
            transform.rotation = Quaternion.Euler(0f, 0f, rotation);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Hazzard")  // Ball has hit the board below the paddle, return to paddle
        {
            mode = 0;
            // Tell game controller to deduct a life and make a really awful sound
        }
    }
}
