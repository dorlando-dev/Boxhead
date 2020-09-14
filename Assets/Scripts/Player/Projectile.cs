using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Projectile : PoolItem
{
    public float movSpeed = 10f;
    private float screenMarginLimitX;
    private float screenMarginLimitY;
    private Vector2 bearing;
    private bool fromPlayer;

    public Vector2 Bearing { get => bearing; set => bearing = value; }
    public bool FromPlayer { get => fromPlayer; set => fromPlayer = value; }

    void Awake()
    {
        screenMarginLimitX = Camera.main.orthographicSize * 0.9f * Camera.main.aspect;
        screenMarginLimitY = Camera.main.orthographicSize * 0.85f;
    }

    void Update()
    {
        var x = transform.position.x;
        var y = transform.position.y;
        x += bearing.x * movSpeed * Time.deltaTime;
        y += bearing.y * movSpeed * Time.deltaTime;

        if (x > -screenMarginLimitX && x < screenMarginLimitX && y > -screenMarginLimitY && y < screenMarginLimitY)
            transform.position = new Vector3(x, y, transform.position.z);
        else
            Destroy();
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        if((fromPlayer && (string.Compare(col.gameObject.tag, "Enemy") == 0)) || (!fromPlayer && (string.Compare(col.gameObject.tag, "Player") == 0)))
            ReturnToPool();
    }

    private void Destroy()
    {
        ReturnToPool();
    }
}
