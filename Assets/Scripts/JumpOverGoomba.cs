using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class JumpOverGoomba : MonoBehaviour
{
    public Transform enemyLocation;
    public GameObject enemies;
    public TextMeshProUGUI scoreText;

    private bool onGroundState;

    private bool countScoreState = false;
    public Vector3 boxSize;
    public float maxDistance;
    public LayerMask layerMask;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // mario jumps
        if (Input.GetKeyDown("space") && onGroundCheck())
        {
            onGroundState = false;
            countScoreState = true;
        }

    }

    void FixedUpdate()
    {
        // when jumping, and Goomba is near Mario and we haven't registered our score
        // Problem: when jumping over 2 < enemies, giving only 1 point
        // Problem2: while jumping over goomba + press space very fast = multiple scores
        if (!onGroundState && countScoreState)
        {
            foreach (Transform enemyTransform in enemies.transform)
            {
                if (Mathf.Abs(transform.position.x - enemyTransform.position.x) < 0.5f)
                {
                    countScoreState = false;
                    GameManager.Instance.AddScore(1);
                    UIManager.Instance.updateScoreTexts();
                    Debug.Log(GameManager.Instance.GetScore());
                }
            }

        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground")) onGroundState = true;
    }


    private bool onGroundCheck()
    {
        if (Physics2D.BoxCast(transform.position, boxSize, 0, -transform.up, maxDistance, layerMask))
        {
            Debug.Log("on ground");
            return true;
        }
        else
        {
            Debug.Log("not on ground");
            return false;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawCube(transform.position - transform.up * maxDistance, boxSize);
    }

}
