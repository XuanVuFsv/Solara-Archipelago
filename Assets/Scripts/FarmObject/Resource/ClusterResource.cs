using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;
using Random = UnityEngine.Random;

public class ClusterResource : MonoBehaviour
{
    public GameObject resourcePrefab;
    public GameObject fx;
    public Animator animator;
    public List<GameObject> resources = new List<GameObject>();

    public int maxResourceContain, resourceContain;
    public float collectTime = 0;
    public float recoverTime = 3;
    public bool inCollecting = false;


    [SerializeField]
    private float health = 100;
    [SerializeField]
    private float maxHealth = 100;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        collectTime = resourcePrefab.GetComponent<NaturalResource>().ammoStats.totalProducingTime;
        resourceContain = Random.Range(1, maxResourceContain);
        InstantiateResources(resourceContain);
    }

    // Update is called once per frame
    void Update()
    {
        if (InputController.Instance.isStopFire)
        {
            if (health < maxHealth)
            {
                health += maxHealth / recoverTime * Time.deltaTime;

                if (health >= maxHealth)
                {
                    inCollecting = false;
                    animator.Play("None");
                    transform.localScale = Vector3.one;
                    health = maxHealth;
                }
            }
        }
    }

    public void InstantiateResources(int _resourceContain)
    {
        for (int i = 0; i < _resourceContain; i++)
        {
            GameObject resource = Instantiate(resourcePrefab, transform.position + 0.2f * Random.value * Vector3.one, Quaternion.identity);
            resource.SetActive(false);
            resources.Add(resource);
        }
    }

    public void StartCollect()
    {
        if (health <= 0 && inCollecting)
        {
            inCollecting = false;
            BreakCluster();
        }
        else
        {
            animator.Play("Breaking");
            inCollecting = true;

            health -= maxHealth / collectTime * Time.deltaTime;
        }
    }

    void BreakCluster()
    {
        AudioBuildingManager.Instance.PlayAudioClip(AudioBuildingManager.Instance.breakCrystal);
        for (int i = 0; i < resources.Count; i++)
        {
            resources[i].SetActive(true);
        }
        resources.Clear();
        fx.transform.parent = null;
        fx.SetActive(true);
        gameObject.SetActive(false);
        Destroy(gameObject, Random.value * 10);
    }
}
