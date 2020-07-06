using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject ballObj;
    public GameObject paddleObj;
    private GameObject destructables;

    public AudioSource[] screamsLight;
    public AudioSource[] screamsMedium;
    public AudioSource[] screamsHeavy;

    public int startingLives;

    private int mode = 0;
    private int lives;
    private int remainingBlocks;

    void Start()
    {
        lives = startingLives;
        destructables = GameObject.Find("Destructables");
        
    }

    // Update is called once per frame
    void Update()
    {
        if (lives <= 0)
        {
            print("You're out of lives you big sodding idiot!!");
        }
    }

    public void HitHazzard()
    {
        lives--;
    }

    private int GetRemainingBlocks()
    {
        int count = 0;
        //foreach (GameObject obj in destructables.transform.get)
        //{

        //}

        return count;
    }

    public void collisionFX(Collision2D collision) {
        if (collision.gameObject.tag == "Hazzard")  // Ball has hit the board below the paddle
        {

        }
        else if (collision.gameObject.layer == 8)   // Ball hit the paddle
        {
            Vector3 velo = ballObj.GetComponent<Rigidbody2D>().velocity;
            float speed = Mathf.Sqrt(Mathf.Pow(velo.x, 2) + Mathf.Pow(velo.y, 2));      // Calculate the current speed of the ball

            if (speed > 13f) {   // Play scream type based on ball speed
                AudioSource toPlay = screamsHeavy[Random.Range(0, screamsHeavy.Length)];    // Choose random scream from array
                if (toPlay != null) {
                    toPlay.Play();
                }
            } else if (speed > 10f) {
                AudioSource toPlay = screamsMedium[Random.Range(0, screamsMedium.Length)];
                if (toPlay != null) {
                    toPlay.Play();
                }
            } else if (speed > 5f) {   
                AudioSource toPlay = screamsLight[Random.Range(0, screamsLight.Length)];     
                if (toPlay != null) {
                    toPlay.Play();
                }
            } 
        }
        else if (collision.gameObject.layer == 9)    // Ball hit the side or back walls
        {
            AudioSource toPlay = screamsLight[Random.Range(0, screamsLight.Length)];    // for now just play light scream  
            if (toPlay != null) {                     
                toPlay.Play();
            }
        }
        else if (collision.gameObject.layer == 10)   // Ball hit a block
        {
            AudioSource toPlay = screamsLight[Random.Range(0, screamsLight.Length)];   // for now just play light scream  
            if (toPlay != null) {
                toPlay.Play();
            }
        } 
    }
}
