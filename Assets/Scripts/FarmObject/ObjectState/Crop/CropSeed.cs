using System.Collections;
using UnityEngine;

public class CropSeed : ObjectState
{
    public CropSeed(Plant plant) : base(plant)
    {

    }

    public override void Start()
    {
        //Debug.Log("Start Seed");
        (objectMachine as Plant).onTree = false;
        //(objectMachine as Plant).transform.parent = null;

        (objectMachine as Plant).rigid = (objectMachine as Plant).GetComponent<Rigidbody>();

        (objectMachine as Plant).suckableCollider.isTrigger = false;
        (objectMachine as Plant).rigid.isKinematic = false;
        (objectMachine as Plant).rigid.useGravity = true;

        //(objectMachine as Plant).gameObject.transform.position = CollectHandler.Instance.shootingInputData.bulletSpawnPoint.position;

        (objectMachine as Plant).gameObject.SetActive(true);
        (objectMachine as Plant).defaultModelPlant.SetActive(true);
        (objectMachine as Plant).seedOuterEffect.GetComponent<ParticleSystem>().Play(true);

        (objectMachine as Plant).StartCoroutine(DestroyTimer());
    }

    public override void End()
    {
        if ((objectMachine as Plant).state == null) return;
        //Debug.Log("End Seed");
        (objectMachine as Plant).StopCoroutine(DestroyTimer());
    }

    IEnumerator DestroyTimer()
    {
        (objectMachine as Plant).startDestroyedTimer = true;
        int t = Random.Range(40, 80);
        yield return new WaitForSeconds(t);
        if ((objectMachine as Plant).startDestroyedTimer && !(objectMachine as Plant).inCrafting)
        {
            (objectMachine as Plant).SetState(new CropRotten((objectMachine as Plant)));
        }
    }
}
