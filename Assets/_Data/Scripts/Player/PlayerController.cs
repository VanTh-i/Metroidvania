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
    protected internal int doubleJumpCounter = 0;

    [Header("Dash")]
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashTime;
    [SerializeField] private float dashCooldown;
    protected internal float gravity;
    private bool dashInput;
    private bool canDash = true;
    private bool dashed;

    [Header("Wall Slide Jump")]
    [SerializeField] private float wallSlidingSpeed;
    [SerializeField] private float wallCheckX;
    [SerializeField] private Transform wallCheckPoint;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private float wallJumpingDuration;
    private bool wallJumping;

    private void Start()
    {
        rb = GetComponentInParent<Rigidbody2D>();
        gravity = rb.gravityScale;
        playerState = transform.parent.GetComponentInChildren<PlayerStateList>();
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
        moveInput = InputManager.Instance.MoveInputHorizontal;
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
            playerState.LookingRight = false;
        }
        else if (moveInput > 0)
        {
            transform.parent.localScale = new Vector2(1, transform.parent.localScale.y);
            playerState.LookingRight = true;
        }
    }

    public void Jump()
    {
        JumpMechanics();

        if (jumpInputUp && rb.velocity.y > 0) //khi nha nut jump
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
        }

        //if (!playerState.IsInAir && coyoteTimeCounter > 0 && jumpBufferCounter > 0)
        if (coyoteTimeCounter > 0 && jumpBufferCounter > 0)
        {
            coyoteTimeCounter = 0;
            rb.velocity = new Vector3(rb.velocity.x, jumpForce);
            playerState.IsInAir = true;
        }

        DoubleJump();
    }

    private void DoubleJump()
    {
        if (coyoteTimeCounter < 0 && !Grounded() && jumpInputDown && doubleJumpCounter < doubleJump
            && playerState.CanDoubleJumpSkill) // double jump
        {
            doubleJumpCounter++;
            rb.velocity = new Vector3(rb.velocity.x, jumpForce);
            playerState.IsInAir = true;
        }
    }

    public void WallSlide() // khi dang truot tuong
    {
        if (IsWallSlide())
        {
            playerState.IsWallSliding = true;
            playerState.IsInAir = false;
            doubleJumpCounter = 0;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }
        else
        {
            playerState.IsWallSliding = false;
        }
    }
    protected internal bool IsWallSlide() // co dang truot tuong hay khong
    {
        if (IsWalled() && !Grounded() && moveInput != 0f && playerState.CanWallSliding)
        {
            return true;
        }
        else return false;
    }

    public void WallJump()
    {
        if (wallJumping)
        {
            rb.velocity = new Vector2(-moveInput * jumpForce / 1.5f, jumpForce);
        }
    }
    public void StartWallJump()
    {
        if (jumpInputDown && playerState.IsWallSliding)
        {
            wallJumping = true;
            Invoke(nameof(StopWallJump), wallJumpingDuration);
        }
    }
    private void StopWallJump()
    {
        wallJumping = false;
    }

    private bool IsWalled() // kiem tra player co cham vao layer tuong hay ko
    {
        return Physics2D.OverlapCircle(wallCheckPoint.position, wallCheckX, wallLayer);
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
            coyoteTimeCounter -= Time.deltaTime;
        }

        if (jumpInputDown)
        {
            jumpBufferCounter = jumpBufferFrame;
        }
        else
        {
            jumpBufferCounter--;
        }

        if (!Grounded() && rb.velocity.y != 0)
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
    public void StartDash()
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

    public IEnumerator WalkIntoNewScene(Vector2 exitDir, float delay)
    {
        yield return new WaitForEndOfFrame();
        transform.parent.localScale = new Vector2(Mathf.Sign(exitDir.x), transform.parent.localScale.y);

        if (exitDir.y > 0)
        {
            float elapsedTime = 0f;
            while (elapsedTime < delay)
            {
                rb.velocity = 5 * exitDir;
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            while (elapsedTime < delay + 1f && !Grounded())
            {
                rb.velocity = moveSpeed * new Vector2(exitDir.x, -0.5f);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

        }
        else if (exitDir.x != 0)
        {
            float elapsedTime = 0f;
            while (elapsedTime < delay)
            {
                rb.velocity = exitDir * moveSpeed;
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }

        yield return new WaitForSeconds(delay);
        playerState.CutScene = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(groundCheckPoint.position, groundCheckX);
        Gizmos.DrawWireSphere(wallCheckPoint.position, wallCheckX);
    }
}
