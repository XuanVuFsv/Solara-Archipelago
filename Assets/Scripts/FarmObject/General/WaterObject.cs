using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterObject : Suckable
{
    public enum WaterState
    {
        Salt = 0,
        Fresh = 1,
    }
    [SerializeField]
    WaterState initWaterState;
    public Vector3 suckedPosition;

    // Start is called before the first frame update
    void Start()
    {
        if (initWaterState == WaterState.Salt) SetState(new SaltWater(this));
        else SetState(new FreshWater(this));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void GoToAxieCollector()
    {
        WaterManager.Instance.waterFX_In.GetComponent<ParticleSystem>().Emit(1);
    }

    public override void MoveOut()
    {

    }

    public override void ChangeToStored()
    {

    }

    public override void ChangeToUnStored()
    {

    }
}
