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

    protected override Vector3 GetHeading() {
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

    protected override void Attack() {

    }

    private void SetPlayer() {
        var go = GameObject.FindGameObjectsWithTag("Player");
        if(go.Length > 0)
            player = go[0];
    }

    protected override void Destroy()
    {
        ReturnToPool();
        EnemyManager.ZombieDied();
    }
}
