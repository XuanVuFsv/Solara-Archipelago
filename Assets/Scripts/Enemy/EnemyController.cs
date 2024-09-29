using System.Collections;
using UnityEngine;
using VitsehLand.Scripts.Audio;
using VitsehLand.Scripts.Manager;
using VitsehLand.Scripts.Player;
using VitsehLand.Scripts.Stats;
using VitsehLand.Scripts.TimeManager;

namespace VitsehLand.Scripts.Enemy
{
    public class EnemyController : MonoBehaviour
    {
        public GameObject parent;
        public GameObject shield;
        public ParticleSystem breakShieldFX;
        public Animator animator;
        public EnemyStats enemyStats;
        public HealthController healthController;

        public EnemyBullet bullet;
        public Transform spawnBulletTransform;

        public bool canAttack = true;

        public enum Defender
        {
            None = 1,
            Shield = 2
        }
        public Defender defender;

        public enum Attacker
        {
            Normal = 0,
            Bullet = 1
        }
        public Attacker attacker;

        // Start is called before the first frame update
        void Awake()
        {
            healthController.health = enemyStats.health;
        }

        // Update is called once per frame
        void Update()
        {
            if (!GameTimeManager.Instance.isNight)
            {
                Destroy(parent, Random.Range(0, 100));
            }
        }

        private void OnTriggerStay(Collider other)
        {
            //Debug.Log("Collide");
            if (other.CompareTag("BreakShield"))
            {
                //Debug.Log("BreakShieldStay");
            }

            if (other.CompareTag("Player") && canAttack)
            {
                StartCoroutine(Attack(other.gameObject));
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("BreakShield") && defender == Defender.Shield)
            {
                //Debug.Log("BreakShield");
                breakShieldFX.Emit(1);
                defender = Defender.None;
                shield.SetActive(false);
                AudioBuildingManager.Instance.audioSource.volume = 1;
                AudioBuildingManager.Instance.PlayAudioClip(AudioBuildingManager.Instance.breakShield);
            }
        }

        public void TakeDamage(int damage)
        {
            if (defender == Defender.None) healthController.TakeDamage(damage);
            else healthController.TakeDamage(damage * enemyStats.shieldDecreaseDamage);

            animator?.Play("TakeDamage");

            if (healthController.health <= 0)
            {
                healthController.health = 0;
                GemManager.Instance.AddGem(enemyStats.gemRewardForPlayerWhenKilled);
                StartCoroutine(Die());
            }
        }

        private IEnumerator Attack(GameObject target)
        {
            canAttack = false;
            animator?.Play("Attack");
            if (attacker == Attacker.Normal) target.GetComponent<HealthController>().TakeDamage(enemyStats.damage);
            else
            {
                Vector3 direction = (target.transform.position - spawnBulletTransform.position).normalized;

                EnemyBullet newBullet = Instantiate(bullet.gameObject, spawnBulletTransform.position, Quaternion.Euler(target.transform.position - spawnBulletTransform.position)).GetComponent<EnemyBullet>();
                newBullet.gameObject.SetActive(true);

                newBullet.TriggerBullet(enemyStats.bulletForce, direction);
            }
            AudioBuildingManager.Instance.audioSource.volume = 0.25f;
            AudioBuildingManager.Instance.PlayAudioClip(AudioBuildingManager.Instance.enemyAttack);

            yield return new WaitForSeconds(1 / enemyStats.speedAttack);
            canAttack = true;
        }

        private IEnumerator Die()
        {
            animator?.Play("Die");
            AudioBuildingManager.Instance.audioSource.volume = 0.5f;
            AudioBuildingManager.Instance.PlayAudioClip(AudioBuildingManager.Instance.enemyDie);
            yield return new WaitForSeconds(0.25f);
            Destroy(parent);
        }
    }
}