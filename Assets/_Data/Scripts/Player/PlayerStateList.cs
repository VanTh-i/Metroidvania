using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateList : MonoBehaviour
{
    public bool IsInAir = false;
    public bool IsDashing = false;
    public bool IsWallSliding = false;
    public bool Invincible;
    public bool RecoilingX, RecoilingY;
    public bool LookingRight;
    public bool Healing;

    public bool CutScene = false;

    [Header("Unlock Skill")]
    public bool CanDoubleJumpSkill;
    public bool CanDashSkill;
    public bool CanWallSliding;
}
