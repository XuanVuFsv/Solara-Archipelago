using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static WaterObject;

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

    public WaterResourceManager owner;

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
        if ((WaterManager.Instance.saltWaterContain == WaterManager.Instance.maxSaltWaterContain && CollectHandler.Instance.waterMode == CollectHandler.WaterMode.Salt)
    || (WaterManager.Instance.freshWaterContain == WaterManager.Instance.maxFreshWaterContain && CollectHandler.Instance.waterMode == CollectHandler.WaterMode.Fresh)) return;
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
