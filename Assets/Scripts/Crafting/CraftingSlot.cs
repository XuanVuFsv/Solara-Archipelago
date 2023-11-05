using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingSlot : MonoBehaviour
{
    public string currentItemName;
    public int count;
    public int maxItemContain;
    public Transform itemPos;

    public GameObject yellowLight, greenLight, redLight;
    public List<Suckable> currentSuckableItems;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag != "Suckable" || CraftingManager.Instance.inCrafting) return;
        //Debug.Log(other.gameObject.name);

        Suckable suckableItem = other.transform.GetComponent<Suckable>();
        if (currentSuckableItems.Count > 0)
        {
            if (suckableItem.ammoStats.name != currentSuckableItems[count - 1].ammoStats.name || count == maxItemContain)
            {
                suckableItem.rigid.AddForce((suckableItem.transform.position - CollectHandler.Instance.shootingInputData.raycastOrigin.position).normalized * 200);
                return;
            }
        }

        suckableItem.rigid.constraints = RigidbodyConstraints.FreezePosition;
        suckableItem.collider.isTrigger = true;
        suckableItem.ResetVelocity();
        suckableItem.rigid.isKinematic = true;
        suckableItem.rigid.useGravity = false;

        suckableItem.transform.position = itemPos.position;
        suckableItem.transform.eulerAngles = Vector3.zero;

        currentSuckableItems.Add(suckableItem);
        count++;
        (suckableItem as Plant).inCrafting = true;

        if (count > 1) currentSuckableItems[currentSuckableItems.Count - 2].gameObject.SetActive(false);
        if (count > 0) currentSuckableItems[currentSuckableItems.Count - 1].gameObject.SetActive(true);

        suckableItem.rigid.constraints = RigidbodyConstraints.None;

        if (count == maxItemContain) TurnFullCraftingItemSlot();
    }

    public void UseItemToCraft()
    {
        TurnCraftingLight();

        if (count > 0)
        {
            //Debug.Log("Craft");
        }
        else return;

        currentSuckableItems[count - 1].gameObject.SetActive(false);

        while(count > 0)
        {
            if (currentSuckableItems[count - 1].gameObject) Destroy(currentSuckableItems[count - 1].gameObject, Random.Range(0, 10));
            currentSuckableItems.Remove(currentSuckableItems[count - 1]);
            count--;
        }
    }

    public string GetCurrentName()
    {
        if (count > 0)
        {
            return count.ToString() + currentSuckableItems[0].ammoStats.name;
        }
        return "0";
    }

    public void CancelCraftingOnThisItem()
    {
        TurnNoneCraftingLight();

        if (count > 0)
        {
            //Debug.Log("Cancel Craft");        
        }
        else return;

        while (count > 0)
        {
            if (currentSuckableItems[count - 1].gameObject)
            {
                Destroy(currentSuckableItems[count - 1].gameObject, Random.Range(0, 10));
                currentSuckableItems[count - 1].gameObject.SetActive(false);
            }
            currentSuckableItems.Remove(currentSuckableItems[count - 1]);
            count--;
        }

        currentItemName = "";
        count = 0;
    }

    public void CompeleteCrafting()
    {
        TurnNoneCraftingLight();

        if (count > 0)
        {
            //Debug.Log("Complete");
        }
        else return;

        currentItemName = "";
        count = 0;
    }

    void TurnCraftingLight()
    {
        greenLight.SetActive(true);
        yellowLight.SetActive(false);
        redLight.SetActive(false);
    }

    void TurnNoneCraftingLight()
    {
        greenLight.SetActive(false);
        yellowLight.SetActive(true);
        redLight.SetActive(false);
    }

    void TurnFullCraftingItemSlot()
    {
        redLight.SetActive(true);
        greenLight.SetActive(false);
        yellowLight.SetActive(false);
    }

    private void OnTriggerExit(Collider other)
    {
        //Debug.Log(other.name);
        other.GetComponent<Plant>().rigid.useGravity = true;
        other.GetComponent<Plant>().ChangeToSeed();
        currentSuckableItems.Remove(currentSuckableItems[count - 1]);
        count--;


        TurnNoneCraftingLight();
        if (count > 0)
        {
            currentSuckableItems[count - 1].transform.position = itemPos.position;
            currentSuckableItems[count - 1].transform.eulerAngles = Vector3.zero;
            StartCoroutine(SetActiveLastItem());
        }
    }

    IEnumerator SetActiveLastItem()
    {
        yield return new WaitForSeconds(1f);
        currentSuckableItems[count - 1].gameObject.SetActive(true);
    }
}
