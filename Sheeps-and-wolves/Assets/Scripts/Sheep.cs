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
    [SerializeField] private int playerFleeRange;
    [SerializeField] private Rigidbody2D rigidBody;
    [SerializeField] private Transform player;
    [SerializeField] private int speed;
    [SerializeField] private int rotationSpeed;
    [SerializeField] private int targetAngleSpan;
    [SerializeField] private float maxSpeed;
    private SheepState state;
    private float currentSpeed;
    private Vector2 direction;

    void Start()
    {
        if (!rigidBody) rigidBody = GetComponent<Rigidbody2D>();
        if (!player) player = GameObject.FindGameObjectWithTag("Player").transform;
        state = SheepState.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        // get movement data
        direction = Utils.Instance.getDirection(transform.eulerAngles.z + 90);
        currentSpeed = rigidBody.velocity.magnitude;

        // process sheep state 
        if ((transform.position - player.position).magnitude <= playerFleeRange && state != SheepState.Fleeing)
        {
            state = SheepState.Fleeing;
        } else if ((transform.position - player.position).magnitude > playerFleeRange)
        {
            // stops the sheep at the end of the flee
            if (state != SheepState.Idle)
            {
                rigidBody.velocity = Vector3.zero;
                currentSpeed = 0;
            }
            state = SheepState.Idle;
        }


        if (state == SheepState.Fleeing)
        {
            float relativePlayerAngularPosition = getPlayerAngularPosition();
            Quaternion targetRotation = getFleeTarget(relativePlayerAngularPosition);

            // rotation
            float rotationStep = rotationSpeed * Time.deltaTime;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationStep);

            // go faster if not rotating
            if (transform.rotation.eulerAngles.z - targetRotation.eulerAngles.z <= targetAngleSpan)
            {
                currentSpeed = maxSpeed;
            } else
            {
                currentSpeed = maxSpeed / 2;
            }
        }

        // apply speed
        rigidBody.velocity = direction * currentSpeed;
    }
   
    /**
     * Get the angular position of the player relative to the sheep (get the poulet)
     */
    private float getPlayerAngularPosition()
    {
        Vector2 playerDirection = player.position - transform.position;
        float relativePlayerAngularPosition = Mathf.Atan2(playerDirection.y, playerDirection.x) / Mathf.PI * 180;
        relativePlayerAngularPosition = Utils.Instance.normalise(relativePlayerAngularPosition);
        Debug.Log("player: " + relativePlayerAngularPosition);
        return relativePlayerAngularPosition;
    }

    /**
     * Get the angle pointing in the opposite direction from the player
     */
    private Quaternion getFleeTarget(float relativePlayerAngularPosition)
    {
        float targetAngle = relativePlayerAngularPosition + 180;
        targetAngle = Utils.Instance.normalise(targetAngle);
        Debug.Log("target: " + targetAngle);
        return Quaternion.Euler(0, 0, targetAngle - 90);
    }
}
