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
    private bool blocked;
    private Direction blockedDirection;
    private float accumTime;
    public float dyingTime = 0.5f;
    public Vector2 orientation;
    private KeyCode[] movementKeyCodes = new KeyCode[]
 {
         KeyCode.LeftArrow,
         KeyCode.RightArrow,
         KeyCode.DownArrow,
         KeyCode.UpArrow
 };

    protected enum Direction
    {
        Left = 0,
        Right = 1,
        Up = 2,
        Down = 3,
        None = 4
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
        // Start facing up 
        orientation = new Vector2(0, 1);
    }

    void Update()
    {
        switch (state)
        {
            case State.Alive:
                if (AnyKeyPressed(movementKeyCodes))
                {
                    HandleMovement();
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

    private void HandleMovement()
    {
        orientation = new Vector2(0, 0);
        //Movimiento horizontal
        if (Input.GetKey(KeyCode.LeftArrow))
        {            
            direction = Direction.Left;            
            animator.SetInteger("KeyPressed", Convert.ToInt32(direction));
            orientation = new Vector2(-1, orientation.y);
            if (direction != blockedDirection)
            {
                var x = Math.Max(transform.position.x - movSpeed * Time.deltaTime, -screenMarginLimitX);
                transform.position = new Vector3(x, transform.position.y, transform.position.z);
                blockedDirection = Direction.None;
            }
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            direction = Direction.Right;
            animator.SetInteger("KeyPressed", Convert.ToInt32(direction));
            orientation = new Vector2(1, orientation.y);
            if (direction != blockedDirection)
            {
                var x = Math.Min(transform.position.x + movSpeed * Time.deltaTime, screenMarginLimitX);
                transform.position = new Vector3(x, transform.position.y, transform.position.z);
                blockedDirection = Direction.None;

            }
        }

        //Movimiento vertical
        if (Input.GetKey(KeyCode.UpArrow))
        {
            direction = Direction.Up;
            animator.SetInteger("KeyPressed", Convert.ToInt32(direction));
            orientation = new Vector2(orientation.x, 1);
            if (direction != blockedDirection)
            {
                var y = Math.Min(transform.position.y + movSpeed * Time.deltaTime, screenMarginLimitY);
                transform.position = new Vector3(transform.position.x, y, transform.position.z);
                blockedDirection = Direction.None;

            }
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            direction = Direction.Down;
            animator.SetInteger("KeyPressed", Convert.ToInt32(direction));
            orientation = new Vector2(orientation.x, -1);
            if (direction != blockedDirection)
            {
                var y = Math.Max(transform.position.y - movSpeed * Time.deltaTime, -screenMarginLimitY);
                transform.position = new Vector3(transform.position.x, y, transform.position.z);
                blockedDirection = Direction.None;
            }
        }

        //Disparos
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot(direction);
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
            Projectile proj = projectile.gameObject.GetComponent<Projectile>();
            proj.Bearing = orientation;
            proj.FromPlayer = true;
            projectile.gameObject.SetActive(true);
        }
    }

    private bool AnyKeyPressed(KeyCode[] keyCodes)
    {
        for (int i = 0; i < keyCodes.Length; i++)
        {
            if (Input.GetKey(keyCodes[i]))
                return true;
        }
        return false;
    }

    private void Die()
    {
        state = State.Dying;
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        if (state == State.Alive)
        {
            if (string.Compare(col.gameObject.tag, "Enemy") == 0)
            {
                blockedDirection = direction;
            }
        }
        UnityEngine.Debug.Log("Choque");
    }

    public void OnTriggerStay2D(Collider2D col)
    {
        //if (state == State.Alive)
        //{
        //    if (string.Compare(col.gameObject.tag, "Enemy") == 0)
        //    {
        //        blockedDirection = direction;
        //    }
        //}
        UnityEngine.Debug.Log("Estoy chocando");
    }

    public void OnTriggerExit2D(Collider2D col)
    {
        //if (state == State.Alive)
        //{
        //    if (string.Compare(col.gameObject.tag, "Enemy") == 0)
        //    {
        //        blockedDirection = direction;
        //    }
        //}
        UnityEngine.Debug.Log("Deje de chocar");
    }
}
