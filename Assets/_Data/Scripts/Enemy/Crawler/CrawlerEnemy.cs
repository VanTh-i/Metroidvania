using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrawlerEnemy : Enemy
{
    protected CrawlidAnimation crawlidAnim;

    [Header("Patrol")]
    [SerializeField] private float flipTime;
    [SerializeField] private float ledgeCheckX;
    [SerializeField] private float ledgeCheckY;
    [SerializeField] private LayerMask groundLayer;
    private float timer;

    protected enum EnemyState
    {
        Crawler_Idle,
        Crawler_Flip
    }
    protected EnemyState currentEnemyState;

    protected override void Start()
    {
        base.Start();
        crawlidAnim = GetComponent<CrawlidAnimation>();

    }
    protected override void Death()
    {
        base.Death();
        if (health <= 0)
        {
            crawlidAnim.DeathAnimation();
            rb.velocity = Vector2.zero;
        }
    }

    protected void ChangeState(EnemyState newState)
    {
        currentEnemyState = newState;
    }

    protected override void UpdateEnemyState()
    {
        switch (currentEnemyState)
        {
            case EnemyState.Crawler_Idle:
                Vector3 ledgeCheckStart = transform.localScale.x > 0 ? new Vector3(ledgeCheckX, 0) : new Vector3(-ledgeCheckX, 0);
                Vector3 wallCheckDir = transform.localScale.x > 0 ? transform.right : -transform.right;

                if (!Physics2D.Raycast(transform.position + ledgeCheckStart, Vector2.down, ledgeCheckY, groundLayer)
                    || Physics2D.Raycast(transform.position, wallCheckDir, ledgeCheckX, groundLayer))
                {
                    ChangeState(EnemyState.Crawler_Flip);

                }

                if (transform.localScale.x > 0)
                {
                    rb.velocity = new Vector2(speed, rb.velocity.y);
                }
                else
                {
                    rb.velocity = new Vector2(-speed, rb.velocity.y);

                }
                break;

            case EnemyState.Crawler_Flip:
                timer += Time.deltaTime;
                if (timer > flipTime)
                {
                    timer = 0;
                    crawlidAnim.FlipAnimation();

                }
                break;
        }
    }
    public void WaitToFlipAnimation()
    {
        transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
        ChangeState(EnemyState.Crawler_Idle);
    }
}
