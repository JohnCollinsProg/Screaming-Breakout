using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject ballObj;
    public GameObject paddleObj;

    public int startingLives;

    private int mode = 0;
    private int lives;

    void Start()
    {
        lives = startingLives;
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
}
