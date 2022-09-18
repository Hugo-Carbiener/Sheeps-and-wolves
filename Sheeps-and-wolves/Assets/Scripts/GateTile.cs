using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New Gate tile", menuName = "Custom tiles/Gate tile")]

public class GateTile : Tile
{
    private enum alignement
    {
        Left,
        Right
    }

    private enum facing
    {
        Front,
        Back
    }

    [Header("Variables")]
    [SerializeField] private alignement side;
    [SerializeField] private facing face;
    [Header("Sprites")]
    // sprites are stored as left front, right front, left back, right back
    [SerializeField] private Sprite[] openedGateSprites; 
    [SerializeField] private Sprite[] openedGateSpritesHighlighted;
    [SerializeField] private Sprite[] closedGateSprites;
    [SerializeField] private Sprite[] closedGateSpritesHighlighted;

    private Sprite closedGateSprite;
    private Sprite closedGateSpriteHighlighted;
    private Sprite openedGateSprite;
    private Sprite openedGateSpriteHighlighted;

    private bool isInteractable = false;
    private bool isOpened = false;
    
    public override void RefreshTile(Vector3Int position, ITilemap tilemap)
    {
        base.RefreshTile(position, tilemap);
    }

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        base.GetTileData(position, tilemap, ref tileData);
        RefreshSprite(ref tileData);
        
    }

    public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
    {
        initSprites();

        return true;
    }

    private void initSprites()
    {
        if (side == alignement.Left && face == facing.Front)
        {
            closedGateSprite = closedGateSprites[0];
            closedGateSpriteHighlighted = closedGateSpritesHighlighted[0];
            openedGateSprite = openedGateSprites[0];
            openedGateSpriteHighlighted = openedGateSpritesHighlighted[0];
        }
        else if (side == alignement.Right && face == facing.Front) 
        {
            closedGateSprite = closedGateSprites[1];
            closedGateSpriteHighlighted = closedGateSpritesHighlighted[1];
            openedGateSprite = openedGateSprites[1];
            openedGateSpriteHighlighted = openedGateSpritesHighlighted[1];
        }
        else if (side == alignement.Left && face == facing.Back)
        {
            closedGateSprite = closedGateSprites[2];
            closedGateSpriteHighlighted = closedGateSpritesHighlighted[2];
            openedGateSprite = openedGateSprites[2];
            openedGateSpriteHighlighted = openedGateSpritesHighlighted[2];
        } else if (side == alignement.Right && face == facing.Back)
        {
            closedGateSprite = closedGateSprites[3];
            closedGateSpriteHighlighted = closedGateSpritesHighlighted[3];
            openedGateSprite = openedGateSprites[3];
            openedGateSpriteHighlighted = openedGateSpritesHighlighted[3];
        }
    }

    private void RefreshSprite(ref TileData tileData)
    {
        //Debug.Log("Refresh with  " + isInteractable + " and " + isOpened);
        if (isInteractable && isOpened)
        {
            tileData.sprite = openedGateSpriteHighlighted;
        } else if (!isInteractable && isOpened)
        {
            tileData.sprite = openedGateSprite;
        } else if (isInteractable && !isOpened)
        {
            tileData.sprite = closedGateSpriteHighlighted;
        } else if (!isInteractable && !isOpened)
        {
            tileData.sprite = closedGateSprite;
        }
    }
    
    public void CycleState()
    {
        isOpened = !isOpened;
    }

    public void SetInteractibility(bool interactibility)
    {
        isInteractable = interactibility;
    }
}
