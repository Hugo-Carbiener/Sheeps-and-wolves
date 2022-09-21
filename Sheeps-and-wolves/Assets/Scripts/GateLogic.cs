using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Assertions;

public class GateLogic : MonoBehaviour
{
    private Grid grid;
    private Tilemap tilemap;
    private List<GateTile> interactingGateTiles;
    private List<Vector3Int> interactingTilePositions;
    private List<CollisionDetection> collisionDetectors;
    [SerializeField] private GameObject collisionDetector;

    private void Awake()
    {
        tilemap = GetComponent<Tilemap>();
        Debug.Log(tilemap.cellBounds.ToString());
        grid = tilemap.transform.parent.GetComponent<Grid>();
        interactingGateTiles = new List<GateTile>();
        interactingTilePositions = new List<Vector3Int>();
        collisionDetectors = new List<CollisionDetection>();
    }

    private void Start()
    {
        foreach (Vector3Int position in tilemap.cellBounds.allPositionsWithin)
        {
            if (tilemap.HasTile(position))
            {
                InstantiateCollisionDetector(position);
            }
        }

        RemoveDuplicateColliders();
    }

    private void Update()
    {
        if (interactingGateTiles.Count > 0 && Input.GetKeyDown(KeyCode.F)) {
            foreach (GateTile interactingGateTile in interactingGateTiles)
            {
                interactingGateTile.CycleState();
            }
            RefreshInteractingTile();
        }
    }

    private void EnterInteraction(Vector3Int position) {

        Vector3Int tilePos = FindNearbyTilePosition(position);
        TileBase tile = tilemap.GetTile(tilePos);

        if (tile)
        {
        Debug.Log("Enter");
            interactingGateTiles.Add((GateTile)tile);
            interactingTilePositions.Add(tilePos);

            if (tilemap.HasTile(tilePos + Vector3Int.right))
            {
                interactingGateTiles.Add((GateTile)tilemap.GetTile(tilePos + Vector3Int.right));
                interactingTilePositions.Add(tilePos + Vector3Int.right);
            } else if (tilemap.HasTile(tilePos + Vector3Int.left))
            {
                interactingGateTiles.Add((GateTile)tilemap.GetTile(tilePos + Vector3Int.left));
                interactingTilePositions.Add(tilePos + Vector3Int.left);
            }

            foreach (GateTile interactingGateTile in interactingGateTiles)
            {
                interactingGateTile.SetInteractibility(true);
            }
            RefreshInteractingTile();
        }
    }
    
    private void ExitInteraction(Vector3Int position)
    {
        Debug.Log("Exit");

        foreach (GateTile interactingGateTile in interactingGateTiles)
        {
            interactingGateTile.SetInteractibility(false);
        }
        RefreshInteractingTile();

        interactingGateTiles.Clear();
        interactingTilePositions.Clear();
    }

    private Vector3Int FindNearbyTilePosition(Vector3Int position)
    {
        for (int x = -1; x < 2; x++)
        {
            for (int y = -1; y < 2; y++)
            {
                Vector3Int curPos = new Vector3Int(position.x + x, position.y + y);
                if (tilemap.HasTile(curPos))
                {
                    return curPos;
                }
            }
        }
        Debug.LogError("Did not find tile in 3x3 area");
        return Vector3Int.zero;
    }

    private void RefreshInteractingTile()
    {
        if (interactingGateTiles.Count > 0)
        {
            foreach (Vector3Int pos in interactingTilePositions)
            {
                tilemap.RefreshTile(pos);
            }
        }
    }

    private void InstantiateCollisionDetector(Vector3Int position)
    {
        // check if there are two doors next to each others
        List<Vector3Int> gates = new List<Vector3Int>();
        gates.Add(position);

        if (tilemap.HasTile(position + Vector3Int.right))
        {
            gates.Add(position + Vector3Int.right);
        } else if (tilemap.HasTile(position + Vector3Int.left))
        {
            gates.Add(position + Vector3Int.left);
        }

        // instantiate collider and set position according to the tile it's linked to
        GameObject detector = Instantiate(collisionDetector, transform);
        float Xpos = 0;
        float Ypos = grid.CellToWorld(position).y;
        foreach(var pos in gates)
        {
            Xpos += grid.CellToWorld(pos).x;
        }
        Xpos = Xpos / gates.Count;
        Vector3 newPos = new Vector3(Xpos, Ypos, 0);
        Vector3 offset = new Vector3(grid.cellSize.x / 2, grid.cellSize.y / 2, 0);
        detector.transform.position = newPos + offset;
        detector.GetComponent<CircleCollider2D>().radius *= gates.Count;

        // detection component logic
        CollisionDetection detectionComponent = detector.GetComponent<CollisionDetection>();
        if (detectionComponent)
        {
            collisionDetectors.Add(detectionComponent);
            detectionComponent.SetPosition(position); 
            detectionComponent.collisionEnterDelegate += EnterInteraction;
            detectionComponent.collisionExitDelegate += ExitInteraction;
        }
    }

    private void RemoveDuplicateColliders()
    {
        for (int i = collisionDetectors.Count - 1; i > 0; i--)
        {
            if (collisionDetectors[i].transform.position == collisionDetectors[i - 1].transform.position)
            {
                CollisionDetection detection = collisionDetectors[i];
                collisionDetectors.RemoveAt(i);
                Destroy(detection.gameObject);
            }
        }
    }

    private void OnEnable()
    {
        foreach (CollisionDetection detectionComponent in collisionDetectors)
        {
            detectionComponent.collisionEnterDelegate += EnterInteraction;
            detectionComponent.collisionExitDelegate += ExitInteraction;
        }
    }

    private void OnDisable()
    {
        foreach(CollisionDetection detectionComponent in collisionDetectors)
        {
            detectionComponent.collisionEnterDelegate -= EnterInteraction;
            detectionComponent.collisionExitDelegate -= ExitInteraction;
        }
    }
}
