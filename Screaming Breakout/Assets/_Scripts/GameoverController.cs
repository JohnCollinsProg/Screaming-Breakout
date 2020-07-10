﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameoverController : MonoBehaviour
{

    public AudioSource bgm;
    // Start is called before the first frame update
    void Start()
    {
        if (bgm != null) {
            bgm.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            SceneManager.LoadScene("TitleScene");
    }
}
