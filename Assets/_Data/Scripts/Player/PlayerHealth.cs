using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    Rigidbody2D rb;
    private SpriteRenderer sr;
    private PlayerStateList playerState;
    private PlayerAnimation playerAnimation;
    private PlayerMana playerMana;

    public int health;
    public int maxHealth;
    public delegate void OnHealthChangedDelegate();
    [HideInInspector] public OnHealthChangedDelegate onHealthChangedCallback;

    protected internal bool restoreTime;
    private float restoreTimeSpeed;
    [SerializeField] private float invincibleTime;

    [Header("Healing")]
    [SerializeField] private float timeToHeal;
    private float healTimer;

    [Header("Knock Back")]
    [SerializeField] private float KBForce;
    private float KBCounter;
    [SerializeField] private float KBTotalTime;


    private void Start()
    {
        rb = GetComponentInParent<Rigidbody2D>();
        sr = GetComponentInParent<SpriteRenderer>();
        playerState = transform.parent.GetComponentInChildren<PlayerStateList>();
        playerAnimation = transform.parent.GetComponentInChildren<PlayerAnimation>();
        playerMana = GetComponent<PlayerMana>();

        Health = maxHealth;
        KBCounter = 0;
    }
    private void Update()
    {
        RestoreTimeScale();
        InvincibleFlash();
        KnockBack();
    }

    public int Health
    {
        get { return health; }
        set
        {
            if (health != value)
            {
                health = Mathf.Clamp(value, 0, maxHealth);

                if (onHealthChangedCallback != null)
                {
                    onHealthChangedCallback.Invoke();
                }
            }
        }
    }

    public void Heal()
    {
        if (InputManager.Instance.HealInput && Health < maxHealth && playerMana.Mana > 0 && !playerState.IsInAir && !playerState.IsDashing)
        {
            playerState.Healing = true;

            playerAnimation.HealAnimation(true);

            healTimer += Time.deltaTime;
            if (healTimer >= timeToHeal)
            {
                Health++;
                healTimer = 0;
            }

            playerMana.Mana -= Time.deltaTime * playerMana.manaDrainSpeed;
        }
        else
        {
            playerState.Healing = false;
            playerAnimation.HealAnimation(false);
            healTimer = 0;
        }
    }

    public void TakeDamage(float damage)
    {

        Health -= Mathf.RoundToInt(damage);
        StartCoroutine(StopTakingDamage());

        Debug.Log("- " + damage);
    }
    private IEnumerator StopTakingDamage()
    {
        playerState.Invincible = true;
        playerAnimation.HurtAnimation();
        KBCounter = KBTotalTime;
        yield return new WaitForSeconds(invincibleTime);
        playerState.Invincible = false;


    }
    private void KnockBack()
    {
        if (KBCounter > 0)
        {
            if (playerState.LookingRight)
            {
                rb.velocity = new Vector2(-KBForce, KBForce / 4);
            }
            else
            {
                rb.velocity = new Vector2(KBForce, KBForce / 4);
            }
            KBCounter -= Time.deltaTime;
        }
    }

    private void InvincibleFlash()
    {
        sr.material.color = playerState.Invincible ?
            Color.Lerp(Color.white, Color.red, Mathf.PingPong(Time.time * invincibleTime, 0.3f)) : Color.white;
    }

    private void RestoreTimeScale()
    {
        if (restoreTime)
        {
            if (Time.timeScale < 1)
            {
                Time.timeScale += Time.deltaTime * restoreTimeSpeed;
            }
            else
            {
                Time.timeScale = 1;
                restoreTime = false;
            }

        }
    }

    public void HitStopTime(float newTimeScale, int restoreSpeed, float delay)
    {

        restoreTimeSpeed = restoreSpeed;
        Time.timeScale = newTimeScale;

        StartCoroutine(StartTimeAgain(delay));
    }
    private IEnumerator StartTimeAgain(float delay)
    {
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(delay);

        Time.timeScale = 1f;
        restoreTime = true;
    }
}
