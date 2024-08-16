using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Animator animator;
    public EnemyStats enemyStats;
    public HealthController healthController;

    public bool canAttack = true;

    // Start is called before the first frame update
    void Awake()
    {
        healthController.health = enemyStats.health;
    }

    // Update is called once per frame
    void Update()
    {
        if (!TimeManager.Instance.isNight)
        {
            Destroy(gameObject, Random.Range(0, 5));
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //Debug.Log("Collide");
        if (other.tag == "Player" && canAttack)
        {
            StartCoroutine(Attack());
            other.GetComponent<HealthController>().TakeDamage(enemyStats.damage);
        }
    }

    public void TakeDamage(int damage)
    {
        healthController.TakeDamage(damage);

        animator.Play("TakeDamage");

        if (healthController.health <= 0)
        {
            healthController.health = 0;
            GemManager.Instance.AddGem(enemyStats.gemRewardForPlayerWhenKilled);
            StartCoroutine(Die());
        }
    }

    private IEnumerator Attack()
    {
        canAttack = false;
        animator.Play("Attack");

        yield return new WaitForSeconds(1 / enemyStats.speedAttack);
        canAttack = true;
    }

    private IEnumerator Die()
    {
        animator.Play("Die");
        yield return new WaitForSeconds(2);
        Destroy(transform.parent.gameObject);
    }
}
