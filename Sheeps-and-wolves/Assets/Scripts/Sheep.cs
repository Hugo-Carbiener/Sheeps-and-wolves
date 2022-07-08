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
    [SerializeField] private int speed = 10;
    [SerializeField] private int rotationSpeed = 90;
    private SheepState state = SheepState.Idle;

    void Start()
    {
        if (!rigidBody) rigidBody = GetComponent<Rigidbody2D>();
        if (!player) player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 targetLocation = player.position - transform.position;
        targetLocation.Normalize();

        float angle = Mathf.Atan2(targetLocation.y, targetLocation.x) / Mathf.PI * 180;
        if (angle < 0)
        {
            angle += 360;
        }
        Debug.Log(angle);
        /*if ((transform.position - player.position).sqrMagnitude <= playerFleeRange * playerFleeRange)
        {
            state = SheepState.Fleeing;
        } else
        {
            state = SheepState.Idle;
        }

        if (state == SheepState.Fleeing)
        {
            rigidBody.AddRelativeForce(Vector2.right * speed * Time.deltaTime);
            transform.rotation = player.rotation;
        } */
    }
}
