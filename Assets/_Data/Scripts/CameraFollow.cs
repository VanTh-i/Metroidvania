using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private float followSpeed;
    [SerializeField] private Vector3 offset;

    private void Start()
    {
        transform.position = Player.Instance.transform.position + offset;
    }

    private void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, Player.Instance.transform.position + offset, followSpeed);
    }
}
