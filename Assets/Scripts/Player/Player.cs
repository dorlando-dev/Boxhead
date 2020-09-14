using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using UnityEngine;

public class Player : MonoBehaviour
{
    public const string Tag = "Player";
    public float movSpeed = 5f;
    public float dyingTime = 0.5f;

    public Vector2 orientation;
    public Transform projectileStartPointLeft;
    public Transform projectileStartPointRight;
    public Transform projectileStartPointUp;
    public Transform projectileStartPointDown;
    public Animator animator;


    private float screenMarginLimitX;
    private float screenMarginLimitY;
    private Direction direction;
    private State state;
    private bool blocked;
    private Direction blockedDirection;
    private float accumTime;
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

                //Spawn zombies
                if (Input.GetKeyDown(KeyCode.Z))
                {
                    SpawnZombie();
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
        int vx = 0, vy = 0;
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            vx = -1;
            direction = Direction.Left;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            vx = 1;
            direction = Direction.Right;
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            vy = 1;
            direction = Direction.Up;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            vy = -1;
            direction = Direction.Down;
        }
        
        Vector2 v = new Vector2(vx, vy);
        orientation = v;
        v = v.normalized;
        float newX = FitToBounds(transform.position.x + v.x * movSpeed * Time.deltaTime, -screenMarginLimitX, screenMarginLimitX);
        float newY = FitToBounds(transform.position.y + v.y * movSpeed * Time.deltaTime, -screenMarginLimitY, screenMarginLimitY);
        transform.position = new Vector3(newX, newY, transform.position.z);
        animator.SetInteger("KeyPressed", Convert.ToInt32(direction));
    }

    private float FitToBounds(float value, float min, float max) {
        if (value > max) {
            return max;
        }

        if (value < min) {
            return min;
        }

        return value;
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

    private void SpawnZombie()
    {
        var zombie = PoolManager.Instance.GetPool("Zombie").GetItem();
        if(zombie != null)
        {
            zombie.gameObject.SetActive(true);
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
                
                Die();
                gameObject.GetComponent<BoxCollider2D>().enabled = false;
            }
        }
    }

    public void OnTriggerStay2D(Collider2D col)
    {

    }

    public void OnTriggerExit2D(Collider2D col)
    {

    }
}
