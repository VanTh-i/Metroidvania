using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected Rigidbody2D rb;
    protected Player player;
    protected SpriteRenderer sr;

    [Header("Stats")]
    [SerializeField] protected float health;
    [SerializeField] protected int damage;
    [SerializeField] protected float speed;
    protected float currentSpeed;

    [Tooltip("Giật Lùi"), Header("Recoil")]
    [SerializeField] protected float recoilLength;
    protected float recoilTimer;
    [SerializeField] protected float recoilFactor;
    protected bool isRecoiling = false;



    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        player = FindObjectOfType<Player>();

        currentSpeed = speed;
    }
    protected virtual void Update()
    {
        //UpdateEnemyState();
        Death();
        Recoiling();
    }

    protected virtual void UpdateEnemyState()
    {
        // for override
    }

    protected virtual void Death()
    {
        if (health <= 0)
        {
            Destroy(gameObject, 1f);
            // TODO: object pool late
        }
    }
    public virtual void EnemyTakeDamage(float damage, Vector2 hitDirection)
    {
        isRecoiling = true;
        health -= damage;
        StartCoroutine(OnHit());
        if (isRecoiling)
        {
            rb.velocity = hitDirection * recoilFactor;
        }
    }
    protected IEnumerator OnHit()
    {
        speed = 0;
        yield return new WaitForSeconds(0.5f);
        speed = currentSpeed;
    }

    #region Hit player
    protected void OnCollisionStay2D(Collision2D other) // cham phai nguoi choi
    {
        if (other.gameObject.CompareTag("Player") && !player.GetComponentInChildren<PlayerStateList>().Invincible)
        {
            other.gameObject.GetComponentInChildren<PlayerHealth>().HitStopTime(0.05f, 5, 0.2f);
            other.gameObject.GetComponentInChildren<PlayerHealth>().TakeDamage(damage);
            //HitPlayer();
        }
    }
    protected void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        }

    }
    protected void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }

    }
    #endregion

    protected void Recoiling()
    {
        if (isRecoiling)
        {
            if (recoilTimer <= recoilLength)
            {
                recoilTimer += Time.deltaTime;
            }
            else
            {
                isRecoiling = false;
                recoilTimer = 0;
            }
        }
        else
        {
            UpdateEnemyState();
        }
    }
}
