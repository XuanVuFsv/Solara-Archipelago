using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterManager : Singleton<WaterManager>
{
    public CollectHandler collectHandler;

    public int saltWaterContain = 0;
    public int freshWaterContain = 0;

    public GameObject waterFX;

    public WaterObject.WaterState currentWaterStateSelected = WaterObject.WaterState.Fresh;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CollectWater(ObjectState waterState)
    {

    }
}
