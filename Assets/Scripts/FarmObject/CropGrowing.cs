using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CropGrowing : CropState
{
    public bool inGrowing = false;

    public CropGrowing(Plant plant) : base(plant)
    {

    }

    public override void Start()
    {
        Debug.Log("Start Growing");
        inGrowing = true;

        cropMachine.startDestroyedTimer = false;
        //cropMachine.StopCoroutine("DestroyTimer");
        cropMachine.inCrafting = false;
        cropMachine.rigid = cropMachine.GetComponent<Rigidbody>();
        cropMachine.rigid.isKinematic = true;
        cropMachine.rigid.useGravity = false;
        cropMachine.ResetVelocity();
        cropMachine.suckableCollider.isTrigger = true;

        //cropMachine.plantData.orginalBody = null;
        cropMachine.defaultModelPlant.SetActive(false);
        GrowPlant();
    }

    public override void End()
    {
        if (cropMachine.state == null) return;
        Debug.Log("End Growing");
    }

    void GrowPlant()
    {
        cropMachine.gameObject.SetActive(true);
        cropMachine.growingBody.SetActive(true);

        cropMachine.startGrowingTime = DateTime.UtcNow.ToLocalTime();
        TimeSpan span = TimeSpan.FromSeconds(cropMachine.ammoStats.totalGrowingTime);
        cropMachine.endGrowingTime = cropMachine.startGrowingTime.Add(span);
        cropMachine.StartCoroutine(StartGrowingProcess());
        Debug.Log("StartGrowingProcess");
    }

    IEnumerator StartGrowingProcess()
    {
        Debug.Log("StartGrowingProcess");
        yield return new WaitForSeconds(cropMachine.ammoStats.totalGrowingTime);
        cropMachine.seedOuterEffect.SetActive(false);
        //wholeOuterEffect.SetActive(true);
        CompleteGrowingSession();
        Debug.Log("Growing Process Done");
        cropMachine.growingTime--;
    }

    public void CompleteGrowingSession()
    {
        //startDestroyedTimer = false;
        //StopCoroutine("DestroyTimer");
        //inCrafting = false;
        foreach (Transform wholePos in cropMachine.wholePlantPoss)
        {
            Plant newWholePlant = GameObject.Instantiate(cropMachine.plantData.wholePlantPrefab, wholePos.position, Quaternion.identity, wholePos).GetComponent<Plant>();
            cropMachine.wholePlants.Add(newWholePlant);
            newWholePlant.orginalPlant = cropMachine;
            cropMachine.wholePlantCount++;

            newWholePlant.onTree = true;
            //newWholePlant.plantData.orginalBody = cropMachine.plantData.growingBody;
            newWholePlant.SetState(new CropWhole(newWholePlant));
        }
        inGrowing = false;
    }

    public override void ResetCropStats()
    {
        cropMachine.wholePlants = new List<Plant>(0);
        inGrowing = false;
    }

    //private void OnDisable()
    //{
    //    cropMachine.elapsedTime = (int)DateTime.UtcNow.Subtract(cropMachine.startGrowingTime.ToLocalTime()).TotalSeconds;
    //    //Debug.Log(elapsedTime);
    //}
}