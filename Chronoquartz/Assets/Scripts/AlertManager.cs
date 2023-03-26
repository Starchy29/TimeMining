using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AlertManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject alertContainer;
    public GameObject alertPopup; 

    public Sprite[] icons;
    public Sprite defaultIcon;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            AddAlert("Test message");
        }
    }
    /// <summary>
    /// Returns a sprite from the array if it exists, otherwise returns default
    /// </summary>
    /// <param name="name">Name of icon</param>
    /// <returns></returns>
    public Sprite ReturnSprite(string name)
    {
        foreach(Sprite icon in icons)
        {
            if(icon.name.ToLower() == name.ToLower())
            {
                return icon;
            }
        }
        return defaultIcon;
    }
    /// <summary>
    /// Adds an alert to the alert container
    /// </summary>
    /// <param name="alertMessage">Message for the alert</param>
    /// <param name="icon">Optional icon, use ReturnSprite to grab from the grid or leave null for default</param>
    void AddAlert(string alertMessage, Sprite icon = null)
    {
        if(icon == null)
        {
            icon = defaultIcon;
        }

        GameObject alert = Instantiate(alertPopup, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        alert.transform.parent = alertContainer.transform;
        alert.transform.SetSiblingIndex(0);

        alert.transform.localScale = new Vector3(0.9689471f, 0.1254676f, 3.0507f);

        alert.transform.Find("Exclamation").GetComponent<Image>().sprite = icon;
        alert.transform.Find("Text (TMP)").GetComponent<TMP_Text>().text = alertMessage;
    }
}
