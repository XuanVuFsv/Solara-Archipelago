using TMPro;
using UnityEngine;
using VitsehLand.Assets.Scripts.Weapon.Collector;
using VitsehLand.Scripts.Farming.General;
using VitsehLand.Scripts.Farming.ObjectState;
using VitsehLand.Scripts.Farming.ObjectState.Water;
using VitsehLand.Scripts.Pattern.Singleton;

namespace VitsehLand.Scripts.Construction.Water
{

    //Logic in this class need to refactor by using event system and put logic UI,Sound to other class
    public class WaterManager : Singleton<WaterManager>
    {
        public CollectHandler collectHandler;

        public float saltWaterContain = 0;
        public float freshWaterContain = 0;

        public int maxSaltWaterContain = 100;
        public int maxFreshWaterContain = 100;

        public GameObject waterFX_In;
        public GameObject waterFX_Out;
        public GameObject waterFX_Collide;

        [Header("Effect")]
        public TextMeshProUGUI saltWaterContainText;
        public TextMeshProUGUI freshWaterContainText;

        public RectTransform saltWaterContainBar;
        public RectTransform freshWaterContainBar;

        // Start is called before the first frame update
        void Start()
        {
            UpdateSaltWaterUI();
            UpdateFreshWaterUI();
        }

        public void CollectWater(WaterObject waterObject, float value)
        {
            ObjectState waterState = waterObject.state;
            //Debug.Log("Collect Water Handle in manager");

            if (waterState is SaltWater && saltWaterContain < maxSaltWaterContain)
            {
                saltWaterContain += value;
                UpdateSaltWaterUI();
            }
            else if (waterState is FreshWater && freshWaterContain < maxFreshWaterContain)
            {
                if (waterObject.owner)
                {
                    if (waterObject.owner.CurrentResourceValue == 0) return;

                    waterObject.owner.UseWater(value);
                    freshWaterContain += value;
                    UpdateFreshWaterUI();
                    return;
                }
                freshWaterContain += value;
                UpdateFreshWaterUI();
            }
        }

        public void BlowWater(float value)
        {
            if (CollectHandler.Instance.waterMode == CollectHandler.WaterMode.Salt)
            {
                if (saltWaterContain <= 0)
                {
                    saltWaterContain = 0;
                    return;
                }
                saltWaterContain -= value;
                UpdateSaltWaterUI();
            }
            else if (CollectHandler.Instance.waterMode == CollectHandler.WaterMode.Fresh)
            {
                if (freshWaterContain <= 0)
                {
                    freshWaterContain = 0;
                    return;
                }
                freshWaterContain -= value;
                UpdateFreshWaterUI();
            }

            waterFX_Out.GetComponent<ParticleSystem>().Emit(1);
            waterFX_Collide.GetComponent<ParticleSystem>().Emit(1);
        }

        public void UpdateSaltWaterUI()
        {
            saltWaterContainText.text = ((int)saltWaterContain).ToString();
            saltWaterContainBar.localScale = new Vector3(saltWaterContain / maxSaltWaterContain, 1, 1);
        }

        public void UpdateFreshWaterUI()
        {
            freshWaterContainText.text = ((int)freshWaterContain).ToString();
            freshWaterContainBar.localScale = new Vector3(freshWaterContain / maxFreshWaterContain, 1, 1);
        }
    }
}