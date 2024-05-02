using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    protected Animator anim;
    protected const string Flip = "Flip";
    protected const string Death = "Death";

    protected void Start()
    {
        anim = GetComponent<Animator>();
    }
}
