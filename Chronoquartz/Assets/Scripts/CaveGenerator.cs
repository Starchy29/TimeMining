using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

// attaches the grid object with a floor child and a wall child
public class CaveGenerator : MonoBehaviour
{
    [SerializeField] private int baseWidth;
    [SerializeField] private int baseHeight;
    [SerializeField] private TileBase groundTile;
    [SerializeField] private TileBase wallTile;
    [SerializeField] private TileBase speedCrystalTile;
    [SerializeField] private TileBase reverseCrystalTile;

    private Tilemap floorTiles;
    private Tilemap wallTiles;

    void Start()
    {
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

    void Update()
    {
        
    }
}
