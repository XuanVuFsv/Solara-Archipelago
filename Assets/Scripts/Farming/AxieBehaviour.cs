using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using VitsehLand.Scripts.Farming.General;

namespace VitsehLand.Scripts.Farming
{
    public class AxieBehaviour : MonoBehaviour
    {
        public Suckable sugar;
        public int minSugarTime, maxSugarTime;
        public bool makeSugar;
        public Suckable lottery;
        public int minLotTime, maxLotTime;
        public bool makeLot;
        public Suckable santol, dragon;
        public int minAmmoTime, maxAmmoTime;
        public bool makeAmmo;

        public List<Transform> movingList = new List<Transform>();
        public int index = 0;
        public float speed = 1;
        float s = 0;

        public int food = 0;
        public int foodStack;
        public int timeToHungry;

        public TextMeshProUGUI state;

        // Start is called before the first frame update
        void Start()
        {
            if (food == foodStack)
            {
                state.text = "Yummyyyyy";
                StartCoroutine("Hungry");
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (food == foodStack)
            {
                if (makeSugar == false)
                {
                    //Debug.Log("Call MakeSugar");
                    StartCoroutine("MakeSugar");
                }

                if (makeLot == false)
                {
                    //Debug.Log("Call MakeLot");
                    StartCoroutine("MakeLot");
                }

                if (makeAmmo == false)
                {
                    //Debug.Log("Call MakeAmmo");
                    StartCoroutine("MakeAmmo");
                }
            }

            if (Vector3.Distance(transform.position, movingList[index].position) <= 2f)
            {
                index = Random.Range(0, movingList.Count - 1);
                s = 0;
            }
            s += Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, movingList[index].position, s * speed);
            transform.LookAt(movingList[index]);
        }

        IEnumerator MakeSugar()
        {
            makeSugar = true;
            yield return new WaitForSeconds(Random.Range(minSugarTime, maxSugarTime));
            Instantiate(sugar.gameObject, transform.position + Vector3.up * 2, Quaternion.identity);
            makeSugar = false;
        }

        IEnumerator MakeLot()
        {
            makeLot = true;
            yield return new WaitForSeconds(Random.Range(minLotTime, maxLotTime));
            Instantiate(lottery.gameObject, transform.position + Vector3.up * 2, Quaternion.identity);
            makeLot = false;
        }

        IEnumerator MakeAmmo()
        {
            makeAmmo = true;
            float t = Random.Range(minAmmoTime, maxAmmoTime);
            yield return new WaitForSeconds(t);
            if ((int)t % 2 == 0)
            {
                Instantiate(santol.gameObject, transform.position + Vector3.up * 2, Quaternion.identity);
            }
            else
            {
                Instantiate(dragon.gameObject, transform.position + Vector3.up * 2, Quaternion.identity);
            }
            makeAmmo = false;
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }

        private void OnCollisionEnter(Collision collision)
        {
            Crop item = collision.gameObject.GetComponent<Crop>();
            if (item == null) return;
            if (item.cropStats.name == "StrawberryJam" || item.cropStats.name == "AppleJam" && food < foodStack)
            {
                food++;
                if (food == foodStack)
                {
                    state.text = "Yummyyyyy";
                    StartCoroutine("Hungry");
                }
                item.gameObject.SetActive(false);
                Destroy(item.gameObject, Random.Range(1, 10));
            }

            if (collision.gameObject.tag == "Axie")
            {
                GetComponent<Rigidbody>().AddForce(0, Random.Range(0, 10), 0);
            }
        }

        IEnumerator Hungry()
        {
            yield return new WaitForSeconds(timeToHungry);
            food = 0;
            state.text = "Hungryyyyy";
        }
    }
}