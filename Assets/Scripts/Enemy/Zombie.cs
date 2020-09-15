using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : Character
{
    public const string Tag = "Enemy";
    public float dyingTime = 0.5f;
    public GameObject player; 

    private float accumTime;
    private float startX = -0.67f, startY = 3.49f;

    void Awake()
    {
        state = State.Alive;
        direction = Direction.Down;
        transform.position = new Vector2(startX, startY);
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

    private void Die()
    {
        state = State.Dying;
        animator.SetInteger("Dead", Convert.ToInt32(direction));
    }

    private void Destroy()
    {
        ReturnToPool();
    }
}
