using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    protected internal Rigidbody2D rb;
    protected internal PlayerStateList playerState;
    private PlayerAnimation playerAnimation;

    [Header("Move")]
    [SerializeField] private float moveSpeed;
    private float moveInput;

    [Header("Jump")]
    [SerializeField] private float jumpForce;
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private float groundCheckY;
    [SerializeField] private float groundCheckX;
    [SerializeField] private LayerMask groundLayer;
    private bool jumpInputDown;
    private bool jumpInputUp;

    [Header("Jump Mechanic")]
    [SerializeField] private int jumpBufferFrame;
    private int jumpBufferCounter = 0;
    [SerializeField] private float coyoteTime;
    private float coyoteTimeCounter = 0f;

    [Header("Double Jump")]
    [SerializeField, Tooltip("so lan co the nhay them")] private int doubleJump;
    private int doubleJumpCounter = 0;

    [Header("Dash")]
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashTime;
    [SerializeField] private float dashCooldown;
    private float gravity;
    private bool dashInput;
    private bool canDash = true;
    private bool dashed;

    [Header("Wall Slide Jump")]
    [SerializeField] private float wallSlidingSpeed;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;
    private float wallJumpingDirection;
    private float wallJumpingTime = 0.2f;
    private float wallJumpingCounter;

    private void Start()
    {
        rb = GetComponentInParent<Rigidbody2D>();
        gravity = rb.gravityScale;
        playerState = GetComponent<PlayerStateList>();
        playerAnimation = transform.parent.GetComponentInChildren<PlayerAnimation>();
    }
    private void Update()
    {
        GetInput();
    }

    private void GetInput()
    {
        jumpInputDown = InputManager.Instance.JumpInputDown;
        jumpInputUp = InputManager.Instance.JumpInputUp;
        moveInput = InputManager.Instance.MoveInput;
        dashInput = InputManager.Instance.DashInput;
    }

    public void Moving()
    {
        Flip();
        rb.velocity = new Vector2(moveSpeed * moveInput, rb.velocity.y);
    }

    private void Flip()
    {
        if (moveInput < 0)
        {
            transform.parent.localScale = new Vector2(-1, transform.parent.localScale.y);
        }
        else if (moveInput > 0)
        {
            transform.parent.localScale = new Vector2(1, transform.parent.localScale.y);
        }
    }

    public void Jump()
    {
        JumpMechanics();

        if (jumpInputUp && rb.velocity.y > 0) //khi nha nut jump
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
        }

        if (!playerState.IsInAir && coyoteTimeCounter > 0f && jumpBufferCounter > 0) //khi an nut jump
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce);
            playerState.IsInAir = true;
        }

        if (playerState.IsInAir && !Grounded() && jumpInputDown && doubleJumpCounter < doubleJump
            && playerState.CanDoubleJumpSkill) // double jump
        {
            doubleJumpCounter++;
            rb.velocity = new Vector3(rb.velocity.x, jumpForce);
            playerState.IsInAir = true;
        }

    }

    protected internal bool Grounded() //check co dang dung tren mat dat hay khong
    {
        if (Physics2D.Raycast(groundCheckPoint.position, Vector2.down, groundCheckY, groundLayer)
            || Physics2D.Raycast(groundCheckPoint.position + new Vector3(groundCheckX, 0, 0), Vector2.down, groundCheckY, groundLayer)
            || Physics2D.Raycast(groundCheckPoint.position + new Vector3(-groundCheckX, 0, 0), Vector2.down, groundCheckY, groundLayer)
            )
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void WallSlide()
    {
        if (IsWallSlide())
        {
            playerState.IsWallSliding = true;
            playerState.IsInAir = false;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }
        else
        {
            playerState.IsWallSliding = false;
        }
    }

    protected internal bool IsWallSlide()
    {
        if (IsWalled() && !Grounded() && moveInput != 0f && playerState.CanWallSliding && !jumpInputDown)
        {
            return true;
        }
        else return false;
    }

    public void WallJump()
    {
        if (playerState.IsWallSliding)
        {
            wallJumpingDirection = -transform.localScale.x;
            wallJumpingCounter = wallJumpingTime;

        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }

        if (jumpInputDown && wallJumpingCounter > 0f)
        {
            playerState.IsInAir = true;
            playerState.IsWallSliding = false;
            doubleJumpCounter = 0;
            rb.velocity = new Vector2(wallJumpingDirection * jumpForce, jumpForce);
            wallJumpingCounter = 0f;

            if (transform.localScale.x != wallJumpingDirection)
            {
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }

        }
    }

    private bool IsWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }

    private void JumpMechanics() //Jump Buffer and Coyote Time
    {
        if (Grounded())
        {
            playerState.IsInAir = false;
            coyoteTimeCounter = coyoteTime;
            doubleJumpCounter = 0;
        }
        else
        {
            if (coyoteTimeCounter >= 0)
            {
                coyoteTimeCounter -= Time.deltaTime;
            }
        }

        if (jumpInputDown)
        {
            jumpBufferCounter = jumpBufferFrame;
        }
        else
        {
            if (jumpBufferCounter >= 0)
            {
                jumpBufferCounter--;
            }
        }

        if (!Grounded() && rb.velocity.y < 0)
        {
            playerState.IsInAir = true;
        }
    }

    private IEnumerator Dash()
    {
        canDash = false;
        playerState.IsDashing = true;
        playerAnimation.DashAnimation();
        rb.gravityScale = 0f;
        if (IsWalled() && playerState.CanWallSliding) //dash nguoc huong dang bam tuong
        {
            rb.velocity = new Vector2(-transform.parent.localScale.x * dashSpeed, 0);
            transform.parent.localScale = new Vector2(-transform.parent.localScale.x, transform.parent.localScale.y);
        }
        else
        {
            rb.velocity = new Vector2(transform.parent.localScale.x * dashSpeed, 0);
        }

        yield return new WaitForSeconds(dashTime);
        rb.gravityScale = gravity;
        playerState.IsDashing = false;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;

    }

    public void StatDash()
    {
        if (dashInput && canDash && !dashed && playerState.CanDashSkill)
        {
            StartCoroutine(Dash());
            dashed = true;
        }

        if (Grounded() || playerState.IsWallSliding)
        {
            dashed = false;
        }
    }
}
