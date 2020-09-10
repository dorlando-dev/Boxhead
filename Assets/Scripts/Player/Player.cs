using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Player : MonoBehaviour
{
    public const string Tag = "Player";
    public float movSpeed = 5f;
    private float screenMarginLimitX;
    private float screenMarginLimitY;
    public Transform projectileStartPointLeft;
    public Transform projectileStartPointRight;
    public Transform projectileStartPointUp;
    public Transform projectileStartPointDown;
    public Animator animator;
    private Direction direction;
    private State state;
    private float accumTime;
    public float dyingTime = 0.5f;

    protected enum Direction
    {
        Left = 0,
        Right = 1,
        Up = 2,
        Down = 3
    }

    protected enum State
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
        direction = Direction.Up;
    }

    void Update()
    {
        switch (state)
        {
            case State.Alive:
                //Movimiento horizontal
                if (Input.GetKey(KeyCode.LeftArrow))
                {
                    direction = Direction.Left;
                    animator.SetInteger("KeyPressed", Convert.ToInt32(direction));
                    var x = Math.Max(transform.position.x - movSpeed * Time.deltaTime, -screenMarginLimitX);
                    transform.position = new Vector3(x, transform.position.y, transform.position.z);
                }
                else if (Input.GetKey(KeyCode.RightArrow))
                {
                    direction = Direction.Right;
                    animator.SetInteger("KeyPressed", Convert.ToInt32(direction));
                    var x = Math.Min(transform.position.x + movSpeed * Time.deltaTime, screenMarginLimitX);
                    transform.position = new Vector3(x, transform.position.y, transform.position.z);
                }

                //Movimiento vertical
                if (Input.GetKey(KeyCode.UpArrow))
                {
                    direction = Direction.Up;
                    animator.SetInteger("KeyPressed", Convert.ToInt32(direction));
                    var y = Math.Min(transform.position.y + movSpeed * Time.deltaTime, screenMarginLimitY);
                    transform.position = new Vector3(transform.position.x, y, transform.position.z);
                }
                else if (Input.GetKey(KeyCode.DownArrow))
                {
                    direction = Direction.Down;
                    animator.SetInteger("KeyPressed", Convert.ToInt32(direction));
                    var y = Math.Max(transform.position.y - movSpeed * Time.deltaTime, -screenMarginLimitY);
                    transform.position = new Vector3(transform.position.x, y, transform.position.z);
                }

                //Disparos
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    Shoot(direction);
                }
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

    private void Shoot(Direction direction)
    {
        var projectile = PoolManager.Instance.GetPool("Projectile").GetItem();
        if (projectile != null)
        {
            switch (direction)
            {
                case Direction.Left:
                    projectile.transform.position = projectileStartPointLeft.position;
                    break;
                case Direction.Right:
                    projectile.transform.position = projectileStartPointRight.position;
                    break;
                case Direction.Up:
                    projectile.transform.position = projectileStartPointUp.position;
                    break;
                case Direction.Down:
                    projectile.transform.position = projectileStartPointDown.position;
                    break;
            }
            projectile.gameObject.SetActive(true);
        }

    }

    private void Die()
    {
        state = State.Dying;
        animator.SetBool("Alive", state == State.Alive);
    }
}
