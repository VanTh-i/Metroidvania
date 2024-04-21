using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected Rigidbody2D rb;
    protected Player player;

    [Header("Stats")]
    [SerializeField] protected float health;
    [SerializeField] protected int damage;
    [SerializeField] protected float speed;
    private float currentSpeed;

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
    public virtual void EnemyTakeDamage(float damage, Vector2 hitDirection, float hitForce)
    {
        health -= damage;
        StartCoroutine(OnHit());
        if (!isRecoiling)
        {
            rb.AddForce(-hitForce * recoilFactor * hitDirection);
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
