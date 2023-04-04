using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

// attaches the grid object with a floor child and a wall child
public class CaveGenerator : MonoBehaviour
{
    [SerializeField] private int caveWidth; // same for height
    [SerializeField] private int baseWidth;
    [SerializeField] private int baseHeight;
    [SerializeField] private Tile groundTile;
    [SerializeField] private Tile wallTile;
    [SerializeField] private Tile sugarTile;
    [SerializeField] private Tile chocolateTile;
    [SerializeField] private Tile oatmealTile;
    [SerializeField] private Tile bedrockTile;
    [SerializeField] private GameObject lightCracks;
    [SerializeField] private GameObject heavyCracks;
    [SerializeField] private TMPro.TextMeshProUGUI timeLimitIndicator;
    [SerializeField] private TMPro.TextMeshProUGUI goalIndicator;

    private Tilemap floorTiles;
    private Tilemap wallTiles;
    private WallData[,] dataGrid;
    private Dictionary<Sprite, WallType> spriteToWallType;
    private int difficulty;

    private float timeLeftSecs; // time left in the day
    private Dictionary<string, int> requiredCookies;
    public Dictionary<string, int> RequiredCookies { get { return requiredCookies; } }

    void Start()
    {
        spriteToWallType = new Dictionary<Sprite, WallType>();
        spriteToWallType[wallTile.sprite] = WallType.Rock;
        spriteToWallType[sugarTile.sprite] = WallType.Sugar;
        spriteToWallType[chocolateTile.sprite] = WallType.Chocolate;
        spriteToWallType[oatmealTile.sprite] = WallType.Oatmeal;
        spriteToWallType[bedrockTile.sprite] = WallType.Bedrock;

        floorTiles = transform.GetChild(0).GetComponent<Tilemap>();
        wallTiles = transform.GetChild(1).GetComponent<Tilemap>();

        transform.position = new Vector3(-caveWidth / 2.0f, -caveWidth / 2.0f, 0); // shift the grid so the base is centered

        requiredCookies = new Dictionary<string, int>();
        difficulty = -1; // difficulty increments at the start of NextDay()
        NextDay();
    }

    void Update()
    {
        timeLeftSecs -= Time.deltaTime;
        if(timeLeftSecs <= 0) {
            // lose game, TEMP: go to main menu
            SceneManager.LoadScene("Titlescreen");
        }

        // update time UI
        int minsLeft = (int)timeLeftSecs / 60;
        int partialMinuteLeft = (int)timeLeftSecs % 60;
        timeLimitIndicator.text = "Time Left: " + minsLeft + ":" + partialMinuteLeft;
    }

    private void Generate()
    {
        float chanceStep = 0.04f;
        float ingredientChance = 0.2f - difficulty * chanceStep;
        if(ingredientChance < chanceStep) {
            ingredientChance = chanceStep;
        }

        dataGrid = new WallData[caveWidth, caveWidth];
        for(int y = 0; y < caveWidth; y++) {
            for(int x = 0; x < caveWidth; x++) {
                floorTiles.SetTile(new Vector3Int(x, y), groundTile);

                if(IsInBase(x, y)) {
                    dataGrid[x, y] = new WallData(WallType.None);
                    continue;
                }

                // choose wall type
                Tile chosenTile = null;
                if(x == 0 || y == 0 || x == caveWidth - 1 || y == caveWidth - 1) {
                    // bedrock border
                    chosenTile = bedrockTile;
                }
                else if(Random.value < ingredientChance) {
                    // cooking ingredient
                    float rand = Random.value;
                    if(rand < 0.5f) {
                        // 50% chance sugar
                        chosenTile = sugarTile;
                    }
                    else if(rand > 0.75) {
                        // 25% chance chocolate
                        chosenTile = chocolateTile;
                    }
                    else {
                        // 25% chance oatmeal
                        chosenTile = oatmealTile;
                    }
                } 
                else {
                    // rock wall
                    chosenTile = wallTile;
                }

                wallTiles.SetTile(new Vector3Int(x, y), chosenTile);
                dataGrid[x, y] = new WallData(spriteToWallType[chosenTile.sprite]);
            }
        }
    }

    private bool IsInBase(int x, int y) {
        return x >= (caveWidth - baseWidth) / 2 && x <= (caveWidth + baseWidth) / 2
            && y >= (caveWidth - baseHeight) / 2 && y <= (caveWidth + baseHeight) / 2;
    }

    public void DamageTile(int x, int y, float damage) {
        WallData data = dataGrid[x, y];

        if(damage <= 0 || data.Health <= 0) {
            // attempted to damage an indestructible or nonexistant wall, or tried to do negative damage
            return;
        }

        float prevHealth = data.Health;
        data.Health -= damage;

        if(data.Health <= 0) {
            // destroy wall
            wallTiles.SetTile(new Vector3Int(x, y, 0), null);

            if(data.Cracks != null) {
                Destroy(data.Cracks);
            }
            dataGrid[x, y] = new WallData(WallType.None);
            return;
        }

        // add cracks
        float percentage = (float)data.Health / data.MaxHealth;
        float lastPercentage = (float)prevHealth / data.MaxHealth;

        if(percentage <= 0.3f && lastPercentage > 0.3f) {
            // add heavy cracks
            GameObject lastCracks = data.Cracks;
            GameObject newCracks = Instantiate(heavyCracks, transform);
            newCracks.transform.localPosition = new Vector3(x + 0.5f, y + 0.5f, 0);

            if(lastCracks == null) {
                newCracks.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
            } else {
                newCracks.transform.rotation = lastCracks.transform.rotation;
            }

            if(lastCracks != null) {
                Destroy(data.Cracks);
            }
            data.Cracks = newCracks;
        }
        else if(percentage <= 0.6f && lastPercentage > 0.6f) {
            // add light cracks
            data.Cracks = Instantiate(lightCracks, transform);
            data.Cracks.transform.localPosition = new Vector3(x + 0.5f, y + 0.5f, 0);
            data.Cracks.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
        }

    }

    public float GetWallHealth(Vector2Int tilePos)
    {
        return dataGrid[tilePos.x, tilePos.y].Health;
    }

    // determines which type of wall is located at the input tilemap coordinate. Returns WallType.None if there is no wall
    public WallType GetWallType(Vector2Int tilePos) {
        
        return dataGrid[tilePos.x, tilePos.y].Type;
    }

    // returns the tilemap coordinate based on the transform position
    public Vector2Int GetTilemapPos(Vector3 worldPosition) {
        Vector3Int cell = floorTiles.WorldToCell(worldPosition);
        return new Vector2Int(cell.x, cell.y);
    }

    public void NextDay() {
        difficulty++;
        timeLeftSecs = 60f * 5f;

        // randomize cookie goal
        int totCookies = 6 + difficulty * 2;
        int numSugar = Random.Range(1, totCookies / 2 + 1);
        int numChoc = Random.Range(1, totCookies - numSugar);
        int numOat = totCookies - numSugar - numChoc;

        requiredCookies["Sugar"] = numSugar;
        requiredCookies["Chocolate"] = numChoc;
        requiredCookies["Oatmeal"] = numOat;

        goalIndicator.text = $"Goal: {numSugar} sugar, {numChoc} chocolate, {numOat} oatmeal";

        // create cave
        Generate();

        // place player back in the middle
        GameObject.Find("Player").transform.position = Vector3.zero;
    }
}
