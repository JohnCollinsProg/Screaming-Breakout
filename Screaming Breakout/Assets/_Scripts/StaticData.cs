using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticData : MonoBehaviour
{
    private bool ballOnTitle = false;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void AllowBallOnTitle()
    {
        ballOnTitle = true;
    }

    public bool GetBallOnTitle()
    {
        return ballOnTitle;
    }
}
