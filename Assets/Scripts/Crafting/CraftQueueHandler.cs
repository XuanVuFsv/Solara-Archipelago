using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VitsehLand.Scripts.Farming.General;
using VitsehLand.Scripts.Stats;

namespace VitsehLand.Scripts.Crafting
{
    public class CraftQueueHandler : MonoBehaviour
    {
        public event Action onCraftCompleted = delegate { };

        public CraftingPresenter craftingManager;
        public CropStats cropStats;
        public Suckable product;

        public GameObject UIContainer;
        public Image productImage;
        public TextMeshProUGUI timeDisplay;

        public float totalTime;
        public bool isReady = true;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (!isReady)
            {
                totalTime = totalTime - Time.deltaTime;
                timeDisplay.text = ((int)totalTime).ToString();
            }
        }

        public void RegisterListener(Action listener)
        {
            onCraftCompleted += listener;
        }

        public IEnumerator CraftProduct(int time)
        {
            productImage.sprite = cropStats.artwork;
            totalTime = time;

            Debug.Log("Start waiting " + (time / product.cropStats.totalProducingTime).ToString() + "energy");
            isReady = false;
            //craftingManager.VFX.SetActive(true);
            //craftingManager.queueActiveQuantity++;

            yield return new WaitForSeconds(time);

            for (int i = 0; i < time / product.cropStats.totalProducingTime; i++)
            {
                Debug.Log("Init");
                GameObject newGameObject = Instantiate(product.gameObject, craftingManager.productPos.position + UnityEngine.Random.value * 0.25f * Vector3.one, Quaternion.identity);
                Debug.Log(newGameObject.name);
            }

            isReady = true;
            //craftingManager.queueActiveQuantity--;
            totalTime = 0;

            //craftingManager.VFX.SetActive(true);
            Debug.Log("Done");

            UIContainer.SetActive(false);
        }

        public void Craft(int time)
        {
            StartCoroutine(CraftProduct(time));
        }
    }
}