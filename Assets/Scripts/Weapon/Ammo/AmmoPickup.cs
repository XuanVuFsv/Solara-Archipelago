using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : Suckable
{
    public Plant.PlantState plantState;
    public Suckable suckableSample;
    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        suckableCollider = GetComponent<Collider>();
    }
}
