using System.Collections;
using UnityEngine;

public class CropSeed : CropState
{
    public CropSeed(Plant plant) : base(plant)
    {

    }

    public override void Start()
    {
        //Debug.Log("Start Seed");
        cropMachine.onTree = false;
        //cropMachine.transform.parent = null;

        cropMachine.rigid = cropMachine.GetComponent<Rigidbody>();

        cropMachine.suckableCollider.isTrigger = false;
        cropMachine.rigid.isKinematic = false;
        cropMachine.rigid.useGravity = true;

        //cropMachine.gameObject.transform.position = CollectHandler.Instance.shootingInputData.bulletSpawnPoint.position;

        cropMachine.gameObject.SetActive(true);
        cropMachine.defaultModelPlant.SetActive(true);
        cropMachine.seedOuterEffect.GetComponent<ParticleSystem>().Play(true);

        cropMachine.StartCoroutine(DestroyTimer());
    }

    public override void End()
    {
        if (cropMachine.state == null) return;
        //Debug.Log("End Seed");
        cropMachine.StopCoroutine(DestroyTimer());
    }

    IEnumerator DestroyTimer()
    {
        cropMachine.startDestroyedTimer = true;
        int t = Random.Range(20, 40);
        yield return new WaitForSeconds(t);
        if (cropMachine.startDestroyedTimer && !cropMachine.inCrafting)
        {
            cropMachine.SetState(new CropRotten(cropMachine));
        }
    }
}
