using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

    private float originalX;
    private float maxOffset = 5.0f;
    private float enemyPatroltime = 2.0f;
    private int moveRight = -1;
    private Vector2 velocity;

    private Rigidbody2D enemyBody;
    private int obstacleLayer;

    public Vector3 startPosition = new Vector3(0.0f, 0.0f, 0.0f);


    void Start()
    {
        enemyBody = GetComponent<Rigidbody2D>();
        // get the starting position
        startPosition = transform.localPosition;
        originalX = transform.position.x;
        ComputeVelocity();
        obstacleLayer = LayerMask.NameToLayer("Obstacle");
    }
    void ComputeVelocity()
    {
        velocity = new Vector2(moveRight * maxOffset / enemyPatroltime, 0);
    }
    void Movegoomba()
    {
        enemyBody.MovePosition(enemyBody.position + velocity * Time.fixedDeltaTime);
    }

    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        // 벽(Obstacle 레이어)과 충돌하면 방향 전환
        if (col.collider.gameObject.layer == obstacleLayer)
        {
            moveRight *= -1;
            ComputeVelocity();
        }
    }


    void FixedUpdate()
    {
        if (Mathf.Abs(enemyBody.position.x - originalX) < maxOffset)
        {// move goomba
            Movegoomba();
        }
        else
        {
            // change direction
            moveRight *= -1;
            ComputeVelocity();
            Movegoomba();
        }
    }
}