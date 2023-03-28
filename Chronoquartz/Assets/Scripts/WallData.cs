using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WallType {
    None,
    Rock,
    SpeedCrystal,
    ReverseCrystal,
    Bedrock
}

// a container for information about each wall tile
public class WallData
{
    public float Health;
    public GameObject Cracks;
    public readonly float MaxHealth;

    private WallType type;
    public WallType Type { get { return type; } }

    public WallData(WallType type)
    {
        Health = 0;
        Cracks = null;
        this.type = type;
        
        // define starting health values
        switch(type)
        {
            case WallType.Rock:
                Health = 1.0f;
                break;
            case WallType.SpeedCrystal:
            case WallType.ReverseCrystal:
                Health = 2.0f;
                break;
        }
        MaxHealth = Health;
    }
}
