using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceUnStored : ObjectState
{
    public ResourceUnStored(PowerContainer power) : base(power)
    {

    }

    public ResourceUnStored(NaturalResource resource) : base(resource)
    {

    }

    public override void Start()
    {
        //Debug.Log("Start Seed");

        (objectMachine).rigid = (objectMachine).GetComponent<Rigidbody>();

        (objectMachine).suckableCollider.isTrigger = false;
        (objectMachine).rigid.isKinematic = false;
        (objectMachine).rigid.useGravity = true;

        (objectMachine).gameObject.SetActive(true);
    }

    public override void End()
    {
        if ((objectMachine).state == null) return;
        //Debug.Log("End Seed");
    }
}
