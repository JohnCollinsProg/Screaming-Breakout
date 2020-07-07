using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleControllerV1 : MonoBehaviour
{
    private Vector3 defaultScale;
    private Vector3 defaultPos;
    private Vector3 basePos;
    // 0 = Mouse
    private int inputMode = 0;

    public float edgeClamp;
    public Animator animator;
    
    void Start()
    {
        defaultScale = transform.localScale;
        defaultPos = transform.position;
        basePos = new Vector3(0f, defaultPos.y, defaultPos.z);

    }

    // Update is called once per frame
    void Update()
    {
        if (inputMode == 0)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);
            transform.position = basePos + new Vector3(Mathf.Clamp(mousePos.x, -edgeClamp, edgeClamp), 0f, 0f);
        }
        // Idk why this is not needed, once bool hurt = true, it always true and it should be stuck in the hurt animation??
        // animator.SetBool("Hurt", false);
    }

    // Also I cant figure out how to offset the sprite render, im manually offsetting each sprite atm but this is gonna be a pain
    public void PlayHurtAnimation()
    {
        animator.SetBool("Hurt", true);
    }
}
