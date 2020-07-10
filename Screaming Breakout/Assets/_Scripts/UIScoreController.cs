using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScoreController : MonoBehaviour {
    
    public Text text;
    public Text summaryText;

    private int score = 0;
    private float multiplier = 1;

    private int livesUsed = 0;
    private int maxBounces = 0;
    private int maxRally = 0;
    private int curBounces = 0;
    private int curRally = 0;
    private int ballResets = 0;
    private int totalScore = 0;

    public int blockHitScore = 5;
    public int bigBlockScore = 15;
    public int bossHitScore = 3;
    public int bossKillScore = 50;
    public int livesScore = 50;
    public int ballResetCost = 25;

    private int highScore = 0;

    void Start () {
    }


    public void Bounce() {
        curBounces++;
    }

    public void HitPaddle() {
        curRally++;
        if (curRally % 5 == 0) {
            multiplier = 1 + (curRally/100f) * 2;
            UpdateUI();
        }
    } 

    public void BlockHit() {
        UpdateScore(blockHitScore);
    }

    public void BigBlockDeath() {
        UpdateScore(bigBlockScore);
    }

    public void BossHit() {
        UpdateScore(bossHitScore);
    }

    public void BallReset() {
        ComboReset();
        ballResets++;
    }

    public void Die() {
        ComboReset();
    }

    public void GameWon() {
        ComboReset();
        CalcTotal();
        ScoreSummary();
    }

    public void ComboReset() {
        if (curBounces > maxBounces) {
            maxBounces = curBounces;
        }
        if (curRally > maxRally) {
            maxRally = curRally;
        }
        curBounces = 0;
        curRally = 0;
        multiplier = 1;
        UpdateUI();
    }

    private void UpdateScore(int increment) {
        score += Mathf.RoundToInt(multiplier * increment);
        UpdateUI();
    }

    private void UpdateUI() {
        text.text = "Score: " + score.ToString() + "\nMulti: " + multiplier.ToString() + "x";
    }

    private int CalcMaxRally() {
        return Mathf.RoundToInt(maxRally * 1.5f);
    }

    private int CalcMaxBounce() {
        return Mathf.RoundToInt(maxBounces * 0.8f);
    }

    private int CalcLives() {
        if (livesUsed < 5) {
            return (5 - livesUsed) * livesScore;
        }
        return 0;
    }

    private int CalcBallResets() {
        return -ballResets * ballResetCost;
    }

    private void CalcTotal() {
        totalScore = Mathf.RoundToInt(score + CalcMaxRally() + CalcMaxBounce() + CalcLives() + CalcBallResets() + bossKillScore);
    }

    public void ScoreSummary() {
        string hs;
        if (totalScore > highScore) {
            highScore = totalScore;
            hs = "New high score!\t\t\t\t\t\t";
        } else {
            hs = "Final score:\t\t\t\t\t\t\t";
        }


        summaryText.text = "Score:\t\t\t\t\t\t\t\t\t" + score.ToString() + "\n"
            + "Longest rally(" + maxRally.ToString() + "):\t\t\t\t\t" + CalcMaxRally().ToString() + "\n"
            + "Most bounces in a row(" + maxBounces.ToString() + "):\t\t" + CalcMaxBounce().ToString() + "\n"
            + "Lives used(" + livesUsed.ToString() +"):\t\t\t\t\t\t" + CalcLives().ToString() + "\n"
            + "Ball resets:\t\t\t\t\t\t\t" + CalcBallResets() + "\n\n"
            + hs + "" + totalScore.ToString();
    }
}