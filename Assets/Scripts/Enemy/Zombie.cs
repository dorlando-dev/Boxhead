using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : PoolItem
{
    public const string Tag = "Enemy";
    public float movSpeed = 2.5f;
    public float dyingTime = 0.5f;
    public GameObject player;
    public Animator animator;    

    private float screenMarginLimitX;
    private float screenMarginLimitY;
    private Direction direction;
    private State state;
    private float accumTime;
    private float startX = -0.67f, startY = 3.49f;

    private enum Direction
    {
        Left = 0,
        Right = 1,
        Up = 2,
        Down = 3
    }

    private enum State
    {
        Alive,
        Dying,
        Dead
    }

    void Awake()
    {
        screenMarginLimitX = Camera.main.orthographicSize * 0.9f * Camera.main.aspect;
        screenMarginLimitY = Camera.main.orthographicSize * 0.85f;
        state = State.Alive;
        direction = Direction.Down;
        transform.position = new Vector2(startX, startY);
    }

    void Update()
    {
        switch (state)
        {
            case State.Alive:
                HandleMovement();
                break;
            case State.Dying:
                accumTime += Time.deltaTime;
                if (accumTime > dyingTime)
                {
                    Destroy();
                    state = State.Dead;
                }
                break;
            case State.Dead:
                break;

        }
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        if(state == State.Alive)
        {
            if (string.Compare(col.gameObject.tag, "Projectile") == 0)
            {
                Die();
                gameObject.GetComponent<BoxCollider2D>().enabled = false;
            }
        }        
    }

    private void HandleMovement() {
        if (player == null) {
            SetPlayer();
        }

        Vector3 heading = player.transform.position - transform.position;
        heading = heading.normalized;

        if(Mathf.Abs(heading.x) > Mathf.Abs(heading.y))
        {
            direction = heading.x > 0? Direction.Right : Direction.Left;
        } else
        {
            direction = heading.y > 0? Direction.Up : Direction.Down;
        }

        animator.SetInteger("Direction", Convert.ToInt32(direction));

        float newX = FitToBounds(transform.position.x + heading.x * movSpeed * Time.deltaTime, -screenMarginLimitX, screenMarginLimitX);
        float newY = FitToBounds(transform.position.y + heading.y * movSpeed * Time.deltaTime, -screenMarginLimitY, screenMarginLimitY);
        transform.position = new Vector3(newX, newY, transform.position.z);
    }

    private void SetPlayer() {
        player = GameObject.FindGameObjectsWithTag("Player")[0];
    }

    private void Die()
    {
        state = State.Dying;
        animator.SetInteger("Dead", Convert.ToInt32(direction));
    }

    private float FitToBounds(float value, float min, float max)
    {
        if (value > max)
        {
            return max;
        }

        if (value < min)
        {
            return min;
        }

        return value;
    }

    private void Destroy()
    {
        ReturnToPool();
    }
}
