using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingObject : MonoBehaviour
{

    public bool fallOnHit = true;
    public float gravityScale;
    public float bounceForce;
    private bool released = false;
    private float releaseTime;
    private bool dead = false;
    private float cullTime;

    private Rigidbody2D rb;

    public bool animated = false;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (animated)
            animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (released && Time.time >= releaseTime)
        {
            rb.gravityScale = gravityScale;
            released = false;
        }

    }

    private void Update()
    {
        if (dead && Time.time > cullTime)
            Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 11)   // Falling object was hit by the ball
        {
            //rb.gravityScale = gravityScale;
            releaseTime = Time.time + Time.deltaTime * 2;
            released = true;
            if (animated)
                animator.SetBool("Falling", true);
        }
        if (collision.gameObject.tag == "Hazzard")  // If the falling object hits the red blocker below the paddle. This might need to be changed later if we change how that works but is fine until then. 
        {
            Destroy(this.gameObject, 0.2f);
        }
        if (collision.gameObject.layer == 8)    // Falling object hit the paddle, make it bounce off and fly off screen
        {
            collision.gameObject.GetComponent<PaddleControllerV1>().TakeImpairingDamage();
            GameObject attractor = GameObject.Find("Out of Bounds Attractor");
            Vector3 towardsAttractor = Vector3.Normalize(attractor.transform.position - transform.position);
            if (transform.position.x > 0) {
                towardsAttractor = new Vector3(-towardsAttractor.x, towardsAttractor.y, towardsAttractor.z);
            }
            rb.AddForce((towardsAttractor * bounceForce), ForceMode2D.Impulse);
            dead = true;
            cullTime = Time.time + 5f;
        }
    }
}
