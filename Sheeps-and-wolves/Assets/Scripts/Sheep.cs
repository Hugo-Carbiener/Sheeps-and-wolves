using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum SheepState
{
    Idle,
    Fleeing
}

public class Sheep : MonoBehaviour
{
    [SerializeField] private int playerFleeRange = 10;
    [SerializeField] private Rigidbody2D rigidBody;
    [SerializeField] private Transform player;
    private SheepState state = SheepState.Idle;

    void Start()
    {
        if (!rigidBody) rigidBody = GetComponent<Rigidbody2D>();
        if (!player) player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if ((transform.position - player.position).sqrMagnitude <= playerFleeRange * playerFleeRange)
        {
            state = SheepState.Fleeing;
        } else
        {
            state = SheepState.Idle;
        }

        if (state == SheepState.Fleeing)
        {
            
        }
    }
}
