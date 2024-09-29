using System.Collections;
using UnityEngine;
using VitsehLand.Scripts.Farming.General;
using VitsehLand.Scripts.Farming.Resource;

namespace VitsehLand.Scripts.Crafting
{
    public class CraftingReceiver : MonoBehaviour
    {
        public CraftingManager craftingManager;

        private void OnTriggerEnter(Collider other)
        {
            Suckable suckable = other.GetComponent<Suckable>();
            if (other && (suckable is PowerContainer || suckable is NaturalResource))
            {
                bool addSuccess = craftingManager.AddItemStorage(suckable.cropStats, suckable.cropContain);
                if (addSuccess)
                {
                    suckable.gameObject.SetActive(false);
                    StartCoroutine(AutoDestroy(suckable.gameObject));
                }
            }
        }

        IEnumerator AutoDestroy(GameObject suckable)
        {
            yield return new WaitForSeconds(Random.value * 5);
            if (suckable) Destroy(suckable);
        }
    }
}