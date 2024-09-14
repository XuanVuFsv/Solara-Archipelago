using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterObject : Suckable
{
    public enum PlantState
    {
        Salt = 0,
        Fresh = 1,
    }

    public GameObject waterFX;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void GoToAxieCollector()
    {
        base.GoToAxieCollector();
    }

    public override void MoveOut()
    {
        base.MoveOut();
    }

    public override void ChangeToStored()
    {
        base.ChangeToStored();
    }

    public override void ChangeToUnStored()
    {
        base.ChangeToUnStored();
    }
}
