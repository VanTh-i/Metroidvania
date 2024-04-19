using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private static Player instance;
    public static Player Instance { get => instance; set => instance = value; }

    private PlayerController playerController;
    private PlayerCombat playerCombat;
    private PlayerHealth playerHealth;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }

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
        if (playerHealth.restoreTime) return;

        playerHealth.Heal();

        if (playerController.playerState.Healing) return;

        playerCombat.Attack();
        playerController.Moving();
        playerController.Jump();
        playerController.WallSlide();
        playerController.StartWallJump();
        playerController.StartDash();

    }

    private void FixedUpdate()
    {
        if (playerController.playerState.IsDashing) return;

        playerController.WallJump();
    }
}
