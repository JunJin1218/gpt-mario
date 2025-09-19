using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerMovement : MonoBehaviour
{
    public float speed = 10;
    public float upSpeed = 10;
    public float maxSpeed = 20;
    private bool onGroundState = true;
    private Rigidbody2D marioBody;
    private SpriteRenderer marioSprite;
    private bool faceRightState = true;
    private bool jumpPressed = false;

    // Start is called before the first frame update
    void Start()
    {
        // Set to be 30 FPS
        Application.targetFrameRate = 144;
        marioBody = GetComponent<Rigidbody2D>();
        marioSprite = GetComponent<SpriteRenderer>();

    }

    void detectInput(ref bool pressed)
    {
        if (Input.GetKeyDown("space"))
            pressed = true;

        if (Input.GetKey("space"))
            pressed = true;

        if (Input.GetKeyUp("space"))
            pressed = false;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if Mario is on ground (prevent double-jump)
        // --> Check colliding object has Tag "Ground"
        if (collision.gameObject.CompareTag("Ground")) onGroundState = true;

        // Debug.Log($"{name} COLLIDE {collision.collider.name}");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("a") && faceRightState) { setMarioFace(false); }

        if (Input.GetKeyDown("d") && !faceRightState) { setMarioFace(true); }

        detectInput(ref jumpPressed);
    }

    // FixedUpdate is called 50 times a second
    void FixedUpdate()
    {
        // moveHorizontal = either 0, +1, -1
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        if (Mathf.Abs(moveHorizontal) > 0)
        {
            Vector2 movement = new Vector2(moveHorizontal, 0);
            // Clamp maxspeed
            if (marioBody.linearVelocity.magnitude < maxSpeed)
                marioBody.AddForce(movement * speed);
        }

        // stop
        if (Input.GetKeyUp("a") || Input.GetKeyUp("d"))
        {
            // stop
            marioBody.linearVelocity = Vector2.zero;
        }

        if (jumpPressed && onGroundState)
        {
            marioBody.AddForce(Vector2.up * upSpeed, ForceMode2D.Impulse);
            onGroundState = false;
            jumpPressed = false;
        }
        // Debug.Log(marioBody.linearVelocity);
        // Debug.Log(onGroundState);
    }

    public void setMarioFace(bool right)
    {
        if (!right)
        {
            faceRightState = false;
            marioSprite.flipX = true;
        }

        if (right)
        {
            faceRightState = true;
            marioSprite.flipX = false;
        }
    }
}