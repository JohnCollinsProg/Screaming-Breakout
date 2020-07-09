using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathFace : MonoBehaviour
{
    public float lifeTime;
    private float cullTime;
    private float spawnTime;
    public float maxScale;
    private float originalScale;

    void Start()
    {
        spawnTime = Time.time;
        cullTime = spawnTime + lifeTime;

        originalScale = transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= cullTime)
            Destroy(gameObject);
        float progress = (Time.time - spawnTime) / (cullTime - spawnTime);
        float scale = Mathf.Lerp(originalScale, maxScale, progress);
        transform.localScale = new Vector3(scale, scale, 1f);
    }
}
