using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceUnStored : ObjectState
{
    public ResourceUnStored(PowerContainer power) : base(power)
    {

    }

    public override void Start()
    {
        //Debug.Log("Start Seed");

        (objectMachine as PowerContainer).rigid = (objectMachine as PowerContainer).GetComponent<Rigidbody>();

        (objectMachine as PowerContainer).suckableCollider.isTrigger = false;
        (objectMachine as PowerContainer).rigid.isKinematic = false;
        (objectMachine as PowerContainer).rigid.useGravity = true;

        (objectMachine as PowerContainer).gameObject.SetActive(true);
    }

    public override void End()
    {
        if ((objectMachine as PowerContainer).state == null) return;
        //Debug.Log("End Seed");
    }
}
