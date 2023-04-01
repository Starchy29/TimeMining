using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Mathematics;

public class CharacterController : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    private float charSpeed;
    [SerializeField]
    private float decelRate;
    private Rigidbody2D rgb;
    [SerializeField]
    private float speedIncrease;
    private int[] ingredients = new int[4]; //flour,sugar,oatmeal,chocolate
    [SerializeField] private int ingredientCapacity;
    private int ingredientCount = 4;

    public GameObject[] shopShard;
    public GameObject[] inventoryShard;

    void Start()
    {
        rgb = gameObject.GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
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
        return r;
    }

   

    void UpdateInventoryUI()
    {
        //Debug.Log(GameObject.Find("Player Inventory").transform.Find("Red"));
        shopShard[2].gameObject.GetComponent<TextMeshProUGUI>().text = ingredients[0].ToString();
        shopShard[0].gameObject.GetComponent<TextMeshProUGUI>().text = ingredients[1].ToString();
        shopShard[1].gameObject.GetComponent<TextMeshProUGUI>().text = ingredients[2].ToString();
        
    }

    public int[] ReturnInventory()
    {
        return ingredients;
    }
}
