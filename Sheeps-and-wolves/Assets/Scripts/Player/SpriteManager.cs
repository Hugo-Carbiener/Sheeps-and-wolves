using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteManager: MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private PlayerManager player;
    [SerializeField] private Animator anim;
    private Rigidbody2D rb;

    private void Start()
    {
        if (!player) player = transform.parent.GetComponent<PlayerManager>();
        if (!anim) anim = GetComponent<Animator>();
        rb = transform.parent.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        transform.eulerAngles = Vector3.zero;
        anim.SetFloat("rotationAngle", player.getRotationAngle());
        anim.SetFloat("velocity", rb.velocity.magnitude);
    }
}
