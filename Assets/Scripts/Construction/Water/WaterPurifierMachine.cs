using System.Collections;
using UnityEngine;
using VitsehLand.Assets.Scripts.Interactive;
using VitsehLand.Scripts.Construction.Water;
using VitsehLand.Scripts.Farming.Resource;

namespace VitsehLand.Assets.Scripts.Construction.Water
{
    public class WaterPurifierMachine : ActivateBehaviour
    {
        public WaterResourceManager freshWaterManager;
        public WaterResourceManager saltWaterReceiver;
        public PowerManager powerManager;

        public bool intProcessing = false;
        public int timeFiltration = 0;

        public GameObject VFX;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (intProcessing)
            {
                //float value = 100 / timeFiltration * Time.deltaTime;
                //powerManager.UsePower(value);
                //saltWaterReceiver.UseWater(value);
                //freshWaterManager.AddWater(value);

                powerManager.UsePower(powerManager.maxPower / timeFiltration * Time.deltaTime);
                saltWaterReceiver.UseWater(saltWaterReceiver.fullResourceValue / timeFiltration * Time.deltaTime);
                freshWaterManager.AddWater(saltWaterReceiver.fullResourceValue / timeFiltration * Time.deltaTime);
            }
        }

        public override void ExecuteActivateAction()
        {
            if (!intProcessing)
            {
                if (powerManager.currentPower == powerManager.maxPower && saltWaterReceiver.CurrentResourceValue == saltWaterReceiver.fullResourceValue)
                {
                    StartCoroutine(StartFiltration());
                }
            }
        }

        public IEnumerator StartFiltration()
        {
            VFX.SetActive(true);
            intProcessing = true;
            yield return new WaitForSeconds(timeFiltration);
            intProcessing = false;
            VFX.SetActive(false);
        }
    }
}