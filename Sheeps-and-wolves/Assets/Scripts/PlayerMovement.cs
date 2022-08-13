using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Speeds")]
    [SerializeField] private Vector2 movementSpeed;
    [SerializeField] private Vector2 rotationSpeed;
    void Update()
    {
      if (Input.GetKeyDown(KeyCode.Z))
        {
            // go forward
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            // go backward
        }

      if (Input.GetKeyDown(KeyCode.Q))
        {
            // rotate left
        }

      if (Input.GetKeyDown(KeyCode.D))
        {
            // rotate right
        }

      if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            // set high speed
        } else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            // set default speed
        }
    }
}
