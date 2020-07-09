using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoofBreaker : MonoBehaviour
{
    private Rigidbody2D rbLeft, rbMid, rbRight;
    private GameObject left, mid, right;
    public float gravityScale;
    public float forcePush;
    public float rotationRange;

    void Start()
    {
        //for (int i=0; i > transform.childCount; i++)
        left = transform.GetChild(0).gameObject;
        mid = transform.GetChild(1).gameObject;
        right = transform.GetChild(2).gameObject;
        rbLeft = left.GetComponent<Rigidbody2D>();
        rbMid = mid.GetComponent<Rigidbody2D>();
        rbRight = right.GetComponent<Rigidbody2D>();

        rbLeft.gravityScale = 0f;
        rbMid.gravityScale = 0f;
        rbRight.gravityScale = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            BreakOpen();
        }
    }

    public void BreakOpen()
    {
        rbLeft.gravityScale = gravityScale;
        rbMid.gravityScale = gravityScale;
        rbRight.gravityScale = gravityScale;

        rbLeft.AddForce(new Vector2(-left.transform.position.x / 2, forcePush), ForceMode2D.Impulse);
        rbMid.AddForce(new Vector2(Random.Range(-1.3f,1.3f), forcePush), ForceMode2D.Impulse);
        rbRight.AddForce(new Vector2(-right.transform.position.x / 2, forcePush), ForceMode2D.Impulse);

        rbLeft.AddTorque(Random.Range(-rotationRange, rotationRange));
        rbMid.AddTorque(Random.Range(-rotationRange, rotationRange));
        rbRight.AddTorque(Random.Range(-rotationRange, rotationRange));
    }
}
