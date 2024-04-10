using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private PlayerController playerController;
    private PlayerAnimation playerAnimation;
    private PlayerCombat playerCombat;
    private PlayerHealth playerHealth;

    private void Start()
    {
        playerController = GetComponentInChildren<PlayerController>();
        playerAnimation = GetComponentInChildren<PlayerAnimation>();
        playerCombat = GetComponentInChildren<PlayerCombat>();
        playerHealth = GetComponentInChildren<PlayerHealth>();
    }

    private void Update()
    {
        if (playerController.playerState.IsDashing) return;
        if (playerController.playerState.CutScene) return;
        if (playerHealth.restoreTime) return;

        playerController.Moving();
        playerController.Jump();
        playerController.WallSlide();
        playerController.WallJump();
        playerController.StartDash();

        playerCombat.Attack();

        playerHealth.Heal();

        //Animation();

    }

    // private void Animation()
    // {
    //     playerAnimation.RunAnimation();
    //     playerAnimation.JumpAnimation();
    //     playerAnimation.FallAnimation();
    //     playerAnimation.WallSideAnimation();
    // }
}
