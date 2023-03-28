using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class DrillBehavior : MonoBehaviour
{

    public enum DrillState
    {
        Idle,
        Drilling,
        Moving,
        Mining
    }

    public DrillState drillState;   // Drills current behavior
    public Transform movePoint;     // Space in front
    private float currentTimer;
    [HideInInspector] public Animator Animator { get; private set; }

    [SerializeField] private float moveSpeed;
    [SerializeField] private Vector2Int oreRange;
     
    [HideInInspector] public float DrillingTime { get; set; }
    [HideInInspector] public float MiningTime { get; set; }
    [HideInInspector] public int OresGathered { get; private set; }
    [HideInInspector] public CaveGenerator GridRef { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        movePoint.parent = null;
        currentTimer = 0;
        OresGathered = 0;
        Animator = GetComponent<Animator>();
        Animator.SetBool("IsDrilling", true);
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

    // Helper Function for detecting the wall closes to the player
    public void WallDetection()
    {
        Vector2Int upcomingTile = GridRef.GetTilemapPos(movePoint.position);
        WallType upcomingWall = GridRef.GetWallType(upcomingTile.x, upcomingTile.y);
        switch (upcomingWall)
        {

            // Drill the wall
            case WallType.Rock:
                drillState = DrillState.Drilling;
                break;

            // Mine the ore
            case WallType.SpeedCrystal:
                drillState = DrillState.Mining;
                movePoint.position = transform.position;
                break;
            case WallType.ReverseCrystal:
                drillState = DrillState.Mining;
                movePoint.position = transform.position;
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
        if (currentTimer >= DrillingTime)
        {
            // Destroy the grid
            currentTimer = 0;
            // Continue Moving 
            drillState= DrillState.Moving;
            Animator.SetBool("IsDrilling", true);
        }
    }

    public void MiningOre()
    {
        currentTimer += Time.deltaTime;
        if (currentTimer >= DrillingTime)
        {
            // Destroy the grid
            OresGathered = Random.Range(oreRange.x, oreRange.y);

            currentTimer = 0;
            drillState = DrillState.Idle;
            Animator.SetBool("IsDrilling", false);
        }
    }

    // Collide with another robot
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision);
        if (collision.gameObject.tag == "Robot")
        {
            drillState = DrillState.Idle;
            Animator.SetBool("IsDrilling", false);

            DrillBehavior otherRobot = collision.gameObject.GetComponent<DrillBehavior>();
            otherRobot.drillState = DrillState.Idle;
            otherRobot.Animator.SetBool("IsDrilling", false);

            Vector2 roundedRobot1 = new Vector2(movePoint.position.x, movePoint.position.y); //new Vector2(RoundToNearestHalf(transform.position.x), RoundToNearestHalf(transform.position.y));
            Vector2 roundedRobot2 = new Vector2(otherRobot.movePoint.position.x, otherRobot.movePoint.position.y); //new Vector2(RoundToNearestHalf(otherRobot.transform.position.x), RoundToNearestHalf(otherRobot.transform.position.y));

            if (Vector2.Distance(roundedRobot1, roundedRobot2) == 0)
            {
                Vector2 absolutePos1 = new Vector2(Mathf.Abs(transform.position.x), Mathf.Abs(transform.position.y));
                Vector2 absolutePos2 = new Vector2(Mathf.Abs(collision.transform.position.x), Mathf.Abs(collision.transform.position.y));
                
                // if point 1 is futher  
                if (absolutePos1.sqrMagnitude>absolutePos2.sqrMagnitude)
                {
                    movePoint.position += transform.up;
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
                transform.position = roundedRobot1;
                collision.gameObject.transform.position = roundedRobot2;
            }
            
            
        }
    }

    public static float RoundToNearestHalf(float number)
    {
        float roundedNumber = (float)Math.Round(number * 2, MidpointRounding.AwayFromZero) / 2;
        if (number % 1 == 0.5f)
        {
            roundedNumber = number;
        }
        return roundedNumber;
    }


}
