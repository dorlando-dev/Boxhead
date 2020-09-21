using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameLord.Pool;

public class Juggernaut : Character
{
    public const string Tag = "Enemy";
    public GameObject player;
    public Transform startPosition;
    public AudioClip audioClipDeath;

    void Awake()
    {
        state = State.Alive;
        direction = Direction.Down;
    }

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector2(startPosition.position.x, startPosition.position.y);
    }

    protected override Vector3 GetHeading()
    {
        if (player == null) {
            SetPlayer();
        }
        if (player != null)
        {
            Vector3 heading = player.transform.position - transform.position;
            orientation = heading;
            return heading.normalized;
        }

        return new Vector3(0, 0, 0);
    }

    protected override void Attack()
    {
        // handled by collision event.
    }

    private void SetPlayer() {
        var go = GameObject.FindGameObjectsWithTag("Player");
        if(go.Length > 0)
            player = go[0];
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player") 
        {
            var projectile = PoolManager.Instance.GetPool("Projectile").GetItem();
            if (projectile != null)
            {
                Projectile proj = projectile.gameObject.GetComponent<Projectile>();
                projectile.transform.position = collision.gameObject.transform.position;
                proj.Bearing = orientation;
                proj.Shooter = this;
                proj.Hit = false;
                projectile.gameObject.SetActive(true);
            }
        }
    }

    protected override void Destroy()
    {
        AudioSource.PlayClipAtPoint(audioClipDeath, transform.position);
        ReturnToPool();
        EnemyManager.JuggernautDied();
    }
}
