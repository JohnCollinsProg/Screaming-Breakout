using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeLife : MonoBehaviour
{
    public float lifeTime;
    //private float spawnTime;
    private float cullTime;
    void Start()
    {
        //spawnTime = Time.time;
        cullTime = Time.time + lifeTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= cullTime)
        {
            Destroy(this.gameObject);
        }
    }
}
