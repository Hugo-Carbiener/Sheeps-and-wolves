using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Vector3 offset;

    private void Awake()
    {
        if (!player) player = GameObject.FindGameObjectWithTag("Player");
    }
    private void FixedUpdate()
    {
        transform.position = player.transform.position + offset;
    }
}
