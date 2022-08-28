using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private PlayerManager player;
    [SerializeField] private Rigidbody2D rigidBody;
    [Header("Speeds")]
    [SerializeField] private Vector2 maxMovementSpeeds;
    [SerializeField] private Vector2 rotationSpeeds;
    [Header("Misc")]
    private float maxSpeedThreshold;

    private void Start()
    {
        if (!player) player = GetComponent<PlayerManager>();
        if (!rigidBody) rigidBody = GetComponent<Rigidbody2D>();
        player.setCurrentSpeed(0);
        player.setRotationspeed(rotationSpeeds.x);
        maxSpeedThreshold = maxMovementSpeeds.x;
    }

    void Update()
    {
        // get movement data
        player.setDirection(Utils.Instance.getDirection(transform.eulerAngles.z + 90));
        player.setCurrentSpeed(rigidBody.velocity.magnitude);

        // speed control 
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            // set high speed
            maxSpeedThreshold = maxMovementSpeeds.y;
            player.setRotationspeed(rotationSpeeds.y);
        } else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            // set default speed
            maxSpeedThreshold = maxMovementSpeeds.x;
            player.setRotationspeed(rotationSpeeds.x);
        }

        // direction control 
        if (Input.GetKey(KeyCode.Z))
        {
            // go forward
            if (player.getCurrentSpeed() == 0)
            {
                //Debug.Log("ignition");
                rigidBody.AddRelativeForce(player.getDirection() * 100 * Time.deltaTime);
            } else {

                //Debug.Log("Multiplication");
                player.setCurrentSpeed(player.getCurrentSpeed() * 2);
            }
        }

        if (Input.GetKey(KeyCode.S))
        {
            // go backward
            player.setCurrentSpeed(player.getCurrentSpeed() / 2);
        }

        if (Input.GetKey(KeyCode.Q))
        {
            // rotate left
            transform.Rotate(Vector3.forward * (player.getRotationSpeed() * Time.deltaTime));
        }

        if (Input.GetKey(KeyCode.D))
        {
            // rotate right
            transform.Rotate(Vector3.back * (player.getRotationSpeed() * Time.deltaTime));
        }

        // speed modifications
        if (player.getCurrentSpeed() > maxSpeedThreshold) player.setCurrentSpeed(maxSpeedThreshold);

        // apply speed
        rigidBody.velocity = player.getDirection() * player.getCurrentSpeed();
    }
}
