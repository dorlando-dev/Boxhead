using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using FrameLord.Pool;

public class Devil : Character
{
    public const string Tag = "Enemy";
    public GameObject player;
    public Transform startPosition;
    public AudioClip audioClipDeath;

    private int attackCooldown;

    // Start is called before the first frame update
    void Awake()
    {
        state = State.Alive;
        direction = Direction.Down;
    }

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

        return new Vector3(0,0,0);
    }

    private void SetPlayer() {
        var go = GameObject.FindGameObjectsWithTag("Player");
        if(go.Length > 0)
            player = go[0];
    }

    protected override void Attack()
    {
        if (attackCooldown > 0) {
            attackCooldown--;
            return;
        }

        var projectile = PoolManager.Instance.GetPool("EnemyProjectile").GetItem();
        if (projectile != null)
        {
            Projectile proj = projectile.gameObject.GetComponent<Projectile>();
            projectile.transform.position = transform.position;
            proj.Bearing = orientation;
            proj.Shooter = this;
            proj.Hit = false;
            projectile.gameObject.SetActive(true);
            attackCooldown = 500;
        }
    }

    protected override void Destroy()
    {
        AudioSource.PlayClipAtPoint(audioClipDeath, transform.position);
        ReturnToPool();
        EnemyManager.DevilDied();
    }
}
