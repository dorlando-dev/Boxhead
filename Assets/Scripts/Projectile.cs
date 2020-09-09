using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float movSpeed = 10f;
    private bool movesLeft = false;
    private bool movesRight = false;
    private bool movesUp = false;
    private bool movesDown = false;
    private float screenMarginLimitX;
    private float screenMarginLimitY;

    // Start is called before the first frame update
    void Start()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
            movesLeft = true;
        else if (Input.GetKey(KeyCode.RightArrow))
            movesRight = true;
        if (Input.GetKey(KeyCode.UpArrow))
            movesUp = true;
        else if (Input.GetKey(KeyCode.DownArrow))
            movesDown = true;

        screenMarginLimitX = Camera.main.orthographicSize * 0.9f * Camera.main.aspect;
        screenMarginLimitY = Camera.main.orthographicSize * 0.85f;
    }

    // Update is called once per frame
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

    private void Destroy()
    {
        GameObject.Destroy(this.gameObject);
    }
}
