using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Recipe
{
    public string name;
    public int sugarcubes;
    public int oatmeal;
    public int chocolate;

    public Recipe(string n, int s, int o, int c)
    {
        this.name = n;
        sugarcubes = s;
        oatmeal = o;
        chocolate = c;
    }

    public bool isEnough(int [] ing)
    {
        if (ing[1] >= sugarcubes && ing[2] >= oatmeal && ing[3] >= chocolate)
            return true;
        else
            return false;
    }
}


public class CookieMenu : MonoBehaviour
{
    private GameObject player;
    private GameObject UIManager;

    [SerializeField]
    private Recipe[] recipes;

    
    public GameObject[] recipeButtons;
    // Start is called before the first frame update
    void Awake()
    {
        UIManager = GameObject.Find("UIManager");
        UpdateRecipeInfo();
        player = GameObject.FindGameObjectWithTag("Player");
        UpdateButtons();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateButtons()
    {
        int[] playerIngredients = player.GetComponent<CharacterController>().ReturnInventory();
        int index = 0;
        foreach(Recipe r in recipes)
        {
            if (r.isEnough(playerIngredients))
                ActivateRecipeButton(index,true);
            else

            index++;
        }
    }

    
    public void ActivateRecipeButton(int i, bool b)
    {
        if (b)
        {
            recipeButtons[i].GetComponent<Button>().enabled = true;
        }
        else
        {
            recipeButtons[i].GetComponent<Button>().enabled = false;
        }

    }

    public void UpdateRecipeInfo()
    {
        Dictionary<string, int>[] recipeDictionary = new Dictionary<string, int>[3];
        int index = 0;
        Debug.Log(recipes[0].sugarcubes);
        foreach(Recipe r in recipes)
        {
            recipeDictionary[index] = new Dictionary<string, int>();
            if(r.sugarcubes>0)
            recipeDictionary[index].Add("sugar", r.sugarcubes);
            if(r.chocolate>0)
            recipeDictionary[index].Add("chocolate", r.chocolate);
            if(r.oatmeal>0)
            recipeDictionary[index].Add("oatmeal", r.oatmeal);
            index++;
        }
        index = 0;
        UIManager.GetComponent<UIManager>().CreateAllCookies(recipeDictionary);
    }
}
