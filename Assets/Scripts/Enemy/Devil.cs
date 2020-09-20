using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Devil : Character
{
    public const string Tag = "Enemy";
    public GameObject player;
    public Transform startPosition;

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

    // Update is called once per frame
    void Update()
    {

    }

    protected override Vector3 GetHeading()
    {
        return new Vector3(0, 0, 0);
    }

    protected override void Attack()
    {

    }

    protected override void Destroy()
    {
        ReturnToPool();
        EnemyManager.EnemyDied();
    }
}
