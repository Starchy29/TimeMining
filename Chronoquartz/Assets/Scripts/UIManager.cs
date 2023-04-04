using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // Arraylist of all possible windows. 
    // Player inventory must be Element 0, and Shop must be Element 1.
    public GameObject[] windows;
    private List<String> unlockedShapes = new List<String>();
    private List<String> ingredients = new List<String>();

    public GameObject ingredientBase;
    public GameObject ingredientContainer;
    public GameObject shapeBase;
    public GameObject shapeContainer;

    public Sprite defaultImg;

    [SerializeField] public Sprite[] cookieImg;
    private Dictionary<string, string> cookieFacts = new Dictionary<string, string>();
    //cookieType + ingredient
    private Dictionary<string, int> cookieIngredients = new Dictionary<string, int>();
    //cookieType + shape for key
    private Dictionary<string, int> cookieSupply = new Dictionary<string, int>();

    private GameObject titlescreen, inventory, cookieinfo, shop;

    public bool isPremium = false;

    /// <summary>
    /// On start, close all scenes unless titlescreen
    /// </summary>
    void Start()
    {
        unlockedShapes.Add("Circle");

        ingredients.Add("chocolate");
        ingredients.Add("flour");
        ingredients.Add("sugar");
        ingredients.Add("oatmeal");
        

        foreach (GameObject window in windows)
        {
            switch(window.name.ToLower())
            {
                case "titlescreen":
                    titlescreen = window;
                    break;
                case "inventory":
                    inventory = window;
                    break;
                case "cookieinfo":
                    cookieinfo = window;
                    break;
                case "shop":
                    shop = window;
                    break;
            }
        }
        if(SceneManager.GetActiveScene().name != "Titlescreen")
        {
            closeAllWindows();
        } else
        {
            titlescreen.SetActive(false);
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
            inventory.SetActive(!inventory.activeSelf);
        }

        // Shop is pressed
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
            closeAllWindows(shop);
            shop.SetActive(!shop.activeSelf);
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

    public void openWindow(GameObject windowToOpen)
    {
        Console.WriteLine(windows);

        foreach (GameObject window in windows)
        {
            if (window.Equals(windowToOpen))
            {
                // open said window
                window.SetActive(true);
            }
        }
    }

    public void closeWindow(GameObject windowToClose)
    {

        foreach (GameObject window in windows)
        {
            if (window.Equals(windowToClose))
            {
                //close the window
                window.SetActive(false);
            }
            else
            {
                //do nothingf
            }
        }
    }

    /// <summary>
    /// display the proper cookie screen when pressed
    /// </summary>
    /// <param name="cookieType"> cookie type (linked to dictionary)</param>
    public void CookieInfo(string cookieType)
    {

        closeAllWindows(cookieinfo);
        cookieinfo.SetActive(!cookieinfo.activeSelf);

        // Destroy any old recipe info
        foreach (Transform child in ingredientContainer.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        // Destroy any old shape info
        foreach (Transform child in shapeContainer.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        switch (cookieType)
        {
            case "chocolatechip":
                GetChildWithName(cookieinfo, "CookieName").GetComponent<TextMeshProUGUI>().text = "Chocolate Chip Cookie";
                break;
            case "sugarcookie":
                GetChildWithName(cookieinfo, "CookieName").GetComponent<TextMeshProUGUI>().text = "Sugar Cookie";
                break;
            case "oatmealcookie":
                GetChildWithName(cookieinfo, "CookieName").GetComponent<TextMeshProUGUI>().text = "Oatmeal Cookie";
                break;
            default:
                GetChildWithName(cookieinfo, "CookieName").GetComponent<TextMeshProUGUI>().text = "Unknown Cookie";
                break;

        }

        foreach(Sprite pic in cookieImg)
        {
            if(pic.name.ToLower() == cookieType)
            {
                GetChildWithName(cookieinfo, "CookieImg").GetComponent<Image>().sprite = pic;
            }
        }
        
        GetChildWithName(cookieinfo, "CookieFact").GetComponent<TextMeshProUGUI>().text = cookieFacts[cookieType.ToLower()];

        // Go through all the ingredients and add them dynamically to the list visual
        foreach (string ingredient in ingredients)
        {
            int amt = 0;
            cookieIngredients.TryGetValue(cookieType + ingredient, out amt);
            if (amt != 0)
            {
                GameObject ingredientAddOn = Instantiate(ingredientBase, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                ingredientAddOn.transform.parent = ingredientContainer.transform;
                ingredientAddOn.transform.SetSiblingIndex(0);
                GetChildWithName(ingredientAddOn, "IngrediantText").GetComponent<TextMeshProUGUI>().text = ingredient;
                GetChildWithName(ingredientAddOn, "IngredientNumber").GetComponent<TextMeshProUGUI>().text = amt.ToString();
            }
        }

        // Go through all the unlocked shapes and add them to the list visual

        foreach (string shapes in unlockedShapes)
        {
            GameObject shapeAddOn = Instantiate(shapeBase, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
            shapeAddOn.transform.parent = shapeContainer.transform;
            shapeAddOn.transform.SetSiblingIndex(0);
            GetChildWithName(shapeAddOn, "ShapeText").GetComponent<TextMeshProUGUI>().text = shapes + " shaped";
            GetChildWithName(shapeAddOn, "ShapeAmount").GetComponent<TextMeshProUGUI>().text = cookieSupply[cookieType + shapes].ToString();
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

    /// <summary>
    /// Find a child by name
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    GameObject GetChildWithName(GameObject obj, string name)
    {
        Transform trans = obj.transform;
        Transform childTrans = trans.Find(name);
        if (childTrans != null)
        {
            return childTrans.gameObject;
        }
        else
        {
            return null;
        }
    }

    public void CreateCookie(string cookieType, Dictionary<string, int> ingredients, string cookieFact)
    {
        // If no cookie image is uploaded

        cookieFacts.Add(cookieType.ToLower(), cookieFact);

        // Go through all the ingredients and add them dynamically to the list
        foreach(string ingredient in ingredients.Keys)
        {
            int amt = 0;
            ingredients.TryGetValue(ingredient, out amt);
            if(amt != 0)
            {
                cookieIngredients.Add(cookieType + ingredient, amt);
            }
        }

        // Go through all the unlocked shapes and add them to the list

        PopulateShapes(cookieType);

    }

    /// <summary>
    /// Adds the potential for a shape if it doesn't exist yet for that cookie type.
    /// </summary>
    /// <param name="cookieType"></param>
    void PopulateShapes(string cookieType)
    {
        foreach (string shapes in unlockedShapes)
        {
            if(!cookieSupply.ContainsKey(cookieType + shapes))
            {
                cookieSupply.Add(cookieType + shapes, 0);
            }

        }
    }

    public void CreateAllCookies(Dictionary<string,int>[] cookieRecipes)
    {/*
        Dictionary<string, int> CCCookie = new Dictionary<string, int>();
        CCCookie.Add("chocolate", 10);
        CCCookie.Add("sugar", 5);
        CCCookie.Add("flour", 5);
        */
        
        CreateCookie("chocolatechip", cookieRecipes[0], "Indulge in a heavenly blend of warm, gooey cookie dough and rich, creamy chocolate chips with every bite of a chocolate chip cookie.");
        /*
        Dictionary<string, int> SugerCookie = new Dictionary<string, int>();
        SugerCookie.Add("sugar", 10);
        SugerCookie.Add("flour", 5);
        */
        CreateCookie("sugarcookie", cookieRecipes[1], "The modern sugar cookie was originally called the Nazareth Sugar Cookie, after German Protestants who settled in Nazareth, Pennsylvania, and improved the recipe.");
        /*
        Dictionary<string, int> OatmealCookie = new Dictionary<string, int>();
        OatmealCookie.Add("oatmeal", 10);
        OatmealCookie.Add("sugar", 5);
        OatmealCookie.Add("flour", 5);
        */
        CreateCookie("oatmealcookie", cookieRecipes[2], "Oatmeal cookies are the #1 non-cereal usage for oatmeal, followed by meatloaf and fruit crisp. Oatmeal is heart healthy and March 19th is National Oatmeal Cookie Day.");
    }

    public void purchasePremium(GameObject successText)
    {
        isPremium = true;
        successText.SetActive(true);
    }
}
