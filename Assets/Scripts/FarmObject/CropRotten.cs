using System.Collections;
using UnityEngine;

public class CropRotten : CropState
{
    public CropRotten(Plant plant) : base(plant)
    {

    }

    public override void Start()
    {
        Debug.Log("Start Rotten");
        DestroyThis();
    }

    public override void End()
    {
        if (cropMachine.state == null) return;
        Debug.Log("End Rotten");
    }

    public void DestroyThis()
    {
        cropMachine.startDestroyedTimer = false;
        if (cropMachine.gameObject) GameObject.Destroy(cropMachine.gameObject);
    }
}