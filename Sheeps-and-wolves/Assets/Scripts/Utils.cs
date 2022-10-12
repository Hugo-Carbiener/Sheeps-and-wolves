using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour
{
    public static Utils Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    // turns an angle into a vector
    public Vector2 GetDirectionFromAngle(float degAngle)
    {
        return new Vector2(Mathf.Cos(degAngle / 180 * Mathf.PI), Mathf.Sin(degAngle / 180 * Mathf.PI));
    }

    // turns a vector into an angle
    public float GetAngleFromVector(Vector2 vec)
    {
        return Mathf.Atan2(vec.x, vec.y) * Mathf.Rad2Deg;
    }

    // normalize an angle if out of bounds

    public float normalise(float degAngle)
    {
        if (degAngle < 0)
        {
            degAngle += 360;
        }
        if (degAngle >= 360)
        {
            degAngle -= 360;
        }
        return degAngle;
    }
}
