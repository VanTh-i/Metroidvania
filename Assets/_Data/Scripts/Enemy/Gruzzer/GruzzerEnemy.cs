using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GruzzerEnemy : Enemy
{
    [Header("Chase")]
    [SerializeField] protected float chaseDistance;
    protected enum EnemyState
    {
        Gruzzer_Idle,
        Gruzzer_Chase,
        Gruzzer_Death,
    }
    protected EnemyState currentEnemyState;

    protected override void Start()
    {
        base.Start();
        ChangeState(EnemyState.Gruzzer_Idle);
    }

    protected override void Death()
    {
        base.Death();
    }

    protected void ChangeState(EnemyState newState)
    {
        currentEnemyState = newState;
    }

    protected override void UpdateEnemyState()
    {
        float distance = Vector2.Distance(transform.position, player.transform.position);

        switch (currentEnemyState)
        {
            case EnemyState.Gruzzer_Idle:
                rb.velocity = Vector2.zero;
                if (distance < chaseDistance)
                {
                    ChangeState(EnemyState.Gruzzer_Chase);

                }
                break;

            case EnemyState.Gruzzer_Chase:
                rb.MovePosition(Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime));
                Flip();
                if (distance > chaseDistance)
                {
                    ChangeState(EnemyState.Gruzzer_Idle);
                }
                break;

            case EnemyState.Gruzzer_Death:
                break;
        }
    }
    protected void Flip()
    {
        sr.flipX = player.transform.position.x > transform.position.x;
    }
}
