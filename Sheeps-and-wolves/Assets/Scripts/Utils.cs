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

    // normalize an angle if out of bounds
    public float normalise(float angle)
    {
        if (angle < 0)
        {
            angle += 360;
        }
        if (angle >= 360)
        {
            angle -= 360;
        }
        return angle;
    }
}
