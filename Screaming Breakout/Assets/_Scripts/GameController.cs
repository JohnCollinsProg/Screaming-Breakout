using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject ballObj;
    private BallController ballCont;
    public GameObject paddleObj;
    private GameObject destructables;

    public int startingLives;
    public float lightThreshold, mediumThreshold, heavyThreshold;

    public AudioSource[] screamsLight;
    public AudioSource[] screamsMedium;
    public AudioSource[] screamsHeavy;

    

    private int mode = 0;
    private int lives;
    private int remainingBlocks;
    private int totalBlocks;        // The number of blocks at the start of the game. 
    private float checkBlocksTime;
    private bool checkBlocks = false;

    void Start()
    {
        lives = startingLives;
        destructables = GameObject.Find("Destructables");
        remainingBlocks = GetRemainingBlocks();
        totalBlocks = remainingBlocks;

        ballCont = ballObj.GetComponent<BallController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (checkBlocks && Time.time >= checkBlocksTime)
        {
            remainingBlocks = GetRemainingBlocks();
            checkBlocks = false;
        }
        if (lives <= 0)
        {
            print("You're out of lives you big sodding idiot!!");
        }
        if (remainingBlocks == 0)
        {
            print("You have destroyed all the blocks, congratulations!");
        }
        if (Input.GetKey(KeyCode.G))
        {
            //print("BallObj has this many children: " + ballObj.transform.childCount);
            remainingBlocks = GetRemainingBlocks();
            //print("Remainging blocks: " + remainingBlocks);
        }
    }

    private int GetRemainingBlocks()
    {
        int count = 0;
        int range  = destructables.transform.childCount;
        //print("Destructables has: " + range + " children.");
        if (range > 0) { 
            for (int i = 0; i < range; i++)
            {
                //print(destructables.transform.GetChild(i).gameObject.name);
                if (destructables.transform.GetChild(i).gameObject.tag == "Point Block")
                {
                    count++;
                }
            }
        }
        return count;
    }


    public void HitHazzard()
    {
        lives--;
    }

    public void HitBlock(GameObject blockObj)
    {
        Vector3 blockPos = blockObj.transform.position; // Save this location so it can center destruction effect
        Destroy(blockObj);
        remainingBlocks = GetRemainingBlocks();
        checkBlocksTime = Time.time + 0.01f;
        checkBlocks = true;

        // Play a blood splatter or puff of smoke. 

        float speed = ballCont.GetSpeed();
        if (speed >= heavyThreshold)
        {
            AudioSource toPlay = screamsHeavy[Random.Range(0, screamsHeavy.Length)];    // Choose random scream from array
            if (toPlay != null)
            {
                toPlay.Play();
            }
        } else if (speed > mediumThreshold)
        {
            AudioSource toPlay = screamsMedium[Random.Range(0, screamsMedium.Length)];
            if (toPlay != null)
            {
                toPlay.Play();
            }
        }
        else if (speed > lightThreshold)
        {
            AudioSource toPlay = screamsLight[Random.Range(0, screamsLight.Length)];
            if (toPlay != null)
            {
                toPlay.Play();
            }
        }
    }

    public void HitWall()
    {
        AudioSource toPlay = screamsLight[Random.Range(0, screamsLight.Length)];    // for now just play light scream  
        if (toPlay != null)
        {
            toPlay.Play();
        }
    }

    // Matt's original code for sound effects and wall and ball collisions
    /*public void collisionFX(Collision2D collision) {
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
    }*/
}
