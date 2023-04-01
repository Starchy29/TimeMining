using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Recipe
{
    public string name;
    public int flour;
    public int sugarcubes;
    public int oatmeal;
    public int chocolate;

    public Recipe(string n, int f, int s, int o, int c)
    {
        this.name = n;
        flour = f;
        sugarcubes = s;
        oatmeal = o;
        chocolate = c;
    }

    public bool isEnough(int [] ing)
    {
        if (ing[0] >= flour && ing[1] >= sugarcubes && ing[2] >= oatmeal && ing[3] >= chocolate)
            return true;
        else
            return false;
    }
}


public class CookieMenu : MonoBehaviour
{
    private GameObject player;

    [SerializeField]
    private Recipe[] recipes;

    
    public GameObject[] recipeButtons;
    // Start is called before the first frame update
    void Awake()
    {
        UpdateButtons();
        player = GameObject.FindGameObjectWithTag("Player");
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
}
