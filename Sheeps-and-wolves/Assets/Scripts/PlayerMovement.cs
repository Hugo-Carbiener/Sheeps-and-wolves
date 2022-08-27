using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Rigidbody2D rigidBody;
    [Header("Speeds")]
    [SerializeField] private Vector2 maxMovementSpeeds;
    [SerializeField] private Vector2 rotationSpeeds;
    [Header("Misc")]
    private float currentSpeed;
    private float rotationSpeed;
    private float maxSpeedThreshold;
    private Vector2 direction;

    private void Start()
    {
        if (!rigidBody) rigidBody = GetComponent<Rigidbody2D>();
        currentSpeed = 0;
        rotationSpeed = rotationSpeeds.x;
        maxSpeedThreshold = maxMovementSpeeds.x;
    }

    void Update()
    {
        // get movement data
        direction = Utils.Instance.getDirection(transform.eulerAngles.z + 90);
        currentSpeed = rigidBody.velocity.magnitude;

        // speed control 
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            // set high speed
            maxSpeedThreshold = maxMovementSpeeds.y;
            rotationSpeed = rotationSpeeds.y;
        } else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            // set default speed
            maxSpeedThreshold = maxMovementSpeeds.x;
            rotationSpeed = rotationSpeeds.x;
        }

        // direction control 
        if (Input.GetKey(KeyCode.Z))
        {
            // go forward
            if (currentSpeed == 0)
            {
                //Debug.Log("ignition");
                rigidBody.AddRelativeForce(direction * 100 * Time.deltaTime);
            } else {

                //Debug.Log("Multiplication");
                currentSpeed = currentSpeed * 2;
            }
        }

        if (Input.GetKey(KeyCode.S))
        {
            // go backward
            currentSpeed = currentSpeed / 2;
        }

        if (Input.GetKey(KeyCode.Q))
        {
            // rotate left
            transform.Rotate(Vector3.forward * (rotationSpeed * Time.deltaTime));
        }

        if (Input.GetKey(KeyCode.D))
        {
            // rotate right
            transform.Rotate(Vector3.back * (rotationSpeed * Time.deltaTime));
        }

        // speed modifications
        if (currentSpeed > maxSpeedThreshold) currentSpeed = maxSpeedThreshold;

        // apply speed
        rigidBody.velocity = direction * currentSpeed;
    }
}
