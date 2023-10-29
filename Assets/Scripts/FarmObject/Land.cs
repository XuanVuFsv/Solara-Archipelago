using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Land : MonoBehaviour
{
    public Transform seedPos;
    public bool haveTree;
    public Plant plant;

    // Update is called once per frame
    void Update()
    {
        if (plant == null) haveTree = false; 
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (haveTree) return;

        if (collision.gameObject.tag == "Suckable")
        {
            //Debug.Log("Detect Land");
            Plant plant = collision.gameObject.GetComponent<Plant>();
            if (plant.ammoStats.weaponSlot == ActiveWeapon.WeaponSlot.AttackGun || plant.onTree == true || plant.plantState == Suckable.PlantState.GrowingBody || plant.plantData.orginalBody == null || plant.nonPlanting == true) return;

            haveTree = true;

            this.plant = plant;
            plant.ChangeToGrowingBody();
            plant.transform.position = seedPos.transform.position;
            plant.transform.eulerAngles = Vector3.zero;
            plant.transform.parent = seedPos;
        }
    }
}
