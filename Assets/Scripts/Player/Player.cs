using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using UnityEngine;
using FrameLord.Pool;

public class Player : Character
{
    public const string Tag = "Player";

    public Transform projectileStartPointLeft;
    public Transform projectileStartPointRight;
    public Transform projectileStartPointUp;
    public Transform projectileStartPointDown;

    public AudioSource audioClipShoot;
    public AudioClip audioClipDeath;

    private KeyCode[] movementKeyCodes = new KeyCode[]
 {
         KeyCode.LeftArrow,
         KeyCode.RightArrow,
         KeyCode.DownArrow,
         KeyCode.UpArrow,
         KeyCode.D
 };

    void Awake()
    {
        state = State.Alive;
        direction = Direction.Up;
        // Start facing up 
        orientation = new Vector2(0, 1);
    }

    protected override Vector3 GetHeading()
    {
        if(!AnyKeyPressed(movementKeyCodes)) 
        {
            return new Vector3(0,0,0);
        }

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

        if (Input.GetKeyDown(KeyCode.D))
        {
            Destroy();
        }

        Vector3 v = new Vector3(vx, vy, 0);
        orientation = v;
        return v.normalized;
    }

    protected override void Attack() {
        if (!Input.GetKeyDown(KeyCode.Space))
        {
            return;
        }

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
            proj.Shooter = this;
            proj.Hit = false;
            projectile.gameObject.SetActive(true);

            audioClipShoot.Play();            
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

    public void Respawn()
    {

    }

    protected override void Destroy()
    {
        AudioSource.PlayClipAtPoint(audioClipDeath, transform.position);
        gameObject.SetActive(false);
        GameManager.Instance.NotifyPlayerDeath();
    }
}
