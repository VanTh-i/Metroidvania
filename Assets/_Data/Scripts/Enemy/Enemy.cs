using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected Rigidbody2D rb;
    protected Player player;

    [Header("Stats")]
    [SerializeField] protected int damage;
    [SerializeField] protected float speed;
    private float currentSpeed;
    [SerializeField] protected float health;

    [Tooltip("Giật Lùi"), Header("Recoil")]
    [SerializeField] protected float recoilLength;
    protected float recoilTimer;
    [SerializeField] protected float recoilFactor;
    protected bool isRecoiling = false;

    protected virtual void Start()
    {
        rb = GetComponentInParent<Rigidbody2D>();
        player = FindObjectOfType<Player>();

        currentSpeed = speed;
    }
    protected virtual void Update()
    {
        Death();
        Recoiling();
    }
    protected void Death()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
            //object pool late
        }
    }
    public virtual void EnemyHit(float damage, Vector2 hitDirection, float hitForce)
    {
        health -= damage;
        StartCoroutine(OnHit());
        if (!isRecoiling)
        {
            rb.AddForce(-hitForce * recoilFactor * hitDirection);
        }
    }

    protected void HitPlayer()
    {
        player.GetComponentInChildren<PlayerHealth>().TakeDamage(damage);
    }
    protected void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player") && !player.GetComponentInChildren<PlayerStateList>().Invincible)
        {
            other.gameObject.GetComponentInChildren<PlayerHealth>().HitStopTime(0.05f, 5, 0.2f);
            HitPlayer();
        }
    }

    protected IEnumerator OnHit()
    {
        speed = 0;
        yield return new WaitForSeconds(0.5f);
        speed = currentSpeed;
    }

    protected void Recoiling()
    {
        if (isRecoiling)
        {
            if (recoilTimer < recoilLength)
            {
                if (recoilTimer <= recoilLength)
                {
                    recoilTimer += Time.deltaTime;
                }
            }
            else
            {
                isRecoiling = true;
                recoilTimer = 0;
            }
        }
    }
}
