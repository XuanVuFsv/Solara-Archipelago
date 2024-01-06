using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedReceiver : MonoBehaviour
{
    public GardenManager ownerGarden;
    public MonitorGardenController monitorGardenController;

    private void OnTriggerEnter(Collider other)
    {
        // If garden has not built completely
        if (!ownerGarden.HasBuilt) return;
        Debug.Log("Check");

        //Only Suckable game object like crop
        if (other.gameObject.tag == "Suckable")
        {
            Debug.Log("Check");

            //Debug.Log("Detect Crop: " + other.name);
            Plant plant = other.gameObject.GetComponent<Plant>();
            if (plant == null) return;
            Debug.Log("Check");

            //Debug.Log(plant.state);

            //Destroy(other.gameObject);
            if (plant.ammoStats.featuredType != AmmoStats.FeaturedType.Normal
                || plant.ammoStats.weaponSlot == ActiveWeapon.WeaponSlot.None
                || plant.ammoStats.weaponSlot == ActiveWeapon.WeaponSlot.HandGun
                || plant.onTree == true
                || plant.state is CropGrowing
                //|| plant.plantState == Plant.PlantState.GrowingBody
                //|| plant.plantData.orginalBody == null
                || plant.nonPlanting == true) return;

            Debug.Log("Check");

            //Find first slot index empty
            int emptySlotIndex = ownerGarden.FindEmptySlotIndex();
            if (emptySlotIndex == -1) return;
            Debug.Log(emptySlotIndex);

            //Auto assign or bind data to slot that was current slot on monitor
            if (ownerGarden.gardenSlotProperties[monitorGardenController.currentSlotIndex].crop == null)
            {
                monitorGardenController.AssignCurrentCropStats(plant.ammoStats);
                monitorGardenController.SetupDisplayCropStats();
                Debug.Log("Update UI");
            }

            //ownerGarden.inGrowing = true;

            // Assign to slot that has index same as current indext on monitor and that slot is null
            if (ownerGarden.gardenSlotProperties[monitorGardenController.currentSlotIndex].crop == null)
            {
                ownerGarden.gardenSlotProperties[monitorGardenController.currentSlotIndex].crop = plant;
                ownerGarden.gardenSlotProperties[monitorGardenController.currentSlotIndex].countDownUI.StartCountDown(plant.ammoStats.totalGrowingTime);

                plant.ownerGarden = ownerGarden;
                plant.ownerSlot = ownerGarden.gardenSlotProperties[monitorGardenController.currentSlotIndex];

                plant.ChangeToGrowingBody();
                plant.transform.position = ownerGarden.gardenSlotProperties[monitorGardenController.currentSlotIndex].seedPos.transform.position;
                plant.transform.eulerAngles = Vector3.zero;
                plant.transform.parent = ownerGarden.gardenSlotProperties[monitorGardenController.currentSlotIndex].seedPos;
            }
            else // Asign to first empty slot found by FindEmptySlotIndex method if slot on monitor not null
            {
                ownerGarden.gardenSlotProperties[emptySlotIndex].crop = plant;
                ownerGarden.gardenSlotProperties[emptySlotIndex].countDownUI.StartCountDown(plant.ammoStats.totalGrowingTime);

                plant.ownerGarden = ownerGarden;
                plant.ownerSlot = ownerGarden.gardenSlotProperties[emptySlotIndex];

                plant.ChangeToGrowingBody();
                plant.transform.position = ownerGarden.gardenSlotProperties[emptySlotIndex].seedPos.transform.position;
                plant.transform.eulerAngles = Vector3.zero;
                plant.transform.parent = ownerGarden.gardenSlotProperties[emptySlotIndex].seedPos;
            }
        }
    }
}
