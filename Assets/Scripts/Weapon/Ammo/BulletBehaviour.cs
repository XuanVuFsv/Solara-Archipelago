using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using VitsehLand.Scripts.Pattern.Observer;
using VitsehLand.Scripts.Pattern.Pooling;

namespace VitsehLand.Scripts.Weapon.Ammo
{
    public class BulletBehaviour : MonoBehaviour
    {
        public float timeLife;
        string bulletName;

        [SerializeField]
        Rigidbody rigid;
        [SerializeField]
        GameEvent hitEvent;

        public void TriggerBullet(string name, int force, Vector3 direction)
        {
            bulletName = name;
            AddForeToBullet(force, direction);
        }

        void AddForeToBullet(int force, Vector3 direction)
        {
            rigid.AddForce(direction.normalized * force);
            StartCoroutine(Timer());
        }

        IEnumerator Timer()
        {
            yield return new WaitForSeconds(timeLife);
            if (transform)
            {
                Destroy(gameObject);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            PoolingManager.Instance.Get("Pool_" + bulletName + "_Setup");
            //Debug.Log("Notify " + transform.position + " " + other.transform.position);
            hitEvent.Notify(transform.position, collision.transform.position - transform.position);
            Explode();
        }

        void Explode()
        {
            Destroy(gameObject);
        }
    }
}