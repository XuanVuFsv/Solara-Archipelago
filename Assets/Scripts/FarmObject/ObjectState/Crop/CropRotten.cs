using System.Collections;
using UnityEngine;

public class CropRotten : ObjectState
{
    public CropRotten(Plant plant) : base(plant)
    {

    }

    public override void Start()
    {
        if ((objectMachine as Plant).orginalPlant)
        {
            (objectMachine as Plant).orginalPlant.wholePlants.Remove((objectMachine as Plant));
        }

        //Debug.Log("Start Rotten");
        DestroyThis();
    }

    public override void End()
    {
        if ((objectMachine as Plant).state == null) return;
        //Debug.Log("End Rotten");
    }

    public void DestroyThis()
    {
        (objectMachine as Plant).startDestroyedTimer = false;
        if ((objectMachine as Plant).gameObject) GameObject.Destroy((objectMachine as Plant).gameObject);
    }
}