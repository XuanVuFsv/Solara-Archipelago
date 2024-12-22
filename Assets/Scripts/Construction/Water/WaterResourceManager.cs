using UnityEngine;
using VitsehLand.Assets.Scripts.Weapon.Collector;
using VitsehLand.Scripts.Construction.Garden;
using VitsehLand.Scripts.Farming.General;
using VitsehLand.Scripts.Ultilities;

namespace VitsehLand.Scripts.Construction.Water
{
    public class WaterResourceManager : GrowingResourceManager
    {
        public WaterObject.WaterState waterState;
        public WaterObject container;

        void Update()
        {
            if (!outOfResource && timeToUseAllResource > 0)
            {
                currentResourceValue = currentResourceValue - fullResourceValue / timeToUseAllResource * Time.deltaTime;
                if (currentResourceValue <= 0) StopUseResource();
            }
        }

        public void UseWater(float ammount)
        {
            currentResourceValue -= ammount;
            if (currentResourceValue <= 0)
            {
                currentResourceValue = 0;
                outOfResource = true;
                return;
            }
        }

        public void AddWater(float ammount)
        {
            outOfResource = false;
            currentResourceValue += ammount;

            if (currentResourceValue >= fullResourceValue)
            {
                currentResourceValue = fullResourceValue;
                return;
            }
        }

        public override void RefillResource()
        {
            MyDebug.Log("Check Resource");
            if (waterState == WaterObject.WaterState.Salt && CollectHandler.Instance.waterMode == CollectHandler.WaterMode.Salt
                || waterState == WaterObject.WaterState.Fresh && CollectHandler.Instance.waterMode == CollectHandler.WaterMode.Fresh)
            {
                if (WaterManager.Instance.saltWaterContain > 0 && CollectHandler.Instance.waterMode == CollectHandler.WaterMode.Salt)
                {
                    outOfResource = false;
                    currentResourceValue += CollectHandler.Instance.suckSpeed * Time.deltaTime;
                    if (currentResourceValue >= fullResourceValue)
                    {
                        currentResourceValue = fullResourceValue;
                    }
                }
                else if (WaterManager.Instance.freshWaterContain > 0 && CollectHandler.Instance.waterMode == CollectHandler.WaterMode.Fresh)
                {
                    outOfResource = false;
                    currentResourceValue += CollectHandler.Instance.suckSpeed * Time.deltaTime;
                    if (currentResourceValue >= fullResourceValue)
                    {
                        currentResourceValue = fullResourceValue;
                    }
                }
            }
        }
    }
}