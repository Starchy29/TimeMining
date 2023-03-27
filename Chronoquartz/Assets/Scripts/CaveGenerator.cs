using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

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
    private WallData[,] dataGrid;

    void Start()
    {
        Dictionary<Sprite, WallType> spriteToWallType = new Dictionary<Sprite, WallType>();
        spriteToWallType[wallTile.sprite] = WallType.Rock;
        spriteToWallType[speedCrystalTile.sprite] = WallType.SpeedCrystal;
        spriteToWallType[reverseCrystalTile.sprite] = WallType.ReverseCrystal;
        // add bedrock

        floorTiles = transform.GetChild(0).GetComponent<Tilemap>();
        wallTiles = transform.GetChild(1).GetComponent<Tilemap>();

        const int REACH = 30; // distance that is generated outward from the center
        dataGrid = new WallData[2 * REACH + 1, 2 * REACH + 1];
        for(int y = -REACH; y <= REACH; y++) {
            for(int x = -REACH; x <= REACH; x++) {
                floorTiles.SetTile(new Vector3Int(x, y), groundTile);

                if(IsInBase(x, y)) {
                    //dataGrid[x, y] = new WallData(WallType.None);
                    continue;
                }

                // choose wall or crystal
                Tile chosenTile = null;
                if(Random.value < 0.1f) {
                    // crystal
                    chosenTile = (Random.value < 0.5f ? speedCrystalTile : reverseCrystalTile);
                } else {
                    // rock wall
                    chosenTile = wallTile;
                }

                wallTiles.SetTile(new Vector3Int(x, y), chosenTile);
                //dataGrid[x, y] = new WallData(spriteToWallType[chosenTile.sprite]);
            }
        }
    }

    private bool IsInBase(int x, int y) {
        return x >= -baseWidth / 2 && x <= baseWidth / 2
            && y >= -baseHeight / 2 && y <= baseHeight / 2;
    }

    // determines which type of wall is located at the input tilemap coordinate. Returns WallType.None if there is no wall
    public WallType GetWallType(int x, int y) {
        return dataGrid[x, y].Type;
    }

    // returns the tilemap coordinate based on the transform position
    public Vector2Int GetTilemapPos(Vector3 worldPosition) {
        Vector3Int cell = floorTiles.LocalToCell(worldPosition);
        return new Vector2Int(cell.x, cell.y);
    }
}
