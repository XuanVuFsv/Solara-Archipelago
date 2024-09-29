using TMPro;
using UnityEngine;
using VitsehLand.Scripts.Farming.General;

namespace VitsehLand.Scripts.Farming.Resource
{
    public class PowerManager : MonoBehaviour
    {
        public GameObject lightOut, lightFull;
        public float maxPower = 100;
        public float currentPower = 100;

        public RectTransform powerUI;
        public TextMeshProUGUI powerUIText;

        // Start is called before the first frame update
        void Start()
        {
            if (currentPower > 0) TurnLigtFullOn();

            powerUIText.text = ((int)(currentPower / maxPower * 100)).ToString() + " %";
            powerUI.localScale = new Vector3(currentPower / maxPower, 0.9f, 1);
        }

        private void OnTriggerEnter(Collider other)
        {
            Suckable suckable = other.GetComponent<Suckable>();
            if (suckable is PowerContainer)
            {
                if (currentPower >= maxPower)
                {
                    return;
                }

                currentPower += 20;
                if (currentPower > 0) TurnLigtFullOn();

                if (currentPower >= maxPower)
                {
                    currentPower = maxPower;

                    powerUIText.text = "100 %";
                    powerUI.localScale = new Vector3(1, 0.9f, 1);
                }


                Destroy(suckable.gameObject);
                powerUIText.text = ((int)(currentPower / maxPower * 100)).ToString() + " %";
                powerUI.localScale = new Vector3(currentPower / maxPower, 0.9f, 1);
            }
        }

        public bool UsePower(float powerUsed)
        {
            //Debug.Log("Use " + powerUsed.ToString() + " energy");
            //if (currentPower == 0)
            //{
            //    return false;
            //}
            //else if (currentPower - powerUsed < 0) return false;
            //else
            {
                currentPower -= powerUsed;
                if (currentPower <= 0)
                {
                    currentPower = 0;
                    TurnLightOutOn();
                }
                powerUIText.text = ((int)(currentPower / maxPower * 100)).ToString() + " %";
                powerUI.localScale = new Vector3(currentPower / maxPower, 0.9f, 1);
                return true;
            }
        }

        public void TurnLightOutOn()
        {
            lightOut.SetActive(true);
            lightFull.SetActive(false);
        }

        public void TurnLigtFullOn()
        {
            lightOut.SetActive(false);
            lightFull.SetActive(true);
        }
    }
}