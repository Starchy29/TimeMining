using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private enum gameStates
    {
        GAME,
        PAUSE,
        MENU,
        STORE
    };
    private gameStates gameState;
    // Start is called before the first frame update
    void Start()
    {
        gameState = gameStates.MENU;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
