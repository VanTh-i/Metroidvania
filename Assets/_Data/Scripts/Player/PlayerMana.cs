using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMana : MonoBehaviour
{
    [SerializeField] private float mana;
    [SerializeField] public float manaDrainSpeed;
    [SerializeField] private Image manaStorage;

    private void Start()
    {
        Mana = mana;
        manaStorage.fillAmount = mana;
    }

    public float Mana
    {
        get { return mana; }
        set
        {
            if (mana != value)
            {
                mana = Mathf.Clamp(value, 0, 1);
                manaStorage.fillAmount = mana;
            }
        }
    }

}
