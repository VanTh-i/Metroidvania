using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator anim;
    private PlayerController playerController;
    private const string Run = "Run";
    private const string Jump = "Jump";
    private const string Falling = "Falling";
    private const string Dash = "Dash";
    private const string SideWall = "SideWall";
    private const string Attacking = "Attacking";
    private const string TakeDamage = "TakeDamage";
    private const string Healing = "Healing";
    private const string Idle = "Idle";

    private void Start()
    {
        anim = GetComponentInParent<Animator>();
        playerController = FindAnyObjectByType<PlayerController>();
    }

    private void Update()
    {
        RunAnimation();
        JumpAnimation();
        FallAnimation();
        WallSideAnimation();
    }

    public void IdleAnimation()
    {
        anim.SetBool(Idle, playerController.rb.velocity.x == 0 && playerController.rb.velocity.y == 0 && playerController.Grounded());
    }

    public void RunAnimation()
    {
        anim.SetBool(Run, playerController.rb.velocity.x != 0 && playerController.Grounded());
    }

    public void JumpAnimation()
    {
        anim.SetBool(Jump, playerController.rb.velocity.y > 0 && !playerController.Grounded());
    }

    public void FallAnimation()
    {
        anim.SetBool(Falling, playerController.rb.velocity.y < 0 && !playerController.Grounded());
    }

    public void WallSideAnimation()
    {
        anim.SetBool(SideWall, playerController.IsWallSlide());
    }

    public void DashAnimation()
    {
        anim.SetTrigger(Dash);
    }

    public void AttackAnimation()
    {
        anim.SetTrigger(Attacking);
    }

    public void HurtAnimation()
    {
        anim.SetTrigger(TakeDamage);
    }

    public void HealAnimation(bool isHealing)
    {
        anim.SetBool(Healing, isHealing);
    }
}
