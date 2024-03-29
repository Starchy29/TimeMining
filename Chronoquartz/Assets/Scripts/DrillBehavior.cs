using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class DrillBehavior : MonoBehaviour
{

    public enum DrillState
    {
        Idle,
        Drilling,
        Moving,
        Mining,
        Inactive
    }

    public DrillState drillState;   // Drills current behavior
    public Transform movePoint;     // Space in front
    private float currentTimer;
    private float inacctiveTimer;
    private bool alertInactive;
    private Vector2Int currentWallIndex;
    [HideInInspector] public Animator Animator { get; private set; }

    [SerializeField] private float moveSpeed;

    [SerializeField] private float destroyTime;
    [SerializeField] private float boostedDestroyTime;
    [SerializeField] private Vector2Int oreRange;
    [SerializeField] private DrillManager drillManager;
    [HideInInspector] public WallType upcomingWall;

    [HideInInspector] public float DrillingTime { get; set; }
    [HideInInspector] public float MiningTime { get; set; }
    [HideInInspector] public int OresGathered { get; private set; }
    [HideInInspector] public CaveGenerator GridRef { get; set; }

    [HideInInspector] public bool isResourceBoosted { get; set; }
    [HideInInspector] public bool isSpeedBoosted { get; set; }

    [HideInInspector] public bool isHealthBoosted { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        DeployDrill();
    }

    public void DeployDrill()
    {
        drillManager = GameObject.Find("DrillManager").GetComponent<DrillManager>();
        movePoint.parent = null;
        currentTimer = 0;
        OresGathered = 0;
        inacctiveTimer = 0;
        Animator = GetComponent<Animator>();
        isResourceBoosted = false;
        isSpeedBoosted = false;
        isHealthBoosted = false;
        alertInactive = false;
        Animator.SetBool("IsDrilling", true);
        Animator.SetBool("IsInactive", false);
        Animator.SetBool("IsDestroyed", false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Move on open walkable tile
    public void Move()
    {


        // Move towards the move position
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, movePoint.position) == 0.00)
        {
            // Move the movePosition by one in its current direction
            movePoint.position += transform.up;

            WallDetection();

        }
    }

    

    public void InactiveTimer()
    {
        if (drillState == DrillState.Idle)
        {



            inacctiveTimer += Time.deltaTime;
            if (inacctiveTimer >= destroyTime / 2.0f && !alertInactive)
            {
                alertInactive = true;
                drillManager.Alerts.AddAlert("An inactive drill is about to lose its resources!");

            }

            // Drill loses its resources
            if (inacctiveTimer >= destroyTime && alertInactive)
            {
                drillState = DrillState.Inactive;
                drillManager.Alerts.AddAlert("A drill lost its resources!");
                Animator.SetBool("IsDrilling", false);
                Animator.SetBool("IsInactive", false);
                Animator.SetBool("IsDestroyed", true);
                OresGathered = 0;

            }
        }
    }

    // Helper Function for detecting the wall closes to the player
    public void WallDetection()
    {
        currentWallIndex = GridRef.GetTilemapPos(movePoint.position);
        upcomingWall = GridRef.GetWallType(currentWallIndex);
        switch (upcomingWall)
        {

            // Drill the wall
            case WallType.Rock:
                drillState = DrillState.Drilling;
                break;

            // Mine the ore
            case WallType.Sugar:
            case WallType.Chocolate:
            case WallType.Oatmeal:
                drillState = DrillState.Mining;
                movePoint.position = transform.position;


                break;

            case WallType.Bedrock:
                Animator.SetBool("IsDrilling", false);
                Animator.SetBool("IsInactive", true);
                Animator.SetBool("IsDestroyed", false);
                drillState = DrillState.Idle;
                break;
            // Keep moving
            case WallType.None:
                drillState = DrillState.Moving;
                break;
        }
    }

    public void DrillingWall()
    {
        currentTimer += Time.deltaTime;
        GridRef.DamageTile(currentWallIndex.x, currentWallIndex.y, Time.deltaTime);

        if (GridRef.GetWallHealth(currentWallIndex) <= 0.0f) 
        {
            // Destroy the grid
            currentTimer = 0;

            // Continue Moving 
            drillState = DrillState.Moving;
            Animator.SetBool("IsDrilling", true);
            Animator.SetBool("IsInactive", false);
            Animator.SetBool("IsDestroyed", false);
        }
    }

    public void MiningOre()
    {
        currentTimer += Time.deltaTime;
        GridRef.DamageTile(currentWallIndex.x, currentWallIndex.y, Time.deltaTime);
        if (GridRef.GetWallHealth(currentWallIndex) <= 0.0f)
        {
            // Destroy the grid
            OresGathered = Random.Range(oreRange.x, oreRange.y);


            drillManager.Alerts.AddAlert("Drill mined " + OresGathered + " ingredients");
            currentTimer = 0;
            drillState = DrillState.Idle;
            Animator.SetBool("IsDrilling", false);
            Animator.SetBool("IsInactive", true);
            Animator.SetBool("IsDestroyed", false);

        }
    }
    public void BoostedInactiveTimer(bool toggle)
    {
        if(toggle)
        {
            destroyTime = boostedDestroyTime; 
        }
        else
        {
            destroyTime = destroyTime;
        }
        
    }

    public void ToggleSpeedBoost(bool toggle)
    {
        if (isSpeedBoosted == false && toggle == true)
        {
            isSpeedBoosted = true;
            moveSpeed *= 1.5f;
            DrillingTime /= 1.5f;
            MiningTime /= 1.5f;
        }

        if (isSpeedBoosted == true && toggle == false)
        {
            isSpeedBoosted = false;
            moveSpeed /= 1.5f;
            DrillingTime *= 1.5f;
            MiningTime *= 1.5f;
        }
    }

    public void ToggleResourceBoost(bool toggle)
    {
        if (isResourceBoosted == false && toggle == true)
        {
            isResourceBoosted = true;
            oreRange = new Vector2Int(oreRange.x * 2, oreRange.y * 2);
        }

        if (isResourceBoosted == true && toggle == false)
        {
            isResourceBoosted = false;
            oreRange = new Vector2Int(oreRange.x / 2, oreRange.y / 2);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Player" && (drillState == DrillState.Idle || drillState == DrillState.Inactive))
        {
            movePoint.parent = this.gameObject.transform;
            this.GetComponent<BoxCollider2D>().enabled = false;
            drillManager.removeDrill(this, upcomingWall);
        }
        Debug.Log(collision);
        if (collision.gameObject.tag == "Robot")
        {

            
            drillState = DrillState.Idle;
            Animator.SetBool("IsDrilling", false);
            Animator.SetBool("IsInactive", true);
            Animator.SetBool("IsDestroyed", false);

            DrillBehavior otherRobot = collision.gameObject.GetComponent<DrillBehavior>();
            otherRobot.drillState = DrillState.Idle;
            otherRobot.Animator.SetBool("IsDrilling", false);
            otherRobot.Animator.SetBool("IsInactive", true);
            otherRobot.Animator.SetBool("IsDestroyed", false);

            Vector2 roundedRobot1 = new Vector2(RoundToNearestHalf(transform.position.x), RoundToNearestHalf(transform.position.y));
            Vector2 roundedRobot2 = new Vector2(RoundToNearestHalf(otherRobot.transform.position.x), RoundToNearestHalf(otherRobot.transform.position.y));

            if (Vector2.Distance(roundedRobot1, roundedRobot2) == 0)
            {
                Vector2 absolutePos1 = new Vector2(Mathf.Abs(transform.position.x), Mathf.Abs(transform.position.y));
                Vector2 absolutePos2 = new Vector2(Mathf.Abs(collision.transform.position.x), Mathf.Abs(collision.transform.position.y));

                // if point 1 is futher  
                if (absolutePos1.sqrMagnitude > absolutePos2.sqrMagnitude)
                {
                    

                    movePoint.position += transform.up;
                    currentWallIndex = GridRef.GetTilemapPos(movePoint.position);
                    upcomingWall = GridRef.GetWallType(currentWallIndex);
                    if (upcomingWall != WallType.None)
                    {
                        this.GetComponent<BoxCollider2D>().enabled = false;
                        drillManager.removeDrill(this, upcomingWall);
                        return;
                    }

                    Debug.Log(transform.up);

                }
                else
                {
                    otherRobot.movePoint.position += otherRobot.transform.up;
                    Debug.Log(otherRobot.transform.up);
                }
                transform.position = movePoint.position;
                otherRobot.transform.position = otherRobot.movePoint.position;
            }
            else
            {

                movePoint.position = roundedRobot1;
                currentWallIndex = GridRef.GetTilemapPos(movePoint.position);
                upcomingWall = GridRef.GetWallType(currentWallIndex);
                bool removedDrill = false;
                if (upcomingWall != WallType.None)
                {
                    this.GetComponent<BoxCollider2D>().enabled = false;
                    drillManager.removeDrill(this, upcomingWall);
                    removedDrill = true;
                }
               
                otherRobot.movePoint.position = roundedRobot2;
                otherRobot.currentWallIndex = GridRef.GetTilemapPos(otherRobot.movePoint.position);
                otherRobot.upcomingWall = GridRef.GetWallType(otherRobot.currentWallIndex);
                if (otherRobot.upcomingWall != WallType.None)
                {
                    collision.GetComponent<BoxCollider2D>().enabled = false;
                    drillManager.removeDrill(otherRobot, upcomingWall);
                    removedDrill = true;
                }
                

                if (removedDrill) { return; }

                transform.position = roundedRobot1;
                collision.gameObject.transform.position = roundedRobot2;
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