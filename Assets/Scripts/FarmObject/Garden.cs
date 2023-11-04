using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Garden : MonoBehaviour
{
    public Transform seedPos;
    public bool inGrowing;

    public bool hasWatering;
    public bool hasFertilizing;

    public Plant plant;

    public int waterStaack;
    public int fertilizerStack;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (inGrowing) return;

        if (other.gameObject.tag == "Suckable")
        {
            Debug.Log("Detect Land");
            Plant plant = other.gameObject.GetComponent<Plant>();
            if (plant == null) return;
            if (plant.ammoStats.featuredType != AmmoStats.FeaturedType.Normal || plant.ammoStats.weaponSlot == ActiveWeapon.WeaponSlot.None || plant.ammoStats.weaponSlot == ActiveWeapon.WeaponSlot.HandGun || plant.onTree == true || plant.plantState == Suckable.PlantState.GrowingBody || plant.plantData.orginalBody == null || plant.nonPlanting == true) return;

            inGrowing = true;

            this.plant = plant;
            plant.ChangeToGrowingBody();
            plant.transform.position = seedPos.transform.position;
            plant.transform.eulerAngles = Vector3.zero;
            plant.transform.parent = seedPos;
        }
    }
}
