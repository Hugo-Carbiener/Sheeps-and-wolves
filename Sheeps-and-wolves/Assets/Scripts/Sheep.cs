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
    [SerializeField] private int playerFleeRange = 5;
    [SerializeField] private Rigidbody2D rigidBody;
    [SerializeField] private Transform player;
    [SerializeField] private int speed = 10;
    [SerializeField] private int rotationSpeed = 90;
    [SerializeField] private int targetAngleSpan = 10;
    private SheepState state = SheepState.Idle;

    void Start()
    {
        if (!rigidBody) rigidBody = GetComponent<Rigidbody2D>();
        if (!player) player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {

        Vector2 playerDirection = player.position - transform.position;
        float relativePlayerAngularPosition = Mathf.Atan2(playerDirection.y, playerDirection.x) / Mathf.PI * 180;
        relativePlayerAngularPosition = Utils.Instance.normalise(relativePlayerAngularPosition);
        float targetAngle = relativePlayerAngularPosition + 180;
        targetAngle = Utils.Instance.normalise(targetAngle);
        Quaternion targetRotation = new Quaternion(0, 0, targetAngle, 0);

        if ((transform.position - player.position).magnitude <= playerFleeRange && state != SheepState.Fleeing)
        {
            state = SheepState.Fleeing;
        } else if ((transform.position - player.position).magnitude > playerFleeRange)
        {
            // stops the sheep at the end of the flee
            if (state != SheepState.Idle)
            {
                rigidBody.velocity = Vector3.zero;
            }
            state = SheepState.Idle;
        }

        if (state == SheepState.Fleeing)
        {
            float upperBound = relativePlayerAngularPosition + 180 + targetAngleSpan;
            float lowerBound = relativePlayerAngularPosition + 180 - targetAngleSpan;
            Utils.Instance.normalise(upperBound);
            Utils.Instance.normalise(lowerBound);


            if  (transform.rotation.eulerAngles.z <= upperBound || transform.rotation.eulerAngles.z >= lowerBound)
            {
                float rotationStep = rotationSpeed * Time.deltaTime;
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationStep);
                rigidBody.AddRelativeForce(Vector2.up * speed * 0.5f * Time.deltaTime);
            } else {
                rigidBody.AddRelativeForce(Vector2.up * speed * Time.deltaTime);
            }
        }
    }
}
