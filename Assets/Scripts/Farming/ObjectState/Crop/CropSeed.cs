using System.Collections;
using UnityEngine;

namespace VitsehLand.Scripts.Farming.ObjectState.Crop
{

    //Note: Remove as
    public class CropSeed : ObjectState
    {
        public CropSeed(global::Crop plant) : base(plant)
        {

        }

        public override void Start()
        {
            //Debug.Log("Start Seed");
            (objectMachine as global::Crop).onTree = false;
            //(objectMachine as Plant).transform.parent = null;

            (objectMachine as global::Crop).rigid = (objectMachine as global::Crop).GetComponent<Rigidbody>();

            (objectMachine as global::Crop).suckableCollider.isTrigger = false;
            (objectMachine as global::Crop).rigid.isKinematic = false;
            (objectMachine as global::Crop).rigid.useGravity = true;

            //(objectMachine as Plant).gameObject.transform.position = CollectHandler.Instance.shootingInputData.bulletSpawnPoint.position;

            (objectMachine as global::Crop).gameObject.SetActive(true);
            (objectMachine as global::Crop).defaultModelPlant.SetActive(true);
            (objectMachine as global::Crop).seedOuterEffect.GetComponent<ParticleSystem>().Play(true);

            (objectMachine as global::Crop).StartCoroutine(DestroyTimer());
        }

        public override void End()
        {
            if ((objectMachine as global::Crop).state == null) return;
            //Debug.Log("End Seed");
            (objectMachine as global::Crop).StopCoroutine(DestroyTimer());
        }

        IEnumerator DestroyTimer()
        {
            (objectMachine as global::Crop).startDestroyedTimer = true;
            int t = Random.Range((objectMachine as global::Crop).minDestroyTime, (objectMachine as global::Crop).maxDestroyTime);
            yield return new WaitForSeconds(t);
            if ((objectMachine as global::Crop).startDestroyedTimer && !(objectMachine as global::Crop).inCrafting)
            {
                (objectMachine as global::Crop).SetState(new CropRotten(objectMachine as global::Crop));
            }
        }
    }
}