using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    private Rigidbody2D rb;
    private PlayerAnimation playerAnimation;
    private PlayerController playerController;
    private PlayerStateList playerState;
    private PlayerMana playerMana;

    [Header("Meele Attack")]
    [SerializeField] private float playerDamage;
    [SerializeField, Tooltip("Attack Speed")] private float timeBetweenAttack;
    [SerializeField] private GameObject slashFX;
    private float timeSinceAttack;
    private bool attack;
    private float moveInputY;

    [SerializeField] private Transform sideAttackPoint, upAttackPoint, downAttackPoint;
    [SerializeField] private float sideAttackArea, upAttackArea, downAttackArea;
    [SerializeField] private LayerMask attackAbleLayer;

    [Header("Recoil")]
    [SerializeField] private int recoilXSteps = 5;
    [SerializeField] private int recoilYSteps = 5;
    [SerializeField] private float recoilXSpeed;
    [SerializeField] private float recoilYSpeed;
    private int stepXRecoiled, stepYRecoiled;


    private void Start()
    {
        rb = transform.parent.GetComponent<Rigidbody2D>();
        playerAnimation = transform.parent.GetComponentInChildren<PlayerAnimation>();
        playerController = transform.parent.GetComponentInChildren<PlayerController>();
        playerState = transform.parent.GetComponentInChildren<PlayerStateList>();
        playerMana = transform.parent.GetComponentInChildren<PlayerMana>();
    }

    private void Update()
    {
        GetInput();
    }
    private void FixedUpdate()
    {
        Recoiling();
    }
    private void GetInput()
    {
        attack = InputManager.Instance.AttackInput;
        moveInputY = InputManager.Instance.MoveInputVertical;
    }

    public void Attack()
    {
        timeSinceAttack += Time.deltaTime;
        if (playerController.IsWallSlide()) return;

        if (attack && timeSinceAttack >= timeBetweenAttack)
        {
            Hit();

            timeSinceAttack = 0;
            playerAnimation.AttackAnimation();
        }
    }

    private void Hit()
    {
        if (moveInputY == 0 || moveInputY < 0 && playerController.Grounded())
        {
            SlashFXAngle(slashFX, 0, sideAttackPoint);
            DealingDamage(sideAttackPoint, sideAttackArea, ref playerState.RecoilingX, recoilXSpeed);
        }
        else if (moveInputY > 0)
        {
            SlashFXAngle(slashFX, 90, upAttackPoint);
            DealingDamage(upAttackPoint, upAttackArea, ref playerState.RecoilingY, recoilYSpeed);
        }
        else if (moveInputY < 0 && playerState.IsInAir)
        {
            SlashFXAngle(slashFX, -90, downAttackPoint);
            DealingDamage(downAttackPoint, downAttackArea, ref playerState.RecoilingY, recoilYSpeed);
        }
    }

    private void DealingDamage(Transform attackPoint, float attackArea, ref bool recoilDir, float recoilStrength)
    {
        Collider2D[] objectsToHit = Physics2D.OverlapCircleAll(attackPoint.position, attackArea, attackAbleLayer);

        foreach (Collider2D objects in objectsToHit)
        {
            if (objects.GetComponent<Enemy>() != null)
            {
                objects.GetComponent<Enemy>().EnemyTakeDamage(playerDamage,
                (transform.parent.position - objects.transform.position).normalized, recoilStrength);

                playerMana.Mana += 0.1f;
            }
        }

        if (objectsToHit.Length > 0)
        {
            recoilDir = true;
        }
    }

    private void SlashFXAngle(GameObject slashFX, int fXAngle, Transform attackPoint)
    {
        slashFX = Instantiate(slashFX, attackPoint);
        if (playerState.LookingRight)
        {
            slashFX.transform.eulerAngles = new Vector3(0, 0, fXAngle);
        }
        else
        {
            slashFX.transform.eulerAngles = new Vector3(0, 0, -fXAngle);
        }

        slashFX.transform.localScale = new Vector2(transform.localScale.x, transform.localScale.y);
    }

    private void Recoiling()
    {
        if (playerState.RecoilingX)
        {
            if (playerState.LookingRight)
            {
                rb.velocity = new Vector2(-recoilXSpeed, 0);
            }
            else
            {
                rb.velocity = new Vector2(recoilXSpeed, 0);
            }
        }

        if (playerState.RecoilingY)
        {
            rb.gravityScale = 0;
            if (moveInputY < 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, recoilYSpeed);
            }
            else
            {
                rb.velocity = new Vector2(rb.velocity.x, -recoilYSpeed);
            }
            playerController.doubleJumpCounter = 0;
        }
        else
        {
            rb.gravityScale = playerController.gravity;
        }

        if (playerState.RecoilingX && stepXRecoiled < recoilXSteps)
        {
            stepXRecoiled++;
        }
        else
        {
            StopRecoilX();
        }

        if (playerState.RecoilingY && stepYRecoiled < recoilYSteps)
        {
            stepYRecoiled++;
        }
        else
        {
            StopRecoilY();
        }

        if (playerController.Grounded())
        {
            StopRecoilY();
        }
    }

    private void StopRecoilX()
    {
        stepXRecoiled = 0;
        playerState.RecoilingX = false;
    }
    private void StopRecoilY()
    {
        stepYRecoiled = 0;
        playerState.RecoilingY = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(sideAttackPoint.position, sideAttackArea);
        Gizmos.DrawWireSphere(upAttackPoint.position, upAttackArea);
        Gizmos.DrawWireSphere(downAttackPoint.position, downAttackArea);
    }
}
