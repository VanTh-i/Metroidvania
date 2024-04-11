using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private PlayerController playerController;
    private PlayerCombat playerCombat;
    private PlayerHealth playerHealth;

    private void Start()
    {
        playerController = GetComponentInChildren<PlayerController>();
        playerCombat = GetComponentInChildren<PlayerCombat>();
        playerHealth = GetComponentInChildren<PlayerHealth>();
    }

    private void Update()
    {
        if (playerController.playerState.IsDashing) return;
        if (playerController.playerState.CutScene) return;
        if (playerController.playerState.Healing) return;
        if (playerHealth.restoreTime) return;

        playerController.Moving();
        playerController.Jump();
        playerController.WallSlide();
        playerController.WallJump();
        playerController.StartDash();

        playerCombat.Attack();

        playerHealth.Heal();
    }
}
