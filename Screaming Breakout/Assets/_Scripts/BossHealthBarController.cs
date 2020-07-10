using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBarController : MonoBehaviour {

    public GameObject healthBarObj;
    private Image healthBarFill;
    private RectTransform rectTransform;
    private float maxHealth;
    private float curHealth;
    private float increment;

    void Start () {
        healthBarFill = GameObject.Find("Fill").GetComponent<Image>();
        healthBarObj.SetActive(false);
        rectTransform = healthBarFill.GetComponent<RectTransform>();
    }

    public void SetMaxHealth(int maxHealth) {
        increment = 100f/maxHealth;
        this.maxHealth = maxHealth;
        curHealth = maxHealth;
    }

    public void BossBattleStart() {
        healthBarObj.SetActive(true);
    }

    public void TakeDamage() {
        curHealth--;
        UpdateUI();
    }

    public void BossDead()
    {
        healthBarObj.SetActive(false);
    }

    public void SetHealth(int health) {
        curHealth = health;
        UpdateUI();
    }

    public void UpdateUI() {
        float newScale = curHealth/maxHealth;
        rectTransform.localScale = new Vector3 (newScale, 1f, 1f);
    }

}