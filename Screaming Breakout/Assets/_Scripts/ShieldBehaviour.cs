using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldBehaviour : MonoBehaviour
{
    public GameObject bossObj;
    public float rotationSpeed;
    // Maybe opt for rotation frequency instead if possible??
    private bool active;
    private bool dead = false;
    private float cullTime;
    public GameObject upperLimit, lowerLimit;
    public float minAngle, maxAngle;
    private float radMinAngle, radMaxAngle;
    [Range(-1,1)]
    public int direction;
    private float distanceToBoss;

    void Start()
    {
        distanceToBoss = Vector3.Magnitude(transform.position - bossObj.transform.position);
        active = true;//just for early testing
        radMinAngle = Mathf.Deg2Rad * minAngle;
        radMaxAngle = Mathf.Deg2Rad * maxAngle;
        float curAngle = Mathf.Atan((transform.position.x - bossObj.transform.position.x) / (transform.position.y - bossObj.transform.position.y));
        if (curAngle < 0)
        {
            radMaxAngle *= -1;
            radMinAngle *= -1;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (active && !dead)
        {
            float curAngle = Mathf.Atan((transform.position.x - bossObj.transform.position.x) / (transform.position.y - bossObj.transform.position.y));
            if (Mathf.Abs(curAngle) > Mathf.Abs(radMaxAngle) || Mathf.Abs(curAngle) < Mathf.Abs(radMinAngle))
            {
                direction *= -1;
                //print(gameObject.name + " is changing direction.");
            }
            //print(gameObject.name + " current angle: " + curAngle + "    = " + (Mathf.Rad2Deg * curAngle) + " Degrees.");
            transform.RotateAround(bossObj.transform.position, Vector3.forward, rotationSpeed * Time.deltaTime * direction);
            //transform.LookAt(bossObj.transform);
            Vector3 towardsBoss = transform.position - bossObj.transform.position;
            //transform.rotation = Quaternion.Euler(0f, 0f, (curAngle + 90f) * Mathf.Rad2Deg);
            //transform.LookAt(new Vector3(bossObj.transform.position.x, transform.position.y, transform.position.z));
        }
    }

    private void Update()
    {
        if (dead && Time.time > cullTime)
            Destroy(gameObject);
    }

    public void DropAway()  // Boss defeated, through self away
    {
        //print(gameObject.name + " is dropping away.");
        transform.parent = null;
        Destroy(gameObject.GetComponent<BoxCollider2D>());  // So nothing is collided with on way down
        Rigidbody2D rb = gameObject.AddComponent<Rigidbody2D>();
        rb.gravityScale = 3f; // So they fall fast
        dead = true;
        cullTime = Time.time + 5f;
        float forceSign = transform.position.x / (Mathf.Abs(transform.position.x)); // Detects if the object is on the left or right and hence where to be thrown. Should return 1 or -1
        rb.AddForce(new Vector2(5f * forceSign, 8f), ForceMode2D.Impulse);
    }
}
