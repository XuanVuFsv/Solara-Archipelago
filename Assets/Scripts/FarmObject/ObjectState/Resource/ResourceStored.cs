using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceStored : ObjectState
{
    public ResourceStored(PowerContainer power) : base(power)
    {

    }

    public ResourceStored(NaturalResource resource) : base(resource)
    {

    }

    public ResourceStored(FarmingEssence essence) : base(essence)
    {

    }

    public override void Start()
    {
        //Debug.Log("Start Stored");

        GemManager.Instance.AddGem((objectMachine).ammoStats.gemEarnWhenHaverst);

        (objectMachine).gameObject.transform.position = CollectHandler.Instance.shootingInputData.bulletSpawnPoint.position;

        (objectMachine).rigid.velocity = Vector3.zero;
        (objectMachine).suckableCollider.isTrigger = false;
        (objectMachine).rigid.useGravity = true;

        (objectMachine).gameObject.SetActive(false);

        (objectMachine).transform.parent = null;
    }

    public override void End()
    {
        if ((objectMachine).state == null) return;
        //Debug.Log("End Stored");
    }
}
