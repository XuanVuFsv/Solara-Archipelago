using UnityEngine;

public class CropStored : CropState
{
    public CropStored(Plant plant) : base(plant)
    {

    }

    public override void Start()
    {
        Debug.Log("Start Stored");

        cropMachine.startDestroyedTimer = false;
        //cropMachine.StopCoroutine("DestroyTimer");
        cropMachine.inCrafting = false;

        cropMachine.gameObject.transform.position = CollectHandler.Instance.shootingInputData.bulletSpawnPoint.position;

        cropMachine.rigid.velocity = Vector3.zero;
        cropMachine.suckableCollider.isTrigger = false;
        cropMachine.rigid.useGravity = true;

        cropMachine.gameObject.SetActive(false);
        cropMachine.seedOuterEffect.SetActive(true);
        cropMachine.wholeOuterEffect.SetActive(false);
    }

    public override void End()
    {
        if (cropMachine.state == null) return;
        Debug.Log("Start Stored");
    }
}