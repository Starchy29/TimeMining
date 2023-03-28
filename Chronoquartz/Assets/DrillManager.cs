using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DrillManager : MonoBehaviour
{
 
    [SerializeField]
    private GameObject drillPrefab;
    public List<DrillBehavior> activeDrills;
    public CaveGenerator grid;
    [SerializeField] float defaultDrillingTimes;
    [SerializeField] float defaultMiningTimes;
    [field: SerializeField]
    public int DrillsAvailable { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        activeDrills= new List<DrillBehavior>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 defaultPos = new Vector2(-0.5f, 0.5f);
        if (Input.GetKeyDown(KeyCode.D) )
        {
            SummonDrill(defaultPos, -90);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            SummonDrill(defaultPos, 90);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            SummonDrill(defaultPos, -180);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            SummonDrill(defaultPos, 0);
        }

        MoveActiveDrills();
    }

    // Summon a drill based on position and direction
    void SummonDrill (Vector2 pos, float dir) 
    {
        if (DrillsAvailable == 0) return;
        DrillsAvailable--;

        GameObject drill = Instantiate(drillPrefab);
        drill.transform.position = pos;
        drill.transform.Rotate(new Vector3(0,0,dir));
        DrillBehavior drillBehaviord = drill.GetComponent<DrillBehavior>();
        
        // Set it's defaluts
        drillBehaviord.drillState = DrillBehavior.DrillState.Moving;
        drillBehaviord.MiningTime = defaultMiningTimes;
        drillBehaviord.DrillingTime = defaultDrillingTimes;
        drillBehaviord.GridRef = GameObject.Find("Grid").GetComponent<CaveGenerator>();

        activeDrills.Add(drill.GetComponent<DrillBehavior>());
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

                // Do nothing
                case DrillBehavior.DrillState.Idle:
                    break;
            }
        }
    }
}
