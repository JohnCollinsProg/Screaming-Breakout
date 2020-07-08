using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehaviour : MonoBehaviour
{
    public int maxHealth;
    private int health;
    private bool alive = true;
    //  Stage 0: swinging shields,  stage 1: spawning falling objects,  stage 2: spawning reactive shields
    private int stage = 0;
    public int stage1Threshold, stage2Threshold;
    public GameObject shield1, shield2;
    public GameObject fallingObj, responsiveShield, lowerFallPosObj, upperFallPosObj;
    private Vector3 lowerFallPos, upperFallPos;
    public float fallingObjectRange;
    public float attackPeriod;
    private float nextAttack;

    void Start()
    {
        health = maxHealth;
        lowerFallPos = lowerFallPosObj.transform.position;
        upperFallPos = upperFallPosObj.transform.position;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            print("Boss health: " + health + ",    stage: " + stage);
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
                            spawnPos = new Vector3(upperFallPos.x, upperFallPos.y + Random.Range(-fallingObjectRange / 2, fallingObjectRange / 2), upperFallPos.z);
                            break;
                        case 2:
                            spawnPos = new Vector3(-upperFallPos.x, upperFallPos.y + Random.Range(-fallingObjectRange / 2, fallingObjectRange / 2), upperFallPos.z);
                            break;
                    }
                    //Vector3 spawnPos = new Vector3(Random.Range(-fallingObjectRange, fallingObjectRange), fallPos.y, fallPos.z);
                    Instantiate(fallingObj, spawnPos, Quaternion.identity);
                    //print("Boss is attacking at: " + Time.time + ",    next attack at: " + nextAttack);
                }
            }
        }
    }

    public void TakeDamage()
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
        }
    }

    public int GetHealth()
    {
        return health;
    }
}
