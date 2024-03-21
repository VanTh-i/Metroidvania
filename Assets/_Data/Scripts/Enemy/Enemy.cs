using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody2D rb;

    [SerializeField] private float health;

    [Tooltip("Giật Lùi"), Header("Recoil")]
    [SerializeField] private float recoilLength;
    private float recoilTimer;
    [SerializeField] private float recoilFactor;
    private bool isRecoiling = false;

    private void Start()
    {
        rb = GetComponentInParent<Rigidbody2D>();
    }
    private void Update()
    {
        Death();
        Recoiling();
    }
    private void Death()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
            //object pool late
        }
    }
    public void EnemyHit(float damage, Vector2 hitDirection, float hitForce)
    {
        health -= damage;
        if (!isRecoiling)
        {
            rb.AddForce(-hitForce * recoilFactor * hitDirection);
        }
    }
    private void Recoiling()
    {
        if (isRecoiling)
        {
            if (recoilTimer < recoilLength)
            {
                recoilTimer += Time.deltaTime;
            }
            else
            {
                isRecoiling = true;
                recoilTimer = 0;
            }
        }
    }
}
