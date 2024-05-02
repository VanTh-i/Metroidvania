using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseEnemy : Enemy
{
    protected override void Update()
    {
        base.Update();
    }
    protected override void UpdateEnemyState()
    {
        base.UpdateEnemyState();
        if (Vector3.Distance(player.transform.position, transform.position) <= 4f)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(player.transform.position.x, player.transform.position.y),
                speed * Time.deltaTime);
        }
    }
}
