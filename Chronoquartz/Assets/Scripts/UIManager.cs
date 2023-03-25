using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    // Arraylist of all possible windows. 
    // Player inventory must be Element 0, and Shop must be Element 1.
    public GameObject[] windows;

    void Start()
    {
        closeAllWindows();
    }

    // Update is called once per frame
    void Update()
    {
        keyCheck();
    }

    /// <summary>
    /// When keys are pressed, showcase the proper UI screen.
    /// </summary>
    void keyCheck()
    {
        // Inventory is pressed
        if(Input.GetKeyDown(KeyCode.I))
        {
            closeAllWindows(windows[0]);
            windows[0].SetActive(!windows[0].active);
        }

        // Inventory is pressed
        if (Input.GetKeyDown(KeyCode.O))
        {
            closeAllWindows(windows[1]);
            windows[1].SetActive(!windows[1].active);
        }
    }

    /// <summary>
    /// Closes all the windows open. Can choose to not ignore a set window.
    /// </summary>
    void closeAllWindows(GameObject ignoreThis = null)
    {
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
}
