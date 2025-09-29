using System;
using UnityEditor;
using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class QuestionBoxDynamics : MonoBehaviour
{
    // ==== PUBLIC =====
    public Animator boxAnimator;
    public Animator coinAnimator;
    public bool containCoin = false;
    [NonSerialized] public bool hit = false;

    private SpringJoint2D spring;
    private Rigidbody2D rb;
    private bool disableSpring = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spring = GetComponent<SpringJoint2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (disableSpring && Math.Abs(rb.linearVelocityY) < 0.01f)
        {
            transform.localPosition = Vector3.zero;
            rb.bodyType = RigidbodyType2D.Static;
            spring.enabled = false;
            disableSpring = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Threshold"))
        {
            boxAnimator.SetTrigger("hit");
            disableSpring = true;
            if (containCoin) { emitCoin(); }
        }
    }

    public void resetQuestionBox()
    {
        boxAnimator.SetTrigger("reset");
        coinAnimator.SetTrigger("reset");
        spring.enabled = true;
        rb.bodyType = RigidbodyType2D.Dynamic;
    }

    public void emitCoin()
    {
        coinAnimator.SetTrigger("hit");
    }
}
