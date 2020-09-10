using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Projectile : PoolItem
{
    public float movSpeed = 10f;
    private bool movesLeft = false;
    private bool movesRight = false;
    private bool movesUp = false;
    private bool movesDown = false;
    private float screenMarginLimitX;
    private float screenMarginLimitY;

    void Awake()
    {
        screenMarginLimitX = Camera.main.orthographicSize * 0.9f * Camera.main.aspect;
        screenMarginLimitY = Camera.main.orthographicSize * 0.85f;
    }

    void Update()
    {
        var x = transform.position.x;
        var y = transform.position.y;
        if (movesLeft)
            x -= movSpeed * Time.deltaTime;
        else if (movesRight)
            x += movSpeed * Time.deltaTime;
        if (movesUp)
            y += movSpeed * Time.deltaTime;
        else if (movesDown)
            y -= movSpeed * Time.deltaTime;

        if(!movesLeft && !movesRight && !movesUp && !movesDown)
            y += movSpeed * Time.deltaTime;

        if (x > -screenMarginLimitX && x < screenMarginLimitX && y > -screenMarginLimitY && y < screenMarginLimitY)
            transform.position = new Vector3(x, y, transform.position.z);
        else
            Destroy();
    }

    void OnEnable()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
            movesLeft = true;
        else if (Input.GetKey(KeyCode.RightArrow))
            movesRight = true;
        if (Input.GetKey(KeyCode.UpArrow))
            movesUp = true;
        else if (Input.GetKey(KeyCode.DownArrow))
            movesDown = true;
    }

    void OnDisable()
    {
        movesLeft = false;
        movesRight = false;
        movesUp = false;
        movesDown = false;
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        ReturnToPool();
    }

    private void Destroy()
    {
        ReturnToPool();
    }
}
