using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockHealth : MonoBehaviour
{
    public int maxHealth;
    private int currentHealth;
    public bool shrinker = false;
    private float shrinkFactor = 0.9f;
    public GameObject deathFace;
    private bool hurt = false;
    public bool animated = false;
    private Animator animator;
    

    void Start()
    {
        currentHealth = maxHealth;
        if (animated)
            animator = GetComponent<Animator>();
    }

    public void TakeDamage()
    {
        currentHealth--;
        if (animated)
        {
            //Debug.Log("Large block has taken damage");
            animator.SetTrigger("Hurt");
        }

        if (shrinker)
        {
            transform.localScale *= shrinkFactor;
        }
    }

    public int GetHealth()
    {
        return currentHealth;
    }

    public void BlockDeath()
    {
        if (deathFace != null)
            Instantiate(deathFace, transform.position, Quaternion.identity);
    }
}
