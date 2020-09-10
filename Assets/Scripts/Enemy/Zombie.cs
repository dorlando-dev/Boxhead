using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    public const string Tag = "Enemy";
    public float movSpeed = 2.5f;
    private float screenMarginLimitX;
    private float screenMarginLimitY;
    public Animator animator;
    private Direction direction;
    private State state;
    private float accumTime;
    public float dyingTime = 0.5f;

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
    }

    void Update()
    {
        switch (state)
        {
            case State.Alive:
                animator.SetInteger("Direction", Convert.ToInt32(direction));
                if (Input.GetKey(KeyCode.D))
                    Die();
                break;
            case State.Dying:
                accumTime += Time.deltaTime;
                if (accumTime > dyingTime)
                {
                    gameObject.SetActive(false);
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
            Die();
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }        
    }

    private void Die()
    {
        state = State.Dying;
        animator.SetInteger("Dead", Convert.ToInt32(direction));
    }
}
