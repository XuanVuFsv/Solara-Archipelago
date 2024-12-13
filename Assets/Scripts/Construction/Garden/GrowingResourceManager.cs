using UnityEngine;
using VitsehLand.Assets.Scripts.Interactive;

namespace VitsehLand.Scripts.Construction.Garden
{
    public class GrowingResourceManager : ActivateBehaviour
    {
        public float fullResourceValue = 100;
        [SerializeField]
        protected float currentResourceValue = 0;
        public float CurrentResourceValue
        {
            get { return currentResourceValue; }
        }

        public float timeToUseAllResource;
        public bool outOfResource = true;

        protected float t = 0;

        // Update is called once per frame
        //void Update()
        //{
        //    if (!outOfResource && timeToUseAllResource > 0)
        //    {
        //        currentResourceValue -= fullResourceValue / timeToUseAllResource * Time.deltaTime;
        //        if (currentResourceValue <= 0) StopUseResource();
        //    }
        //}

        public override void ExecuteActivateAction()
        {
            RefillResource();
        }

        public virtual void StopUseResource()
        {
            outOfResource = true;
        }

        public virtual void RefillResource()
        {

        }

        public float GetCurrentResourceValueRatio()
        {
            return (float)(currentResourceValue / fullResourceValue);
        }
    }
}