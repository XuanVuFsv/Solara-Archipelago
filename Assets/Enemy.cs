using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VitsehLand.Scripts.Farming;

public class Enemy : MonoBehaviour
{
    RaycastHit hit;
    public List<Transform> dirs = new List<Transform>();

    public float speed = 0.001f;
    float s = 0;
    int index;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(true);
        index = Random.Range(0, 5);
    }

    // Update is called once per frame
    void Update()
    {
        s += Time.deltaTime;
        transform.position = Vector3.Lerp(transform.position, dirs[index].position, s * speed);
        transform.LookAt(dirs[index]);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Axie")
        {
            AxieBehaviour axie = collision.transform.gameObject.GetComponent<AxieBehaviour>();
            if (axie)
            {
                axie.food = 0;
                axie.state.text = "Hungryyyyy";
                gameObject.SetActive(false);
                Destroy(gameObject, Random.Range(2, 20));
            }
        }
    }
}
