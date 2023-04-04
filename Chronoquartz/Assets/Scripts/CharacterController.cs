using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Mathematics;

public class CharacterController : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private float charSpeed;
    [SerializeField] private float decelRate;
    private Rigidbody2D rgb;
    [SerializeField] private float speedIncrease;
    [SerializeField] private int[] ingredients = new int[3] { 0, 0, 0 }; //sugar,oatmeal,chocolate
    [SerializeField] private int ingredientCapacity;
    private int ingredientCount = 3;
    private GameObject UIManager;

    private bool nearOven = false;
    private bool canMove = true;
    public bool Premium { get; private set; }

    public GameObject[] shopShard;
    public GameObject[] inventoryShard;
    public GameObject CookieManager;
    

    void Start()
    {
        CookieManager = GameObject.Find("BakingMenu");
        CookieManager.SetActive(false);
        rgb = gameObject.GetComponent<Rigidbody2D>();
        UIManager = GameObject.Find("UIManager");
        Premium = false;


    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
            {
                rgb.velocity = Vector2.Lerp(rgb.velocity, Vector2.zero, decelRate);
            }
            else
            {
                rgb.velocity = new Vector2(charSpeed * Input.GetAxis("Horizontal"), charSpeed * Input.GetAxis("Vertical"));

            }

            if (new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) != Vector2.zero)
            {
                Quaternion toRotation = Quaternion.LookRotation(Vector3.forward, new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")));
                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, 180.0f * Time.deltaTime);
            }

            if (nearOven)
                if (Input.GetKeyDown(KeyCode.E))
                {
                    canMove = false;
                    rgb.velocity = Vector2.zero;
                   
                    CookieManager.SetActive(true);
                    CookieManager.GetComponent<CookieMenu>().UpdateButtons();
                }
        }
        else
        {
            if (nearOven)
                if (Input.GetKeyDown(KeyCode.E))
                {
                    canMove = true;
                    CookieManager.SetActive(false);
                }
        }
    }

    void IncreaseSpeed()
    {
        charSpeed += speedIncrease;
    }

    void ResetSpeed()
    {
        charSpeed -= speedIncrease;
    }

    public int[] UpdateIngredients(int []r)
    {
        for (int i = 0; i < ingredientCount; i++)
            if (ingredients[i] + r[i] <= ingredientCapacity)
            {
                ingredients[i] += r[i];
                r[i] = 0;
            }
            else
            {
                ingredients[i] = ingredientCapacity;
                r[i] -= ingredientCapacity;
            }
        UpdateInventoryUI();
        Debug.Log("Inventory is now: " + ingredients[0] + " " + ingredients[1] + " " + ingredients[2]);
        return r;
    }

   

    void UpdateInventoryUI()
    {
        UIManager.GetComponent<UIManager>().UpdatePantry(ingredients);
        
    }


        

    public int[] ReturnInventory()
    {
        return ingredients;
    }

    public void NearOven(bool b)
    {
        nearOven = b;
    }
    

    

    public void IngredientsUsed(int s, int o, int c)
    {
        ingredients[0] -= s;
        ingredients[1] -= o;
        ingredients[2] -= c;
        UpdateInventoryUI();
    }

    public void SetPlayerMoveable(bool moveable)
    {
        canMove = moveable;
    }

    public void SetPremium(GameObject successText)
    {
        Debug.Log("premium now");
        successText.SetActive(true);
        Premium = true;
    }
}
