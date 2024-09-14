using UnityEngine;

public class CropStored : ObjectState
{
    public CropStored(Plant plant) : base(plant)
    {

    }

    public override void Start()
    {
        //Debug.Log("Start Stored");
        if ((objectMachine as Plant).orginalPlant)
        {
            (objectMachine as Plant).orginalPlant.wholePlants.Remove((objectMachine as Plant));
        }

        GemManager.Instance.AddGem((objectMachine as Plant).ammoStats.gemEarnWhenHaverst);

        (objectMachine as Plant).startDestroyedTimer = false;
        //(objectMachine as Plant).StopCoroutine("DestroyTimer");
        (objectMachine as Plant).inCrafting = false;

        (objectMachine as Plant).gameObject.transform.position = CollectHandler.Instance.shootingInputData.bulletSpawnPoint.position;

        (objectMachine as Plant).rigid.velocity = Vector3.zero;
        (objectMachine as Plant).suckableCollider.isTrigger = false;
        (objectMachine as Plant).rigid.useGravity = true;

        (objectMachine as Plant).gameObject.SetActive(false);
        (objectMachine as Plant).seedOuterEffect.SetActive(true);
        (objectMachine as Plant).wholeOuterEffect.SetActive(false);

        (objectMachine as Plant).transform.parent = null;
    }

    public override void End()
    {
        if ((objectMachine as Plant).state == null) return;
        //Debug.Log("End Stored");
    }
}