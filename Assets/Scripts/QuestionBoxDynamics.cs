using System;
using UnityEditor;
using UnityEngine;
using System.Collections;

public class QuestionBoxDynamics : MonoBehaviour
{
    // ==== PUBLIC =====
    public Animator boxAnimator;
    [NonSerialized] public bool hit = false;

    private SpringJoint2D spring;
    private Rigidbody2D rb;
    private bool destroySpring = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spring = GetComponent<SpringJoint2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (destroySpring && Math.Abs(rb.linearVelocityY) < 0.01f)
        {
            transform.localPosition = Vector3.zero;
            rb.bodyType = RigidbodyType2D.Static;
            Destroy(spring);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Threshold"))
        {
            boxAnimator.SetTrigger("hit");
            destroySpring = true;
        }
    }
}
