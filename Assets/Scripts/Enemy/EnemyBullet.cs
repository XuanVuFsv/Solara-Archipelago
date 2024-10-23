using System.Collections;
using UnityEngine;
using VitsehLand.Scripts.Player;
using VitsehLand.Scripts.Stats;

namespace VitsehLand.Scripts.Enemy
{
    public class EnemyBullet : MonoBehaviour
    {
        public float timeLife;

        [SerializeField]
        Rigidbody rigid;

        [SerializeField]
        EnemyStat enemyStat;

        public void TriggerBullet(int force, Vector3 direction)
        {
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
                gameObject.SetActive(false);
                Destroy(gameObject, Random.Range(0, 5));
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                other.gameObject.GetComponent<HealthController>().TakeDamage(enemyStat.damage);
            }
            Explode();
        }

        void Explode()
        {
            Destroy(gameObject, Random.Range(0, 5));
        }
    }
}