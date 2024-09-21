using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CraftQueueHandler : MonoBehaviour
{
    public CraftingManager craftingManager;
    public AmmoStats ammoStats;
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

    public IEnumerator CraftProduct(int time)
    {
        craftingManager.VFX.SetActive(true);
        productImage.sprite = ammoStats.artwork;
        totalTime = time;

        Debug.Log("Start waiting " + (time / product.ammoStats.totalProducingTime).ToString() + "energy");
        isReady = false;
        craftingManager.queueActiveQuantity++;

        yield return new WaitForSeconds(time);

        for (int i = 0; i < time / product.ammoStats.totalProducingTime; i++)
        {
            Debug.Log("Init");
            GameObject newGameObject = Instantiate(product.gameObject, craftingManager.productPos.position + Random.value * 0.25f * Vector3.one, Quaternion.identity);
            Debug.Log(newGameObject.name);
        }

        isReady = true;
        craftingManager.queueActiveQuantity--;
        totalTime = 0;

        craftingManager.VFX.SetActive(true);
        Debug.Log("Done");

        UIContainer.SetActive(false);
    }

    public void Craft(int time)
    {
        StartCoroutine(CraftProduct(time));
    }
}
