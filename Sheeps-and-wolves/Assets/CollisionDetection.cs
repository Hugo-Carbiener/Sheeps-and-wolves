using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    public delegate void collisionEnterDetectionDelegate(Vector3Int position);
    public delegate void collisionExitDetectionDelegate(Vector3Int position);
    public collisionEnterDetectionDelegate collisionEnterDelegate;
    public collisionExitDetectionDelegate collisionExitDelegate;

    private Vector3Int position;

    private void OnTriggerEnter2D()
    {
        collisionEnterDelegate.Invoke(position);
    }

    private void OnTriggerExit2D()
    {
        collisionExitDelegate.Invoke(position);
    }

    public void SetPosition(Vector3Int position)
    {
        this.position = position;
    }
}
