using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
    private int[] shards = new int[3];
    private int[] gems = new int[3];
    private int shardCapacity;
    private int gemCapacity;
    private int gemCount = 3;

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
    }

    void IncreaseSpeed()
    {
        charSpeed += speedIncrease;
    }

    void ResetSpeed()
    {
        charSpeed -= speedIncrease;
    }

    public int[] UpdateShards(int []r)
    {
        for (int i = 0; i < gemCount; i++)
            if (shards[i] + r[i] <= shardCapacity)
            {
                shards[i] += r[i];
                r[i] = 0;
            }
            else
            {
                shards[i] = shardCapacity;
                r[i] -= shardCapacity;
            }
        UpdateInventoryUI();
        return r;
    }

    public int[] UpdateGems(int []r)
    {
        for (int i = 0; i < gemCount; i++)
            if (gems[i] + r[i] <= gemCapacity)
            {
                gems[i] += r[i];
                r[i] = 0;
            }
            else
            {
                gems[i] = gemCapacity;
                r[i] -= gemCapacity;
            }
        UpdateInventoryUI();
        return r;
    }

    void UpdateInventoryUI()
    {
        GameObject.Find("PlayerInventory").transform.Find("RedShardNumber").gameObject.GetComponent<TextMeshProUGUI>().text = shards[0].ToString();
        GameObject.Find("PlayerInventory").transform.Find("BlueShardNumber").gameObject.GetComponent<TextMeshProUGUI>().text = shards[1].ToString();
        GameObject.Find("PlayerInventory").transform.Find("GreenShardNumber").gameObject.GetComponent<TextMeshProUGUI>().text = shards[2].ToString();

        GameObject.Find("PlayerInventory").transform.Find("RedCrystalNumber").gameObject.GetComponent<TextMeshProUGUI>().text = gems[0].ToString();
        GameObject.Find("PlayerInventory").transform.Find("BlueCrystalNumber").gameObject.GetComponent<TextMeshProUGUI>().text = gems[1].ToString();
        GameObject.Find("PlayerInventory").transform.Find("GreenCrystalNumber").gameObject.GetComponent<TextMeshProUGUI>().text = gems[2].ToString();
    }
}
