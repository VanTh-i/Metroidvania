using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartController : MonoBehaviour
{
    private PlayerHealth playerHealth;
    private GameObject[] heartContainers;
    private Image[] heartFills;
    public Transform heartParent;
    public GameObject heartContainersPrefab;

    private void Start()
    {
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerHealth>();
        heartContainers = new GameObject[playerHealth.maxHealth];
        heartFills = new Image[playerHealth.maxHealth];

        playerHealth.onHealthChangedCallback += UpdateHeartHUD;

        InstantiateHeartContainers();
        UpdateHeartHUD();
    }

    private void SetHeartContainers()
    {
        for (int i = 0; i < heartContainers.Length; i++)
        {
            if (i < playerHealth.maxHealth)
            {
                heartContainers[i].SetActive(true);
            }
            else
            {
                heartContainers[i].SetActive(false);
            }
        }
    }

    private void SetFilledHeart()
    {
        for (int i = 0; i < heartFills.Length; i++)
        {
            if (i < playerHealth.health)
            {
                heartFills[i].fillAmount = 1;
            }
            else
            {
                heartFills[i].fillAmount = 0;
            }
        }
    }

    private void InstantiateHeartContainers()
    {
        for (int i = 0; i < playerHealth.maxHealth; i++)
        {
            GameObject temp = Instantiate(heartContainersPrefab);
            temp.transform.SetParent(heartParent, false);
            heartContainers[i] = temp;
            heartFills[i] = temp.transform.Find("Heart Fill").GetComponent<Image>();
        }
    }

    private void UpdateHeartHUD()
    {
        SetHeartContainers();
        SetFilledHeart();
    }
}
