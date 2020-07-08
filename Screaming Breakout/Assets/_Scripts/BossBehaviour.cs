using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehaviour : MonoBehaviour
{
    public int maxHealth;
    private int health;
    public int stage2Threshold, stage3Threshold;
    public GameObject shield1, shield2;
    public GameObject fallingObj, responsiveShield;

    void Start()
    {
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
