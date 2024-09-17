using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceStored : ObjectState
{
    public ResourceStored(PowerContainer power) : base(power)
    {

    }

    public override void Start()
    {
        //Debug.Log("Start Stored");

        GemManager.Instance.AddGem((objectMachine as PowerContainer).ammoStats.gemEarnWhenHaverst);

        (objectMachine as PowerContainer).gameObject.transform.position = CollectHandler.Instance.shootingInputData.bulletSpawnPoint.position;

        (objectMachine as PowerContainer).rigid.velocity = Vector3.zero;
        (objectMachine as PowerContainer).suckableCollider.isTrigger = false;
        (objectMachine as PowerContainer).rigid.useGravity = true;

        (objectMachine as PowerContainer).gameObject.SetActive(false);

        (objectMachine as PowerContainer).transform.parent = null;
    }

    public override void End()
    {
        if ((objectMachine as PowerContainer).state == null) return;
        //Debug.Log("End Stored");
    }
}
