using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : PoolItem
{
    public float movSpeed;
    public Animator animator;  

    protected Direction direction;
    protected State state;
    protected Vector3 orientation;
    public Rigidbody2D rigidbody;

    protected float health = 100f;

    public enum Direction
    {
        Left = 0,
        Right = 1,
        Up = 2,
        Down = 3
    }

    public enum State
    {
        Alive,
        Dying,
        Dead
    }

    void Update()
    {
        switch (state)
        {
            case State.Alive:
                Vector3 heading = GetHeading();
                SetDirection(heading);
                rigidbody.velocity = heading * movSpeed;

                Attack();
                break;
            case State.Dying:
                break;
            case State.Dead:
                break;

        }
    }

    private void SetDirection(Vector3 heading) {
        if (heading.sqrMagnitude == 0) {
            return;
        }

        if(Mathf.Abs(heading.x) > Mathf.Abs(heading.y))
        {
            direction = heading.x > 0? Direction.Right : Direction.Left;
        } else
        {
            direction = heading.y > 0? Direction.Up : Direction.Down;
        }
        animator.SetInteger("Direction", Convert.ToInt32(direction));
    }

    private void Die() {

    }

    protected abstract Vector3 GetHeading();

    protected abstract void Attack();

    public void HandleHit(Projectile projectile) {
        health -= projectile.Damage;

        if (health <= 0f) 
        {
            Die();
        }
    }
}