using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DrillManager : MonoBehaviour
{
 
    [SerializeField]
    private GameObject drillPrefab;
    public List<DrillBehavior> activeDrills;
    private UIManager UICookieCount;
    public CaveGenerator grid;
    [SerializeField] float defaultDrillingTimes;
    [SerializeField] float defaultMiningTimes;
    CharacterController player;
    public AlertManager Alerts { get; set; }
    [field: SerializeField]
    public int DrillsAvailable { get; set; }

    private bool sugarBoost;
    [SerializeField] float sugarTimer;
    private float currentSugarTimer;
    private bool chocolateBoost;
    [SerializeField] float chocolateTimer;
    private float currentChocolateTimer;
    private bool oatmealBoost;
    [SerializeField] float oatmealTimer;
    private float currentOatmealTimer;

    // Start is called before the first frame update
    void Start()
    {
        activeDrills= new List<DrillBehavior>();
        player = GameObject.Find("Player").GetComponent<CharacterController>();
        Alerts = GameObject.Find("UIManager").GetComponent<AlertManager>();
        UICookieCount = GameObject.Find("UIManager").GetComponent<UIManager>();
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.L) )
        {
            //Vector3 drillSpawn = new Vector3(RoundToNearestHalf(player.transform.position.x + 1.0f), RoundToNearestHalf(player.transform.position.y), player.transform.position.z);
            Vector3 drillSpawn = new Vector3(RoundToNearestHalf(player.transform.position.x), RoundToNearestHalf(player.transform.position.y), player.transform.position.z);
            
            SummonDrill(drillSpawn, -90);
            alerts.AddAlert("Drill created going right!");
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            // Vector3 drillSpawn = new Vector3(RoundToNearestHalf(player.transform.position.x -1.0f) , RoundToNearestHalf(player.transform.position.y), player.transform.position.z);
            Vector3 drillSpawn = new Vector3(RoundToNearestHalf(player.transform.position.x) , RoundToNearestHalf(player.transform.position.y), player.transform.position.z);
            SummonDrill(drillSpawn, 90);
            alerts.AddAlert("Drill created going left!");
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            //Vector3 drillSpawn = new Vector3(RoundToNearestHalf(player.transform.position.x), RoundToNearestHalf(player.transform.position.y- 1.0f), player.transform.position.z);
            Vector3 drillSpawn = new Vector3(RoundToNearestHalf(player.transform.position.x), RoundToNearestHalf(player.transform.position.y), player.transform.position.z);
            SummonDrill(drillSpawn, -180);
            alerts.AddAlert("Drill created going downward!");
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            //Vector3 drillSpawn = new Vector3(RoundToNearestHalf(player.transform.position.x), RoundToNearestHalf(player.transform.position.y + 1.0f), player.transform.position.z);
            Vector3 drillSpawn = new Vector3(RoundToNearestHalf(player.transform.position.x), RoundToNearestHalf(player.transform.position.y), player.transform.position.z);
            SummonDrill(drillSpawn, 0);
            alerts.AddAlert("Drill created going upward!");
        }
        */

        if (Input.GetKeyDown(KeyCode.F))
        {
            // Summon it to the player's closest grid tile
            Vector3 drillSpawn = new Vector3(RoundToNearestHalf(player.transform.position.x), RoundToNearestHalf(player.transform.position.y), player.transform.position.z);
            
            // Summon it to the nearest 90% angle
            SummonDrill(drillSpawn, (Mathf.Round(player.transform.eulerAngles.z / 90) * 90));
            
        }

        MoveActiveDrills();
        UseCookie();
        BoostedTimer();

    }

    public void IncrimentBotBuild()
    {
        DrillsAvailable++;
    }

    public void ActivateSugarCookie()
    {
        if (!sugarBoost)
        { 

            if (UICookieCount.cookieSupply["sugarcookie" + "Circle"] > 0)
            {
                UICookieCount.UpdateCookieCount("sugarcookie", "Circle", -1);



                Alerts.AddAlert("Sugar Boost Activate!");
                sugarBoost = true;
                foreach (var drill in activeDrills)
                {
                    drill.ToggleSpeedBoost(true);
                }
            }
            else
            {
                Alerts.AddAlert("No Cookie for Sugar Boost!");
            }
        }
    }


    public void ActivateChocolateCookie()
    {
        if (!chocolateBoost)
        {

            //Debug.Log(UICookieCount.cookieSupply["chocolatechip" + "Circle"]);
            if (UICookieCount.cookieSupply["chocolatechip" + "Circle"] > 0)
            {
                UICookieCount.UpdateCookieCount("chocolatechip", "Circle", -1);

                Alerts.AddAlert("Chocolate Chunk Boost Activate!");
                chocolateBoost = true;
                foreach (var drill in activeDrills)
                {
                    drill.ToggleResourceBoost(true);
                }
            }
            else
            {
                Alerts.AddAlert("No Cookie for Chocolate Boost!");
            }
        }
    }

    public void ActivateOatmealCookie()
    {
        if (!oatmealBoost)
        { 
                //Debug.Log(UICookieCount.cookieSupply["oatmealcookie" + "Circle"]);

            if (UICookieCount.cookieSupply["oatmealcookie" + "Circle"] > 0)
            {
                UICookieCount.UpdateCookieCount("oatmealcookie", "Circle", -1);

                Alerts.AddAlert("Healthy Oatmeal Boost Activate!");
                oatmealBoost = true;
                foreach (var drill in activeDrills)
                {
                    drill.BoostedInactiveTimer(true);
                }
            }
            else
            {
                Alerts.AddAlert("No Cookie for Oatmeal Boost!");
            }
        }
    }


    void UseCookie()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ActivateSugarCookie();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ActivateChocolateCookie();
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ActivateOatmealCookie();
        }
    }

    void BoostedTimer()
    {
        if (sugarBoost)
        {
            currentSugarTimer += Time.deltaTime;
            if (sugarTimer <= currentSugarTimer)
            {
                sugarBoost = false;
                Alerts.AddAlert("Sugar Boost Deactivate!");
                currentSugarTimer = 0;
                foreach (var drill in activeDrills)
                {
                    drill.ToggleSpeedBoost(false);
                }
            }
        }
        if (chocolateBoost)
        {
            currentChocolateTimer += Time.deltaTime;
            if (chocolateTimer <= currentChocolateTimer)
            {
                chocolateBoost = false;
                Alerts.AddAlert("Chocolate Chunk Deactivate!");
                currentChocolateTimer = 0;
                foreach (var drill in activeDrills)
                {
                    drill.ToggleResourceBoost(false);
                }
            }
        }
        if (oatmealBoost)
        {
            currentOatmealTimer += Time.deltaTime;
            if (oatmealTimer <= currentOatmealTimer)
            {
                oatmealBoost = false;
                Alerts.AddAlert("Healthy Oatmeal Deactivate!");
                currentOatmealTimer = 0;
                foreach (var drill in activeDrills)
                {
                    drill.BoostedInactiveTimer(false);
                }
            }
        }

    }

    // Summon a drill based on position and direction
    void SummonDrill (Vector2 pos, float dir) 
    {
        if (DrillsAvailable == 0) return;
        DrillsAvailable--;

        Alerts.AddAlert("Deploying Drill!");

        GameObject drill = Instantiate(drillPrefab);
        drill.transform.position = pos;
        drill.transform.Rotate(new Vector3(0,0,dir));
        DrillBehavior drillBehaviord = drill.GetComponent<DrillBehavior>();


        // Set it's defaluts
        drillBehaviord.DeployDrill();
        drillBehaviord.drillState = DrillBehavior.DrillState.Moving;
        drillBehaviord.MiningTime = defaultMiningTimes;
        drillBehaviord.DrillingTime = defaultDrillingTimes;
        drillBehaviord.GridRef = GameObject.Find("Grid").GetComponent<CaveGenerator>();

       
        if (sugarBoost) { drillBehaviord.ToggleSpeedBoost(true); }
        if (chocolateBoost) { drillBehaviord.ToggleResourceBoost(true); }
        if (oatmealBoost) { drillBehaviord.BoostedInactiveTimer(true); }

        activeDrills.Add(drill.GetComponent<DrillBehavior>());
    }

    public void removeDrill(DrillBehavior drill, WallType walltype)
    {
        DrillsAvailable++;
        int[] ores = new int[3];
        switch(walltype)
        {
            case WallType.Sugar:
                Debug.Log("Sugar collected : " + drill.OresGathered);
                ores = new int[]{drill.OresGathered, 0, 0};
                break;
            case WallType.Oatmeal:
                Debug.Log("Oatmeal collected : " + drill.OresGathered);
                ores = new int[]{ 0, drill.OresGathered, 0 };
                break;
            case WallType.Chocolate:
                Debug.Log("Chocolate collected : " + drill.OresGathered);
                ores = new int[]{ 0, 0, drill.OresGathered};
                break;
            default:
                ores = new int[] { 0, 0, 0};
                break;
        }
        
        player.UpdateIngredients(ores);
        activeDrills.Remove(drill);
        Alerts.AddAlert("Drill removed!");
        Destroy(drill.gameObject);
    }

    void MoveActiveDrills()
    {
        foreach (var drill in activeDrills)
        {
            switch (drill.drillState)
            {
                
                case DrillBehavior.DrillState.Drilling:
                    drill.DrillingWall();
                    break;
                case DrillBehavior.DrillState.Mining:
                    drill.MiningOre();
                    break;
                case DrillBehavior.DrillState.Moving:
                    drill.Move();
                    break;

                case DrillBehavior.DrillState.Idle:
                    if(drill != null) drill.InactiveTimer();
                    break;

                case DrillBehavior.DrillState.Inactive:
                    break;
            }
        }
    }

    // From ChatGPT
    public static float RoundToNearestHalf(float num)
    {
        if (Math.Abs(num % 1) >= 0.25 && Math.Abs(num % 1) < 0.75) // Check if num is close to a half
        {
            float roundedNum = (float)(Math.Round(Math.Abs(num) * 2) / 2.0); // Round to nearest half decimal point
            if (num < 0) // Check if num is negative
            {
                roundedNum *= -1; // If num is negative, make the roundedNum negative as well
            }
            return roundedNum;
        }
        else // Otherwise, round to the nearest integer
        {
            return (float)Math.Round(num) + 0.5f;
        }
    }
}