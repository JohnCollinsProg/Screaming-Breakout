using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject ballObj;
    public GameObject paddleObj;
    private GameObject destructables;

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
}
