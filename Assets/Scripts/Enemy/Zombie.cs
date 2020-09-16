using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : Character
{
    public const string Tag = "Enemy";
    public GameObject player;
    public Transform startPosition;

    void Awake()
    {
        state = State.Alive;
        direction = Direction.Down;
    }

    void Start()
    {
        transform.position = new Vector2(startPosition.position.x, startPosition.position.y);
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

    protected override Vector3 GetHeading() {
        if (player == null) {
            SetPlayer();
        }

        Vector3 heading = player.transform.position - transform.position;
        orientation = heading;
        return heading.normalized;
    }

    protected override void Attack() {

    }

    private void SetPlayer() {
        player = GameObject.FindGameObjectsWithTag("Player")[0];
    }

    protected override void Destroy()
    {
        ReturnToPool();
    }
}
