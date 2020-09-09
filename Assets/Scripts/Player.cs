using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float movSpeed = 5f;
    private float screenMarginLimitX;
    private float screenMarginLimitY;
    public GameObject projectilePrefab;
    public Transform projectileStartPointLeft;
    public Transform projectileStartPointRight;
    public Transform projectileStartPointUp;
    public Transform projectileStartPointDown;
    public Animator animator;
    private static Direction direction;

    // Start is called before the first frame update
    void Start()
    {
        screenMarginLimitX = Camera.main.orthographicSize * 0.9f * Camera.main.aspect;
        screenMarginLimitY = Camera.main.orthographicSize * 0.85f;
        direction = Direction.Up;
    }

    // Update is called once per frame
    void Update()
    {
        //Movimiento horizontal
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            direction = Direction.Left;
            animator.SetInteger("KeyPressed", Convert.ToInt32(direction));            
            var x = Math.Max(transform.position.x - movSpeed * Time.deltaTime, -screenMarginLimitX);
            transform.position = new Vector3(x, transform.position.y, transform.position.z);
            UnityEngine.Debug.Log("Left = " + KeyCode.LeftArrow);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            direction = Direction.Right;
            animator.SetInteger("KeyPressed", Convert.ToInt32(direction));
            var x = Math.Min(transform.position.x + movSpeed * Time.deltaTime, screenMarginLimitX);
            transform.position = new Vector3(x, transform.position.y, transform.position.z);
            UnityEngine.Debug.Log("Right = " + KeyCode.RightArrow);
        }

        //Movimiento vertical
        if (Input.GetKey(KeyCode.UpArrow))
        {
            direction = Direction.Up;
            animator.SetInteger("KeyPressed", Convert.ToInt32(direction));
            var y = Math.Min(transform.position.y + movSpeed * Time.deltaTime, screenMarginLimitY);
            transform.position = new Vector3(transform.position.x, y, transform.position.z);
            UnityEngine.Debug.Log("Up = " + KeyCode.UpArrow);
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            direction = Direction.Down;
            animator.SetInteger("KeyPressed", Convert.ToInt32(direction));
            var y = Math.Max(transform.position.y - movSpeed * Time.deltaTime, -screenMarginLimitY);
            transform.position = new Vector3(transform.position.x, y, transform.position.z);
            UnityEngine.Debug.Log("Down = " + KeyCode.DownArrow);
        }

        //Disparos
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot(direction);
        }
    }

    private void Shoot(Direction direction)
    {
        switch (direction)
        {
            case Direction.Left:
                GameObject.Instantiate(projectilePrefab, projectileStartPointLeft.position, Quaternion.identity);
                break;
            case Direction.Right:
                GameObject.Instantiate(projectilePrefab, projectileStartPointRight.position, Quaternion.identity);
                break;
            case Direction.Up:
                GameObject.Instantiate(projectilePrefab, projectileStartPointUp.position, Quaternion.identity);
                break;
            case Direction.Down:
                GameObject.Instantiate(projectilePrefab, projectileStartPointDown.position, Quaternion.identity);
                break;
            default:
                break;
        }
        
    }

    enum Direction
    {
        Left = 0,
        Right = 1,
        Up = 2,
        Down = 3
    }
}
