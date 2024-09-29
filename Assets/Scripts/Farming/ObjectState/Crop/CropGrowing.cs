using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VitsehLand.Scripts.Farming.ObjectState.Crop
{
    public class CropGrowing : ObjectState
    {
        public bool inGrowing = false;

        public CropGrowing(global::Crop plant) : base(plant)
        {

        }

        public override void Start()
        {
            Debug.Log("Start Growing");

            (objectMachine as global::Crop).startDestroyedTimer = false;
            //(objectMachine as Plant).StopCoroutine("DestroyTimer");
            (objectMachine as global::Crop).inCrafting = false;
            (objectMachine as global::Crop).rigid = (objectMachine as global::Crop).GetComponent<Rigidbody>();
            (objectMachine as global::Crop).rigid.isKinematic = true;
            (objectMachine as global::Crop).rigid.useGravity = false;
            (objectMachine as global::Crop).ResetVelocity();
            (objectMachine as global::Crop).suckableCollider.isTrigger = true;

            //(objectMachine as Plant).plantData.orginalBody = null;
            (objectMachine as global::Crop).defaultModelPlant.SetActive(false);
            GrowPlant();
        }

        public override void End()
        {
            if ((objectMachine as global::Crop).state == null) return;
            Debug.Log("End Growing");
        }

        public async void GrowPlant()
        {
            if (!objectMachine) return;
            (objectMachine as global::Crop).gameObject.SetActive(true);
            (objectMachine as global::Crop).growingBody.SetActive(true);

            //Debug.Log(objectMachine);
            //Debug.Log((objectMachine as Plant).ownerGarden);
            //Debug.Log((objectMachine as Plant).ownerGarden.waterManager);

            if ((objectMachine as global::Crop).ownerGarden.waterManager.outOfResource) await UniTask.WaitUntil(() => (objectMachine as global::Crop).ownerGarden.waterManager.outOfResource == false);
            inGrowing = true;

            (objectMachine as global::Crop).ownerSlot.countDownUI.StartCountDown((objectMachine as global::Crop).cropStats.totalGrowingTime);
            //(objectMachine as Plant).ownerSlot.countDownUI.StartCountDown((int)((objectMachine as Plant).cropStats.totalGrowingTime * (objectMachine as Plant).ownerGarden.fertilizerManager.reducingTimeValue));
            if ((objectMachine as global::Crop).ownerGarden.monitorGardenController.currentSlotIndex == (objectMachine as global::Crop).ownerSlot.index) (objectMachine as global::Crop).ownerGarden.monitorGardenController.CheckCropUIState();

            //if ((objectMachine as Plant).ownerGarden.waterManager.outOfResource == false) return;

            (objectMachine as global::Crop).startGrowingTime = DateTime.UtcNow.ToLocalTime();
            TimeSpan span = TimeSpan.FromSeconds((objectMachine as global::Crop).cropStats.totalGrowingTime);
            //TimeSpan span = TimeSpan.FromSeconds((objectMachine as Plant).cropStats.totalGrowingTime * (objectMachine as Plant).ownerGarden.fertilizerManager.reducingTimeValue);
            (objectMachine as global::Crop).endGrowingTime = (objectMachine as global::Crop).startGrowingTime.Add(span);
            (objectMachine as global::Crop).StartCoroutine(StartGrowingProcess());
            Debug.Log("StartGrowingProcess");
        }

        IEnumerator StartGrowingProcess()
        {
            //Debug.Log("StartGrowingProcess");
            yield return new WaitForSeconds((objectMachine as global::Crop).cropStats.totalGrowingTime);
            //yield return new WaitForSeconds((objectMachine as Plant).cropStats.totalGrowingTime * (objectMachine as Plant).ownerGarden.fertilizerManager.reducingTimeValue);
            (objectMachine as global::Crop).seedOuterEffect.SetActive(false);
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
            foreach (Transform wholePos in (objectMachine as global::Crop).wholePlantPoss)
            {
                global::Crop newWholePlant = UnityEngine.Object.Instantiate((objectMachine as global::Crop).plantData.wholePlantPrefab, wholePos.position, Quaternion.identity, wholePos).GetComponent<global::Crop>();
                (objectMachine as global::Crop).wholePlants.Add(newWholePlant);
                newWholePlant.orginalPlant = objectMachine as global::Crop;
                (objectMachine as global::Crop).wholePlantCount++;

                newWholePlant.onTree = true;
                //newWholePlant.plantData.orginalBody = (objectMachine as Plant).plantData.growingBody;
                newWholePlant.SetState(new CropWhole(newWholePlant));
            }
            inGrowing = false;

            if ((objectMachine as global::Crop).ownerGarden.monitorGardenController.currentSlotIndex == (objectMachine as global::Crop).ownerSlot.index) (objectMachine as global::Crop).ownerGarden.monitorGardenController.CheckCropUIState();

            await UniTask.WaitUntil(() => (objectMachine as global::Crop).HaveWholeCrop() == false);
            //(objectMachine as Plant).ownerSlot.countDownUI.StartCountDown((int)((objectMachine as Plant).cropStats.totalGrowingTime * (objectMachine as Plant).ownerGarden.fertilizerManager.reducingTimeValue));

            //if ((objectMachine as Plant).ownerGarden.monitorGardenController.currentSlotIndex == (objectMachine as Plant).ownerSlot.index) (objectMachine as Plant).ownerGarden.monitorGardenController.CheckCropUIState();

            GrowPlant();
        }

        public override void ResetCropStats()
        {
            (objectMachine as global::Crop).wholePlants = new List<global::Crop>(0);
            inGrowing = false;
        }

        //private void OnDisable()
        //{
        //    (objectMachine as Plant).elapsedTime = (int)DateTime.UtcNow.Subtract((objectMachine as Plant).startGrowingTime.ToLocalTime()).TotalSeconds;
        //    //Debug.Log(elapsedTime);
        //}
    }
}