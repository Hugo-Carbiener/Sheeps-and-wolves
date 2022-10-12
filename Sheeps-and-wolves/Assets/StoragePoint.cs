using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class StoragePoint : MonoBehaviour
{
    [Header("Target point")]
    [SerializeField] private Vector2 target;
    private List<GameObject> sheepsStored;

    private void Awake()
    {
        sheepsStored = new List<GameObject>();
        Assert.IsTrue(target != null);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject collided = collision.gameObject;
        if (collided.tag == "Sheep")
        {
            sheepsStored.Add(collided);
            collided.GetComponent<Sheep>().MoveTo(target);
        }
    }
}
