using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public GameObject paddle;
    public float baseSpeed;
    public float speedIncrement;
    public float maxSpeed;

    // 0 = follow cursor, 1 =  bouncing
    private int mode;
    private Vector3 defaultPos;
    private Vector3 basePos;

    void Start()
    {
        defaultPos = transform.position;
        basePos = new Vector3(0f, defaultPos.y, defaultPos.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (mode == 0 && Input.GetMouseButtonDown(0))   // Release the ball from the paddle
        {
            mode = 1;
            
        }
        if (mode == 0)
        {
            transform.position = basePos + new Vector3(paddle.transform.position.x, 0f, 0f);
        }
    }
}
