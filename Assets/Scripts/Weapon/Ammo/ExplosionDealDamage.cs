using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VitsehLand.Scripts.Enemy;
using VitsehLand.Scripts.Stats;

namespace VitsehLand.Scripts.Weapon.Ammo
{
    public class ExplosionDealDamage : MonoBehaviour
    {
        public CropStats cropStats;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Enemy")
            {
                //Debug.Log("Explode");
                other.gameObject.GetComponent<EnemyController>().TakeDamage(cropStats.damage);
            }
        }
    }
}