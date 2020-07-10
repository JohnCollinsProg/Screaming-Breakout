using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject ballObj;
    private BallController ballCont;
    public GameObject paddleObj;
    public PaddleControllerV1 paddleCont;
    private GameObject destructables;
    public GameObject bossObj;
    private BossBehaviour bossBehv;
    private bool bossDead = false;
    private bool roofOpen = false;

    public bool useLives;
    public int startingLives;
    private int lives;
    public float lightThreshold, mediumThreshold, heavyThreshold;
    private float gameoverTime;
    public float deathPeriod = 5f;

    public AudioSource[] screamsLight;
    public AudioSource[] screamsMedium;
    public AudioSource[] screamsHeavy;
    public AudioSource deathSound;

    private int mode = 0;
    // Stage = 0 - level 1, stage 1 - level 2, stage 3 - boss
    private int stage = 0;
    private int totalStages;
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

    //private GameObject roofObj;
    private RoofBreaker roofBreaker;
    private float camMax = 10.71f;
    public float postGameTime;
    private float nextSceneTime;
    private bool nextSceneTimer = false;

    private GameObject dataObj;
    private StaticData staticData;
    public GameObject dataObjPrefab;

    public GameObject uiLivesObj;
    private UILivesController uiLivesCont;
    public GameObject uiScoreObj;
    private UIScoreController uiScoreCont;

    public GameObject uiHealthBarObj;
    private BossHealthBarController uiHealthBarCont;         

    void Start()
    {
        lives = startingLives;
        destructables = GameObject.Find("Destructables");
        totalStages = destructables.transform.childCount - 1; // -1 so that it is in index form. 
        remainingBlocks = GetRemainingBlocks();
        print("init: get remainingBlocks: " + remainingBlocks);
        totalBlocks = remainingBlocks;

        ballCont = ballObj.GetComponent<BallController>();
        paddleCont = paddleObj.GetComponent<PaddleControllerV1>();
        bossBehv = bossObj.GetComponent<BossBehaviour>();
        uiLivesCont = uiLivesObj.GetComponent<UILivesController>();
        uiLivesCont.SetLives(startingLives);
        uiScoreCont = uiScoreObj.GetComponent<UIScoreController>();

        uiHealthBarCont = uiHealthBarObj.GetComponent<BossHealthBarController>();
        uiHealthBarCont.SetMaxHealth(bossBehv.maxHealth);

        stageChangeVector = new Vector3(0f, stageDistance, 0f);

        roofBreaker = GameObject.Find("Exploding Roof").GetComponent<RoofBreaker>();

        dataObj = GameObject.Find("Data");
        if (dataObj != null)
        {
            staticData = dataObj.GetComponent<StaticData>();
            print("Found staticData, ballOnTitle: " + staticData.GetBallOnTitle());
        }
        else
        {
            print("Could not find static data. Making a new one.");
            dataObj = Instantiate(dataObjPrefab, Vector3.zero, Quaternion.identity);
            staticData = dataObj.GetComponent<StaticData>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (checkBlocks && Time.time >= checkBlocksTime)
        {
            remainingBlocks = GetRemainingBlocks();
            checkBlocks = false;
        }
        if (stage == totalStages)
        {
            //remainingBlocks = -1;
            stage++;
            bossBehv.SpawnBoss();
        }
        if (remainingBlocks == 0)   // All the blocks have been destroyed, progress onto the next stage of the game. 
        {
            
            if (stage < totalStages)    // Progress to the next stage
            {
                stage++;
                remainingBlocks = GetRemainingBlocks();
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

        if (roofOpen && ballObj.transform.position.y > Camera.main.transform.position.y)    // The camera is free and will pan upwards to follow the ball outwards
        {
            float newYPos = Mathf.Clamp(Mathf.Lerp(Camera.main.transform.position.y, ballObj.transform.position.y, 0.6f * Time.deltaTime), Camera.main.transform.position.y, camMax);
            Camera.main.transform.position = new Vector3(0f, newYPos, -10f);
        }
        if (roofOpen && Camera.main.transform.position.y + 0.05f >= camMax && !nextSceneTimer)
        {
            nextSceneTimer = true;
            nextSceneTime = Time.time + postGameTime;
            staticData.AllowBallOnTitle();
            print("Beginning transition to return to title");
            paddleCont.PlayWinAnimation();
            uiScoreCont.GameWon();
        }
        if (nextSceneTimer && Time.time >= nextSceneTime)
        {
            print("Post game over, returning to title");

            // load main menu with the ball
            staticData.AllowBallOnTitle();
            SceneManager.LoadScene("TitleScene");
            //StartCoroutine(LoadYourAsyncScene());
        }

        if (lives <= 0 && useLives && !bossDead && gameoverTime == 0)
        {
            print("You're out of lives you big sodding idiot!!");
            paddleCont.SetDead();
            gameoverTime = Time.time + deathPeriod;
            //SceneManager.LoadScene("GameOverScene");
        }
        if (lives <= 0 && useLives && !bossDead && Time.time >= gameoverTime)
            SceneManager.LoadScene("GameOverScene");

        if (Input.GetKey(KeyCode.G))
        {
            //print("BallObj has this many children: " + ballObj.transform.childCount);
            print("Stage: " + stage);
            remainingBlocks = GetRemainingBlocks();
            print("Remainging blocks: " + remainingBlocks + ",    stage: " + stage + ",    totalStages: " + totalStages);
        }
    }

    /*IEnumerator LoadYourAsyncScene()
    {
        // The Application loads the Scene in the background as the current Scene runs.
        // This is particularly good for creating loading screens.
        // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
        // a sceneBuildIndex of 1 as shown in Build Settings.

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("TitleScene");

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }*/

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
        if (lives > 0) {
            uiLivesCont.SetLives(lives);
        }
        uiScoreCont.Die();
    }

    public void HitBlock(GameObject blockObj)
    {
        Vector3 blockPos = blockObj.transform.position; // Save this location so it can center destruction effect
        BlockHealth blockHealth = blockObj.GetComponent<BlockHealth>(); // I don't like this, but I'm not sure how else to implement it. 
        blockHealth.TakeDamage();
        uiScoreCont.BlockHit();
        if (blockHealth.GetHealth() <= 0)
        {
            if (blockHealth.shrinker) {
                PlayHeavyScream();
                uiScoreCont.BigBlockDeath();
            }
            blockHealth.BlockDeath();
            Destroy(blockObj);
        }
        if (stage < totalStages)
        {
            remainingBlocks = GetRemainingBlocks();
            checkBlocksTime = Time.time + 0.01f;
            checkBlocks = true;
        }

        // Play a blood splatter or puff of smoke.
        if (blockHealth.shrinker) {
            PlayMediumScream();
        } else {
            PlayLightScream();
        }
    }

    public void HitPaddle() 
    {
        PlayVariableScream(ballCont.GetSpeed());
        uiScoreCont.HitPaddle();
    }

    public void HitWall(string name)
    {
        PlayLightScream();
        if (name == "Roof" && bossDead)
        {
            // Break the roof open
            roofBreaker.BreakOpen();
            roofOpen = true;
            Destroy(GameObject.Find("Roof"));
        }
    }

    public void HitBoss(Vector2 point)
    {
        bossBehv.TakeDamage(point);
        uiScoreCont.BossHit();
        uiHealthBarCont.TakeDamage();
        // play a sound? Or maybe the boss should do this depending on its health. 
    }

    public void BossBattleStart() {
        uiHealthBarCont.BossBattleStart();
    }

    public void SetBossHealth() {
        uiHealthBarCont.SetHealth(1);
    }

    public void BossDead()
    {
        uiHealthBarCont.BossDead();
        bossDead = true;
    }

    public void Bounce() {
        uiScoreCont.Bounce();
    }

    public void BallReset() {
        uiScoreCont.BallReset();
    }

    private void PlayVariableScream(float speed) 
    {
        if (speed >= heavyThreshold)
        {
            PlayHeavyScream();
            paddleCont.PlayBigHurtAnimation();
        } else if (speed > mediumThreshold)
        {
            PlayMediumScream();
            paddleCont.PlayBigHurtAnimation();
        }
        else if (speed > lightThreshold)
        {
            PlayLightScream();
            paddleCont.PlayHurtAnimation();
        }
    }

    private void PlayLightScream() {
        AudioSource toPlay = screamsLight[Random.Range(0, screamsLight.Length)];    // for now just play light scream  
        if (toPlay != null)
            toPlay.Play();
    }
    private void PlayMediumScream() {
        AudioSource toPlay = screamsMedium[Random.Range(0, screamsMedium.Length)];    // for now just play light scream  
        if (toPlay != null)
            toPlay.Play();
    }
    private void PlayHeavyScream() {
        AudioSource toPlay = screamsHeavy[Random.Range(0, screamsHeavy.Length)];    // for now just play light scream  
        if (toPlay != null)
            toPlay.Play();
    }
}
