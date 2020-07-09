using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleController : MonoBehaviour
{
    // 0 = displaying only title, 1 = displaying click to progress, 2 = displaying tutorial, 3 = loading game
    private int stage = 0;
    private float displayClick = 3f;

    private GameObject dataObj;
    private StaticData staticData;
    private BallController ballController;
    public GameObject ballObj;
    private GameObject ballInstance;
    public Vector3 initVelo;
    private Rigidbody2D ballRb;

    public GameObject clickPrompt, tutorial;

    void Start()
    {
        dataObj = GameObject.Find("Data");
        if (dataObj != null)
        {
            staticData = dataObj.GetComponent<StaticData>();

            if (staticData.GetBallOnTitle())
            {
                ballInstance = Instantiate(ballObj, Vector3.zero, Quaternion.identity);
                ballRb = ballInstance.GetComponent<Rigidbody2D>();
                ballRb.velocity = initVelo;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (stage == 2 && Input.GetMouseButtonDown(0))  // Load game
        {
            print("Loading Scene");
            SceneManager.LoadScene("FinalScene");
        }
        if (stage == 1 && Input.GetMouseButtonDown(0))  // Display tutorial
        {
            stage = 2;
            print("enabling tutorial");
            tutorial.SetActive(true);
        }
        if ((stage == 0) && Input.GetMouseButtonDown(0) || (Time.time > displayClick))  // Display the click prompt
        {
            if (stage == 0)
                stage = 1;
            print("Displaying prompt");
            clickPrompt.SetActive(true);
        }
        
        
    }
}
