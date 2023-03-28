using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    // Arraylist of all possible windows. 
    // Player inventory must be Element 0, and Shop must be Element 1.
    public GameObject[] windows;

    /// <summary>
    /// On start, close all scenes unless titlescreen
    /// </summary>
    void Start()
    {
        if(SceneManager.GetActiveScene().name != "Titlescreen")
        {
            closeAllWindows();
        } else
        {
            windows[1].SetActive(false);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        KeyCheck();
    }

    /// <summary>
    /// When keys are pressed, showcase the proper UI screen.
    /// </summary>
    void KeyCheck()
    {
        // Inventory is pressed
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            //closeAllWindows(windows[0]);
            closeAllWindows();
            windows[0].SetActive(!windows[0].activeSelf);
        }

        // Shop is pressed
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
            closeAllWindows(windows[1]);
            windows[1].SetActive(!windows[1].activeSelf);
        }
    }

    /// <summary>
    /// Closes all the windows open. Can choose to not ignore a set window.
    /// </summary>
    /// <param name="ignoreThis">A scene to ignore closing of (used for the pop up purchases)</param>
    public void closeAllWindows(GameObject ignoreThis = null)
    {
        Console.WriteLine(windows);

        foreach(GameObject window in windows)
        {
            if(window.Equals(ignoreThis))
            {
                //ignore the window

            } else
            {
                window.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Does everything for the titlescreen
    /// </summary>
    /// <param name="clicked">Pass in with words what you want the scene to do</param>
    public void TitlescreenSwitcher(string clicked)
    {
        switch(clicked)
        {
            case "Play":
                SceneManager.LoadScene("Gameplay");
            break;

            case "TitlescreenToSettings":
                windows[0].SetActive(false);
                windows[1].SetActive(true);
            break;

            case "SettingsToTitlescreen":
                windows[0].SetActive(true);
                windows[1].SetActive(false);
                break;
            case "Quit":
                Application.Quit();
                break;
        }
    }
}
