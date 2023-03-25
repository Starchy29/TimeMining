using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum WallType {
    None,
    Rock,
    SpeedCrystal,
    ReverseCrystal
}

// attaches the grid object with a floor child and a wall child
public class CaveGenerator : MonoBehaviour
{
    [SerializeField] private int baseWidth;
    [SerializeField] private int baseHeight;
    [SerializeField] private Tile groundTile;
    [SerializeField] private Tile wallTile;
    [SerializeField] private Tile speedCrystalTile;
    [SerializeField] private Tile reverseCrystalTile;

    private Tilemap floorTiles;
    private Tilemap wallTiles;
    private Dictionary<Sprite, WallType> spriteToWallType;

    void Start()
    {
        spriteToWallType = new Dictionary<Sprite, WallType>();
        spriteToWallType[wallTile.sprite] = WallType.Rock;
        spriteToWallType[speedCrystalTile.sprite] = WallType.SpeedCrystal;
        spriteToWallType[reverseCrystalTile.sprite] = WallType.ReverseCrystal;

        floorTiles = transform.GetChild(0).GetComponent<Tilemap>();
        wallTiles = transform.GetChild(1).GetComponent<Tilemap>();

        const int REACH = 30; // distance that is generated outward from the center
        for(int y = -REACH; y <= REACH; y++) {
            for(int x = -REACH; x <= REACH; x++) {
                floorTiles.SetTile(new Vector3Int(x, y), groundTile);

                if(IsInBase(x, y)) {
                    continue;
                }

                // choose wall or crystal
                if(Random.value < 0.1f) {
                    // crystal
                    wallTiles.SetTile(new Vector3Int(x, y), Random.value < 0.5f ? speedCrystalTile : reverseCrystalTile);
                } else {
                    // wall
                    wallTiles.SetTile(new Vector3Int(x, y), wallTile);
                }
            }
        }
    }

    private bool IsInBase(int x, int y) {
        return x >= -baseWidth / 2 && x <= baseWidth / 2
            && y >= -baseHeight / 2 && y <= baseHeight / 2;
    }

    // determines which type of wall is located at the input tilemap coordinate. Returns WallType.None if there is no wall
    public WallType GetWallType(int x, int y) {
        Tile tile = wallTiles.GetTile<Tile>(new Vector3Int(x, y, 0));
        if(tile == null) {
            return WallType.None;
        }

        return spriteToWallType[tile.sprite];
    }

    // returns the tilemap coordinate based on the transform position
    public Vector2Int GetTilemapPos(Vector3 worldPosition) {
        Vector3Int cell = floorTiles.LocalToCell(worldPosition);
        return new Vector2Int(cell.x, cell.y);
    }
}
