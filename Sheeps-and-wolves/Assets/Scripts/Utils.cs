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

    // angle utils
    public Vector2 getDirection(float degAngle)
    {
        return new Vector2(Mathf.Cos(degAngle / 180 * Mathf.PI), Mathf.Sin(degAngle / 180 * Mathf.PI));
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
