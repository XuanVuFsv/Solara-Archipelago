using UnityEngine;
using VitsehLand.Assets.Scripts.Farming.General;
using VitsehLand.Scripts.Farming.ObjectState.Crop;
using VitsehLand.Scripts.Weapon.General;

namespace VitsehLand.Scripts.Construction.Garden
{
    public class SeedReceiver : MonoBehaviour
    {
        public GardenManager ownerGarden;
        public MonitorGardenController monitorGardenController;

        private void OnTriggerEnter(Collider other)
        {
            // If garden has not built completely
            if (!ownerGarden.HasBuilt) return;

            //Only Suckable game object like crop
            if (other.gameObject.tag == "Suckable")
            {
                Crop plant = other.gameObject.GetComponent<Crop>();
                if (plant == null) return;

                if (plant.cropStats.featuredType != GameObjectType.FeaturedType.Normal
                    || plant.cropStats.weaponSlot == ActiveWeapon.WeaponSlot.None
                    || plant.cropStats.weaponSlot == ActiveWeapon.WeaponSlot.HandGun
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
                    monitorGardenController.AssignCurrentCropStats(plant.cropStats);
                    monitorGardenController.SetupDisplayCropStats();
                    Debug.Log("Update UI");
                }

                //ownerGarden.inGrowing = true;

                // Assign to slot that has index same as current indext on monitor and that slot is null
                if (ownerGarden.gardenSlotProperties[monitorGardenController.currentSlotIndex].crop == null)
                {
                    ownerGarden.gardenSlotProperties[monitorGardenController.currentSlotIndex].crop = plant;
                    //ownerGarden.gardenSlotProperties[monitorGardenController.currentSlotIndex].countDownUI.StartCountDown(plant.cropStats.totalGrowingTime);

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
                    //ownerGarden.gardenSlotProperties[emptySlotIndex].countDownUI.StartCountDown(plant.cropStats.totalGrowingTime);

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
}