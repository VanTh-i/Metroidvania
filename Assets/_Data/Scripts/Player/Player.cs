using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private PlayerController playerController;
    private PlayerAnimation playerAnimation;

    private void Start()
    {
        playerController = GetComponentInChildren<PlayerController>();
        playerAnimation = GetComponentInChildren<PlayerAnimation>();
    }

    private void Update()
    {
        if (playerController.playerState.IsDashing) return;

        playerController.Moving();
        playerController.Jump();
        playerController.WallSlide();
        playerController.WallJump();
        playerController.StatDash();

        playerAnimation.RunAnimation();
        playerAnimation.JumpAnimation();
        playerAnimation.FallAnimation();
        playerAnimation.WallSideAnimation();

    }
}
