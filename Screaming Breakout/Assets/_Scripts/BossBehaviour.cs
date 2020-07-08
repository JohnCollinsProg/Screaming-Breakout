using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehaviour : MonoBehaviour
{
    public int maxHealth;
    private int health;
    private bool alive = true;
    //  Stage 0: swinging shields,  stage 1: spawning falling objects,  stage 2: spawning reactive shields, -1 = dying dramatically, -2 = dead and falling
    private int stage = 0;
    public int stage1Threshold, stage2Threshold;
    public GameObject shield1, shield2;
    private ShieldBehaviour shieldBehaviour1, shieldBehaviour2;
    public GameObject fallingObj, responsiveShield, lowerFallPosObj, upperFallPosObj;
    private Vector3 lowerFallPos, upperFallPos;
    public GameObject dyingObj;
    private Vector3 normalPos;
    private GameController gCont;
    public float fallingObjectRange;
    public float attackPeriod;
    private float nextAttack;
    private bool shieldSpawning = false;
    private float shieldSpawnTime;
    private float shieldDelay = 0.04f;
    private Vector3 shieldSpawnLocation;
    private float deathTime;
    private float deathDuration = 3f;
    private float deathComplete;

    void Start()
    {
        health = maxHealth;
        lowerFallPos = lowerFallPosObj.transform.position;
        upperFallPos = upperFallPosObj.transform.position;
        shieldBehaviour1 = shield1.GetComponent<ShieldBehaviour>();
        shieldBehaviour2 = shield2.GetComponent<ShieldBehaviour>();
        normalPos = transform.position;
        gCont = GameObject.Find("Game Controller").GetComponent<GameController>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            print("Boss health: " + health + ",    stage: " + stage);
        }
        if (Input.GetKeyDown(KeyCode.Y))
            health = 1;

        if (stage == -1)
        {
            float progress = (Time.time - deathTime) / (deathComplete - deathTime);
            //print("Death progress: " + progress);
            //transform.position = Vector3.Lerp(transform.position, dyingObj.transform.position, progress);
            // Alternatively-    transform.position = Vector3.Lerp( ""Position when started dying, or normal position"" , dyingObj.transform.position, progress);
            transform.position = Vector3.Lerp(normalPos, dyingObj.transform.position, progress);
            print("Boss transform: " + transform.position + ",    at Death progress: " + progress);
            if (transform.position.x < dyingObj.transform.position.x)
            {
                print("Boss is " + (transform.position.x - dyingObj.transform.position.x) + " units too far");
            }

            //transform.position = Vector3.MoveTowards(transform.position, dyingObj.transform.position, 0.13f * Time.deltaTime);

            //transform.rotation = Quaternion.Lerp(transform.rotation, dyingObj.transform.rotation, progress);
            //transform.rotation = Quaternion.Lerp(Quaternion.identity, dyingObj.transform.rotation, progress);
            float zRot = Mathf.LerpAngle(0, dyingObj.transform.rotation.z * Mathf.Rad2Deg, progress);
            transform.rotation = Quaternion.Euler(0f, 0f, zRot);
            if (progress > 1)
            {
                //Debug.Break();
                stage = -2;
                //Destroy(gameObject.GetComponent<CircleCollider2D>());
                Rigidbody2D rb = gameObject.AddComponent<Rigidbody2D>();
                rb.gravityScale = 0.7f;
                rb.velocity = Vector3.zero;
            }
        }
    }
    void FixedUpdate()
    {
        if (alive) { 
            if (stage > 0)  // Drop falling objects
            {
                if (Time.time >= nextAttack)    // Drop an object
                {
                    nextAttack += attackPeriod;
                    int spawnArea = Random.Range(0, 2); // 0 = bottom, 1 = top left, 2 = top right
                    Vector3 spawnPos = new Vector3(Random.Range(-fallingObjectRange, fallingObjectRange), lowerFallPos.y, lowerFallPos.z);// Left here for safety, should always be overwritten. 
                    switch (spawnArea)
                    {
                        case 0:
                            spawnPos = new Vector3(Random.Range(-fallingObjectRange, fallingObjectRange), lowerFallPos.y, lowerFallPos.z);
                            break;
                        case 1:
                            spawnPos = new Vector3(-upperFallPos.x, upperFallPos.y + Random.Range(-fallingObjectRange / 2, fallingObjectRange / 2), upperFallPos.z);
                            break;
                        case 2:
                            spawnPos = new Vector3(upperFallPos.x, upperFallPos.y + Random.Range(-fallingObjectRange / 2, fallingObjectRange / 2), upperFallPos.z);
                            break;
                    }
                    //Vector3 spawnPos = new Vector3(Random.Range(-fallingObjectRange, fallingObjectRange), fallPos.y, fallPos.z);
                    Instantiate(fallingObj, spawnPos, Quaternion.identity);
                    //print("Boss is attacking at: " + Time.time + ",    next attack at: " + nextAttack);
                }
            }
            if (stage > 1)
            {
                if (shieldSpawning && Time.time >= shieldSpawnTime)
                {
                    shieldSpawning = false;
                    Instantiate(responsiveShield, shieldSpawnLocation, Quaternion.identity);
                }
            }
            
        }
    }

    public void TakeDamage(Vector2 collisionPos)
    {
        if (health - 1 > 0) // Taking damage will not kill the boss
        {
            health--;
            if (health == stage2Threshold)  // Progress to stage 2
            {
                stage = 2;
                print("Boss entering stage 2.");

            }
            else if (health == stage1Threshold)//Progress to stage 1
            {
                stage = 1;
                print("Boss entering stage 1.");
                nextAttack = Time.time + 0.2f;
            }
            if (stage > 1)  // Begin spawning a responsive shield
            {
                //Instantiate(responsiveShield, collisionPos, Quaternion.identity);
                shieldSpawnLocation = collisionPos;
                shieldSpawning = true;
                shieldSpawnTime = Time.time + shieldDelay;
            }
        }
        else if (alive) // Damage will be fatal
        {
            print("The boss has died.");
            Destroy(gameObject.GetComponent<CircleCollider2D>());
            health = 0;
            alive = false;
            stage = -1;
            deathTime = Time.time;
            deathComplete = Time.time + deathDuration;
            shieldBehaviour1.DropAway();
            shieldBehaviour2.DropAway();
        }
    }

    public int GetHealth()
    {
        return health;
    }
}
