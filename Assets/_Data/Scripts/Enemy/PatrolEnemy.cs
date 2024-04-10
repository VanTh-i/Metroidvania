using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolEnemy : Enemy
{
    public Transform[] patrolPoints;
    private int patrol = 0;

    protected override void Update()
    {
        base.Update();
        Patrol();
    }
    protected void Patrol()
    {
        if (patrol == 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, patrolPoints[0].position, speed * Time.deltaTime);
            if (Vector2.Distance(transform.position, patrolPoints[0].position) < 0.2f)
            {
                transform.localScale = new Vector2(-1, transform.localScale.y);
                patrol = 1;
            }
        }
        if (patrol == 1)
        {
            transform.position = Vector2.MoveTowards(transform.position, patrolPoints[1].position, speed * Time.deltaTime);
            if (Vector2.Distance(transform.position, patrolPoints[1].position) < 0.2f)
            {
                transform.localScale = new Vector2(1, transform.localScale.y);
                patrol = 0;
            }
        }
    }


}
