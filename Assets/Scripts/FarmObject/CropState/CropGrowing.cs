using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Cysharp.Threading.Tasks;

public class CropGrowing : CropState
{
    public bool inGrowing = false;

    public CropGrowing(Plant plant) : base(plant)
    {

    }

    public override void Start()
    {
        Debug.Log("Start Growing");

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

    public async void GrowPlant()
    {
        if (!cropMachine) return;
        cropMachine.gameObject.SetActive(true);
        cropMachine.growingBody.SetActive(true);

        //Debug.Log(cropMachine);
        //Debug.Log(cropMachine.ownerGarden);
        //Debug.Log(cropMachine.ownerGarden.waterManager);

        if (cropMachine.ownerGarden.waterManager.outOfResource) await UniTask.WaitUntil(() => cropMachine.ownerGarden.waterManager.outOfResource == false);
        inGrowing = true;

        cropMachine.ownerSlot.countDownUI.StartCountDown((int)(cropMachine.ammoStats.totalGrowingTime * cropMachine.ownerGarden.fertilizerManager.reducingTimeValue));
        if (cropMachine.ownerGarden.monitorGardenController.currentSlotIndex == cropMachine.ownerSlot.index) cropMachine.ownerGarden.monitorGardenController.CheckCropUIState();

        //if (cropMachine.ownerGarden.waterManager.outOfResource == false) return;

        cropMachine.startGrowingTime = DateTime.UtcNow.ToLocalTime();
        TimeSpan span = TimeSpan.FromSeconds(cropMachine.ammoStats.totalGrowingTime * cropMachine.ownerGarden.fertilizerManager.reducingTimeValue);
        cropMachine.endGrowingTime = cropMachine.startGrowingTime.Add(span);
        cropMachine.StartCoroutine(StartGrowingProcess());
        Debug.Log("StartGrowingProcess");
    }

    IEnumerator StartGrowingProcess()
    {
        //Debug.Log("StartGrowingProcess");
        yield return new WaitForSeconds(cropMachine.ammoStats.totalGrowingTime * cropMachine.ownerGarden.fertilizerManager.reducingTimeValue);
        cropMachine.seedOuterEffect.SetActive(false);
        //wholeOuterEffect.SetActive(true);
        CompleteGrowingSession();
        Debug.Log("Growing Process Done");
        //cropMachine.growingTime--;
    }

    public async void CompleteGrowingSession()
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

        if (cropMachine.ownerGarden.monitorGardenController.currentSlotIndex == cropMachine.ownerSlot.index) cropMachine.ownerGarden.monitorGardenController.CheckCropUIState();

        await UniTask.WaitUntil(() => cropMachine.HaveWholeCrop() == false);
        //cropMachine.ownerSlot.countDownUI.StartCountDown((int)(cropMachine.ammoStats.totalGrowingTime * cropMachine.ownerGarden.fertilizerManager.reducingTimeValue));

        //if (cropMachine.ownerGarden.monitorGardenController.currentSlotIndex == cropMachine.ownerSlot.index) cropMachine.ownerGarden.monitorGardenController.CheckCropUIState();

        GrowPlant();
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