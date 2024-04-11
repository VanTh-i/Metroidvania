using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private float followSpeed;
    [SerializeField] private Vector3 offset;
    public Transform player;

    private void Start()
    {
        transform.position = player.position + offset;
    }

    private void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, player.position + offset, followSpeed);
    }
}
