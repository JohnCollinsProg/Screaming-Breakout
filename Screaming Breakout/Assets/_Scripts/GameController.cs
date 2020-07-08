using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject ballObj;
    private BallController ballCont;
    public GameObject paddleObj;
    public PaddleControllerV1 paddleCont;
    private GameObject destructables;
    public GameObject bossObj;
    private BossBehaviour bossBehv;

    public int startingLives;
    public float lightThreshold, mediumThreshold, heavyThreshold;

    public AudioSource[] screamsLight;
    public AudioSource[] screamsMedium;
    public AudioSource[] screamsHeavy;
    public AudioSource deathSound;

    private int mode = 0;
    private int stage = 0;
    private int totalStages;
    private int lives;
    private int remainingBlocks;
    private int totalBlocks;        // The number of blocks at the start of the game. 
    private float checkBlocksTime;
    private bool checkBlocks = false;
    private bool moveStage = true;
    private float stageDistance = -10f;
    private float timeToChangeStage = 4.5f;
    //private float stageChangeStartTime;
    private float stageChangeProgress;
    private float stageChangeCompleteTime;
    private Vector3 nextStagePos;
    private Vector3 stageChangeVector;

    void Start()
    {
        lives = startingLives;
        destructables = GameObject.Find("Destructables");
        totalStages = destructables.transform.childCount;
        remainingBlocks = GetRemainingBlocks();
        print("init: get remainingBlocks: " + remainingBlocks);
        totalBlocks = remainingBlocks;

        ballCont = ballObj.GetComponent<BallController>();
        paddleCont = paddleObj.GetComponent<PaddleControllerV1>();
        bossBehv = bossObj.GetComponent<BossBehaviour>();

        stageChangeVector = new Vector3(0f, stageDistance, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (checkBlocks && Time.time >= checkBlocksTime)
        {
            remainingBlocks = GetRemainingBlocks();
            checkBlocks = false;
        }
        if (remainingBlocks == 0)   // All the blocks have been destroyed, progress onto the next stage of the game. 
        {
            if (stage + 1 < totalStages)
            {
                stage++;
                print("Stage " + stage + " completed, moving onto stage " + (stage + 1));
                moveStage = true;
                //stageChangeStartTime = Time.time;
                stageChangeCompleteTime = Time.time + timeToChangeStage;
                stageChangeProgress = 0f;
                nextStagePos = destructables.transform.position + stageChangeVector;
            }
        }
        if (moveStage)              // The stage is moving to set up the next stage
        {
            stageChangeProgress += Time.deltaTime;
            destructables.transform.position = Vector3.Lerp(destructables.transform.position, nextStagePos, (stageChangeProgress / stageChangeCompleteTime));
            if (destructables.transform.position.y <= nextStagePos.y)   // The next stage setup is complete. 
            {
                destructables.transform.position = nextStagePos;
                moveStage = false;
            }
        }

        if (lives <= 0)
        {
            print("You're out of lives you big sodding idiot!!");
        }
        
        if (Input.GetKey(KeyCode.G))
        {
            //print("BallObj has this many children: " + ballObj.transform.childCount);
            remainingBlocks = GetRemainingBlocks();
            print("Remainging blocks: " + remainingBlocks + ",    stage: " + stage + ",    totalStages: " + totalStages);
        }
    }

    private int GetRemainingBlocks()
    {
        int count = 0;
        //print("Checking remaining blocks, stage = " + stage);
        //string stageName = "Stage " + stage.ToString();
        GameObject stageObj = destructables.transform.GetChild(stage).gameObject;
        int range = stageObj.transform.childCount;
        //print("Destructables has: " + range + " children.");
        if (range > 0) { 
            for (int i = 0; i < range; i++)
            {
                //print(destructables.transform.GetChild(i).gameObject.name);
                if (stageObj.transform.GetChild(i).gameObject.tag == "Point Block")
                {
                    count++;
                }
            }
        }
        return count;
    }


    public void HitHazzard()
    {
        // Somehow the death sound is sometimes played when the ball hits the paddle (even though it doesn't enter this function)
        if (deathSound != null)
            deathSound.Play();
        lives--;
    }

    public void HitBlock(GameObject blockObj)
    {
        Vector3 blockPos = blockObj.transform.position; // Save this location so it can center destruction effect
        BlockHealth blockHealth = blockObj.GetComponent<BlockHealth>(); // I don't like this, but I'm not sure how else to implement it. 
        blockHealth.TakeDamage();
        if (blockHealth.GetHealth() <= 0)
            Destroy(blockObj);
        remainingBlocks = GetRemainingBlocks();
        checkBlocksTime = Time.time + 0.01f;
        checkBlocks = true;

        // Play a blood splatter or puff of smoke.
        PlayLighScream();
    }

    public void HitPaddle() 
    {
        PlayVariableScream(ballCont.GetSpeed());
        paddleCont.PlayHurtAnimation();
    }

    public void HitWall()
    {
        PlayLighScream();
    }

    public void HitBoss()
    {
        bossBehv.TakeDamage();
        // play a sound? Or maybe the boss should do this depending on its health. 
    }

    private void PlayVariableScream(float speed) 
    {
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

    private void PlayLighScream() {
        AudioSource toPlay = screamsLight[Random.Range(0, screamsLight.Length)];    // for now just play light scream  
        if (toPlay != null)
        {
            toPlay.Play();
        }
    }
}
