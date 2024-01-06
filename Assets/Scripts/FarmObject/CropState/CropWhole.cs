using System.Collections.Generic;
using UnityEngine;

public class CropWhole : CropState
{
    public CropWhole(Plant plant) : base(plant)
    {

    }

    public override void Start()
    {
        //Debug.Log("Start Whole");

        cropMachine.startDestroyedTimer = false;
        //StopCoroutine("DestroyTimer");
        cropMachine.inCrafting = false;

        cropMachine.rigid = cropMachine.GetComponent<Rigidbody>();

        cropMachine.suckableCollider.isTrigger = true;
        cropMachine.rigid.isKinematic = false;
        cropMachine.rigid.useGravity = false;

        cropMachine.growingBody.SetActive(false);
        cropMachine.gameObject.SetActive(true);
        cropMachine.defaultModelPlant.SetActive(true);

        SetWholePlantEffect();
    }

    public override void End()
    {
        if (cropMachine.state == null) return;

        Debug.Log("Harvest");
        //cropMachine.plantData.orginalBody = null;
        cropMachine.transform.parent = null;

        cropMachine.suckableCollider.isTrigger = false;
        cropMachine.rigid.useGravity = true;

        cropMachine.orginalPlant.wholePlantCount--;
        if (cropMachine.orginalPlant.wholePlantCount == 0)
        {
            cropMachine.orginalPlant.state.ResetCropStats();
        }
        cropMachine.onTree = false;
    }

    public void SetWholePlantEffect()
    {
        cropMachine.seedOuterEffect.SetActive(false);
        cropMachine.wholeOuterEffect.SetActive(true);
    }
}