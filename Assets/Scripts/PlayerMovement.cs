using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerMovement : MonoBehaviour
{
    // ===  PUBLIC   ===
    public float HorizontalAcc = 10;
    public float DecellerationTime = 0.1f;
    public float upSpeed = 10;
    public float maxSpeed = 20;
    public Vector3 boxSize;
    public float maxDistance;
    public LayerMask layerMask;
    public bool showBox;
    public Animator marioAnimator;

    // ===  PRIVATE  ===
    private bool onGroundState = true;
    private Rigidbody2D marioBody;
    private SpriteRenderer marioSprite;
    private bool faceRightState = true;
    private bool jumpPressed = false;
    private float moveHorizontal = 0;

    // Start is called before the first frame update
    void Start()
    {
        // Set to be 30 FPS
        Application.targetFrameRate = 144;
        marioBody = GetComponent<Rigidbody2D>();
        marioSprite = GetComponent<SpriteRenderer>();
        marioBody.constraints = RigidbodyConstraints2D.FreezeRotation;
        marioAnimator.SetBool("onGround", onGroundState);
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
        // if (collision.gameObject.CompareTag("Ground")) onGroundState = true;
        onGroundState = onGroundCheck();

        // Debug.Log($"{name} COLLIDE {collision.collider.name}");
    }

    private bool onGroundCheck()
    {
        if (Physics2D.BoxCast(transform.position, boxSize, 0, -transform.up, maxDistance, layerMask))
        {
            // Debug.Log("on ground");
            marioAnimator.SetBool("onGround", true);
            return true;
        }
        else
        {
            // Debug.Log("not on ground");
            return false;
        }
    }

    void OnDrawGizmos()
    {
        if (showBox)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawCube(transform.position - transform.up * maxDistance, boxSize);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!GPTManager.Instance.useGPTControl)
        {
            if (Input.GetKeyDown("a") && faceRightState) { setMarioFace(false); }
            if (Input.GetKeyDown("d") && !faceRightState) { setMarioFace(true); }
            detectInput(ref jumpPressed);
            moveHorizontal = Input.GetAxisRaw("Horizontal"); // moveHorizontal = either 0, +1, -1
            marioAnimator.SetFloat("xSpeed", Mathf.Abs(marioBody.linearVelocity.x));
        }

    }

    // FixedUpdate is called 50 times a second
    void FixedUpdate()
    {
        if (!GameManager.Instance.playerAlive) return;
        var velocity = marioBody.linearVelocity;
        if (!GPTManager.Instance.useGPTControl)
        {
            if (Mathf.Abs(moveHorizontal) > 0)
            {
                float target = moveHorizontal * maxSpeed;
                velocity.x = Mathf.MoveTowards(velocity.x, target, HorizontalAcc * Time.fixedDeltaTime);
            }
            else { if (onGroundState) velocity.x = Mathf.MoveTowards(velocity.x, 0f, DecellerationTime); }

            if (jumpPressed && onGroundState)
            {
                velocity.y = upSpeed;
                onGroundState = false;
                jumpPressed = false;
                marioAnimator.SetBool("onGround", false);
            }
            marioBody.linearVelocity = velocity;
        }
        else
        {
            if (GPTManager.Instance.State == GPTState.Playing)
            {
                float target = GPTManager.Instance.dir * maxSpeed;
                velocity.x = Mathf.MoveTowards(velocity.x, target, HorizontalAcc * Time.fixedDeltaTime);
            }
            else { if (onGroundState) velocity.x = Mathf.MoveTowards(velocity.x, 0f, DecellerationTime); }
            if (GPTManager.Instance.doJump && onGroundState)
            {
                velocity.y = upSpeed;
                onGroundState = false;
                GPTManager.Instance.doJump = false;
                marioAnimator.SetBool("onGround", true);
            }
            marioBody.linearVelocity = velocity;
        }
        // Debug.Log(marioBody.linearVelocity);
        // Debug.Log(onGroundState);
    }

    public void setMarioFace(bool right)
    {
        if (!GameManager.Instance.playerAlive) return;
        if (!right)
        {
            faceRightState = false;
            marioSprite.flipX = true;
            if (marioBody.linearVelocity.x > 0.1f)
                marioAnimator.SetTrigger("onSkid");
        }

        if (right)
        {
            faceRightState = true;
            marioSprite.flipX = false;
            if (marioBody.linearVelocity.x < -0.1f)
                marioAnimator.SetTrigger("onSkid");
        }
    }
}