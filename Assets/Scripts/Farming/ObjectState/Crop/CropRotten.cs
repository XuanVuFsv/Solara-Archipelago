using UnityEngine;

namespace VitsehLand.Scripts.Farming.ObjectState.Crop
{
    public class CropRotten : ObjectState
    {
        public CropRotten(global::Crop plant) : base(plant)
        {

        }

        public override void Start()
        {
            if ((objectMachine as global::Crop).orginalPlant)
            {
                (objectMachine as global::Crop).orginalPlant.wholePlants.Remove(objectMachine as global::Crop);
            }

            //Debug.Log("Start Rotten");
            DestroyThis();
        }

        public override void End()
        {
            if ((objectMachine as global::Crop).state == null) return;
            //Debug.Log("End Rotten");
        }

        public void DestroyThis()
        {
            (objectMachine as global::Crop).startDestroyedTimer = false;
            if ((objectMachine as global::Crop).gameObject) Object.Destroy((objectMachine as global::Crop).gameObject);
        }
    }
}