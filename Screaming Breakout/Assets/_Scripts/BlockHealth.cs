using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockHealth : MonoBehaviour
{
    public int maxHealth;
    private int currentHealth;
    public bool shrinker = false;
    private float shrinkFactor = 0.9f;

    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage()
    {
        currentHealth--;
        // Change image, or shade more red or something. 
        if (shrinker)
        {
            transform.localScale *= shrinkFactor;
        }
    }

    public int GetHealth()
    {
        return currentHealth;
    }
}
