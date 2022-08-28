using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [Header("Movement variables")]
    private float currentSpeed;
    private float rotationSpeed;
    private Vector2 direction;

    public float getCurrentSpeed() { return this.currentSpeed; }
    public void setCurrentSpeed(float speed) { currentSpeed = speed; }
    public float getRotationSpeed() { return this.rotationSpeed;  }
    public void setRotationspeed(float speed) { rotationSpeed = speed; }
    public Vector2 getDirection() { return this.direction; }
    public void setDirection(Vector2 dir) { direction = dir; }


}
