using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrawlidAnimation : EnemyAnimation
{
    public void FlipAnimation()
    {
        anim.SetTrigger(Flip);
    }
    public void DeathAnimation()
    {
        anim.SetTrigger(Death);
    }
}
