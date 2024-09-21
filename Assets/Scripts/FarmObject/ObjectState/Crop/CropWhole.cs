using System.Collections.Generic;
using UnityEngine;

public class CropWhole : ObjectState
{
    public CropWhole(Plant plant) : base(plant)
    {

    }

    public override void Start()
    {
        //Debug.Log("Start Whole");
        GemManager.Instance.AddGem((objectMachine as Plant).ammoStats.gemEarnWhenHaverst);

        (objectMachine as Plant).startDestroyedTimer = false;
        //StopCoroutine("DestroyTimer");
        (objectMachine as Plant).inCrafting = false;

        (objectMachine as Plant).rigid = (objectMachine as Plant).GetComponent<Rigidbody>();

        (objectMachine as Plant).suckableCollider.isTrigger = true;
        (objectMachine as Plant).rigid.isKinematic = false;
        (objectMachine as Plant).rigid.useGravity = false;

        (objectMachine as Plant).growingBody.SetActive(false);
        (objectMachine as Plant).gameObject.SetActive(true);
        (objectMachine as Plant).defaultModelPlant.SetActive(true);

        SetWholePlantEffect();
    }

    public override void End()
    {
        if ((objectMachine as Plant).state == null) return;

        Debug.Log("Harvest");
        //(objectMachine as Plant).plantData.orginalBody = null;
        (objectMachine as Plant).transform.parent = null;

        (objectMachine as Plant).suckableCollider.isTrigger = false;
        (objectMachine as Plant).rigid.useGravity = true;

        (objectMachine as Plant).orginalPlant.wholePlantCount--;
        if ((objectMachine as Plant).orginalPlant.wholePlantCount == 0)
        {
            (objectMachine as Plant).orginalPlant.state.ResetCropStats();
        }
        (objectMachine as Plant).onTree = false;
    }

    public void SetWholePlantEffect()
    {
        (objectMachine as Plant).seedOuterEffect.SetActive(false);
        (objectMachine as Plant).wholeOuterEffect.SetActive(true);
    }
}