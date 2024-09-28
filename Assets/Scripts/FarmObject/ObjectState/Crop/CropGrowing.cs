using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Cysharp.Threading.Tasks;

public class CropGrowing : ObjectState
{
    public bool inGrowing = false;

    public CropGrowing(Plant plant) : base(plant)
    {

    }

    public override void Start()
    {
        Debug.Log("Start Growing");

        (objectMachine as Plant).startDestroyedTimer = false;
        //(objectMachine as Plant).StopCoroutine("DestroyTimer");
        (objectMachine as Plant).inCrafting = false;
        (objectMachine as Plant).rigid = (objectMachine as Plant).GetComponent<Rigidbody>();
        (objectMachine as Plant).rigid.isKinematic = true;
        (objectMachine as Plant).rigid.useGravity = false;
        (objectMachine as Plant).ResetVelocity();
        (objectMachine as Plant).suckableCollider.isTrigger = true;

        //(objectMachine as Plant).plantData.orginalBody = null;
        (objectMachine as Plant).defaultModelPlant.SetActive(false);
        GrowPlant();
    }

    public override void End()
    {
        if ((objectMachine as Plant).state == null) return;
        Debug.Log("End Growing");
    }

    public async void GrowPlant()
    {
        if (!objectMachine) return;
        (objectMachine as Plant).gameObject.SetActive(true);
        (objectMachine as Plant).growingBody.SetActive(true);

        //Debug.Log(objectMachine);
        //Debug.Log((objectMachine as Plant).ownerGarden);
        //Debug.Log((objectMachine as Plant).ownerGarden.waterManager);

        if ((objectMachine as Plant).ownerGarden.waterManager.outOfResource) await UniTask.WaitUntil(() => (objectMachine as Plant).ownerGarden.waterManager.outOfResource == false);
        inGrowing = true;

        (objectMachine as Plant).ownerSlot.countDownUI.StartCountDown((int)((objectMachine as Plant).cropStats.totalGrowingTime));
        //(objectMachine as Plant).ownerSlot.countDownUI.StartCountDown((int)((objectMachine as Plant).cropStats.totalGrowingTime * (objectMachine as Plant).ownerGarden.fertilizerManager.reducingTimeValue));
        if ((objectMachine as Plant).ownerGarden.monitorGardenController.currentSlotIndex == (objectMachine as Plant).ownerSlot.index) (objectMachine as Plant).ownerGarden.monitorGardenController.CheckCropUIState();

        //if ((objectMachine as Plant).ownerGarden.waterManager.outOfResource == false) return;

        (objectMachine as Plant).startGrowingTime = DateTime.UtcNow.ToLocalTime();
        TimeSpan span = TimeSpan.FromSeconds((objectMachine as Plant).cropStats.totalGrowingTime);
        //TimeSpan span = TimeSpan.FromSeconds((objectMachine as Plant).cropStats.totalGrowingTime * (objectMachine as Plant).ownerGarden.fertilizerManager.reducingTimeValue);
        (objectMachine as Plant).endGrowingTime = (objectMachine as Plant).startGrowingTime.Add(span);
        (objectMachine as Plant).StartCoroutine(StartGrowingProcess());
        Debug.Log("StartGrowingProcess");
    }

    IEnumerator StartGrowingProcess()
    {
        //Debug.Log("StartGrowingProcess");
        yield return new WaitForSeconds((objectMachine as Plant).cropStats.totalGrowingTime);
        //yield return new WaitForSeconds((objectMachine as Plant).cropStats.totalGrowingTime * (objectMachine as Plant).ownerGarden.fertilizerManager.reducingTimeValue);
        (objectMachine as Plant).seedOuterEffect.SetActive(false);
        //wholeOuterEffect.SetActive(true);
        CompleteGrowingSession();
        Debug.Log("Growing Process Done");
        //(objectMachine as Plant).growingTime--;
    }

    public async void CompleteGrowingSession()
    {
        //startDestroyedTimer = false;
        //StopCoroutine("DestroyTimer");
        //inCrafting = false;
        foreach (Transform wholePos in (objectMachine as Plant).wholePlantPoss)
        {
            Plant newWholePlant = GameObject.Instantiate((objectMachine as Plant).plantData.wholePlantPrefab, wholePos.position, Quaternion.identity, wholePos).GetComponent<Plant>();
            (objectMachine as Plant).wholePlants.Add(newWholePlant);
            newWholePlant.orginalPlant = (objectMachine as Plant);
            (objectMachine as Plant).wholePlantCount++;

            newWholePlant.onTree = true;
            //newWholePlant.plantData.orginalBody = (objectMachine as Plant).plantData.growingBody;
            newWholePlant.SetState(new CropWhole(newWholePlant));
        }
        inGrowing = false;

        if ((objectMachine as Plant).ownerGarden.monitorGardenController.currentSlotIndex == (objectMachine as Plant).ownerSlot.index) (objectMachine as Plant).ownerGarden.monitorGardenController.CheckCropUIState();

        await UniTask.WaitUntil(() => (objectMachine as Plant).HaveWholeCrop() == false);
        //(objectMachine as Plant).ownerSlot.countDownUI.StartCountDown((int)((objectMachine as Plant).cropStats.totalGrowingTime * (objectMachine as Plant).ownerGarden.fertilizerManager.reducingTimeValue));

        //if ((objectMachine as Plant).ownerGarden.monitorGardenController.currentSlotIndex == (objectMachine as Plant).ownerSlot.index) (objectMachine as Plant).ownerGarden.monitorGardenController.CheckCropUIState();

        GrowPlant();
    }

    public override void ResetCropStats()
    {
        (objectMachine as Plant).wholePlants = new List<Plant>(0);
        inGrowing = false;
    }

    //private void OnDisable()
    //{
    //    (objectMachine as Plant).elapsedTime = (int)DateTime.UtcNow.Subtract((objectMachine as Plant).startGrowingTime.ToLocalTime()).TotalSeconds;
    //    //Debug.Log(elapsedTime);
    //}
}