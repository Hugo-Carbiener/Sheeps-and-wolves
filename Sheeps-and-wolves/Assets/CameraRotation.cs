using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    [SerializeField] private GameObject player;

    private void Awake()
    {
        if (!player) player = GameObject.FindGameObjectWithTag("Player");
    }
    private void FixedUpdate()
    {
        transform.position = player.transform.position + new Vector3(0, 0, -10);
    }
}
