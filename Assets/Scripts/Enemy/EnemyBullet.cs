using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float timeLife;

    [SerializeField]
    Rigidbody rigid;

    [SerializeField]
    EnemyStats enemyStats;

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
            other.gameObject.GetComponent<HealthController>().TakeDamage(enemyStats.damage);
        }
        Explode();
    }

    void Explode()
    {
        Destroy(gameObject, Random.Range(0, 5));
    }
}
