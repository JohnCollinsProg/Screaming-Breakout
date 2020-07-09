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
    private bool impaired = false;
    private float slowAmount;
    private float unimpairedTime;

    public float edgeClamp;
    public Animator animator;
    public float impairTime;
    
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
            if (!impaired)// Normal mouse movement state. 
            {
                Vector3 mousePos = Input.mousePosition;
                mousePos = Camera.main.ScreenToWorldPoint(mousePos);
                transform.position = basePos + new Vector3(Mathf.Clamp(mousePos.x, -edgeClamp, edgeClamp), Camera.main.transform.position.y, 0f);
            }
            else          // Affected by a falling blocks impairment, slowing movement
            {
                Vector3 mousePos = Input.mousePosition;
                mousePos = Camera.main.ScreenToWorldPoint(mousePos);
                float newXPos = Mathf.Lerp(transform.position.x, mousePos.x, slowAmount);//0.005f
                transform.position = basePos + new Vector3(Mathf.Clamp(newXPos, -edgeClamp, edgeClamp), Camera.main.transform.position.y, 0f);
                //print("Mouse X: " + mousePos.x + ",    newXPos: " + newXPos + ",    transform.position.x: " + transform.position.x);
            }
        }
        // Idk why this is not needed, once bool hurt = true, it always true and it should be stuck in the hurt animation??
        // animator.SetBool("Hurt", false);

        if (impaired && Time.time >= unimpairedTime)
        {
            impaired = false;
            animator.SetBool("Impaired", false);
        }

        
    }

    // Also I cant figure out how to offset the sprite render, im manually offsetting each sprite atm but this is gonna be a pain
    public void PlayHurtAnimation()
    {
        animator.SetTrigger("Hurt");
    }

    public void PlayBigHurtAnimation()
    {
        animator.SetTrigger("HurtHeavy");
    }

    public void PlayWinAnimation() {
        animator.SetBool("WinGame", true);
    }

    public void TakeImpairingDamage(float slowAmount)
    {
        impaired = true;
        this.slowAmount = slowAmount;
        unimpairedTime = Time.time + impairTime;
        animator.SetBool("Impaired", true);
        print("You have been impaired");
    }
}
