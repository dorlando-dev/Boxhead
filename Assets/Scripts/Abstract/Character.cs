using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameLord.Pool;

public abstract class Character : PoolItem
{
    public float movSpeed;
    public Animator animator;  

    protected Direction direction;
    protected State state;
    protected Vector3 orientation;
    public Rigidbody2D rigidBody;
    public float dyingTime = 0.5f;

    public float health = 100f;
    protected float accumTime;

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
                rigidBody.velocity = heading * movSpeed;

                Attack();
                break;
            case State.Dying:
                Destroy();
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

    protected void Die()
    {
        state = State.Dying;
        //animator.SetInteger("Dead", Convert.ToInt32(direction));
    }

    protected abstract Vector3 GetHeading();

    protected abstract void Attack();

    public void HandleHit(Projectile projectile) {
        health -= projectile.Damage;
        DecreseHealthAnimator();
        rigidBody.AddForce(projectile.Bearing * 500f);
        if (health <= 0f) 
        {
            Die();
        }
    }

    protected abstract void DecreseHealthAnimator();

    protected abstract void Destroy();

}