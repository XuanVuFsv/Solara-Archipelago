using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Enemy prefab;
    public bool stop = false;

    // Update is called once per frame
    void Update()
    {
        if (stop == false)
        {
            StartCoroutine("CreateEnemy");
        }
    }

    IEnumerator CreateEnemy()
    {
        stop = true;
        GameObject enemy = Instantiate(prefab.gameObject, transform.position, Quaternion.identity);
        enemy.SetActive(true); ;
        yield return new WaitForSeconds(Random.Range(10, 120));
        stop = false;
    }
}
