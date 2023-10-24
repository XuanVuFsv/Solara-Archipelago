using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class SeedProperties
{
    [Header("Effect")]
    public GameObject trailTracer;
    public GameObject bulletObject;
    public ParticleSystem hitEffectPrefab;

    [Header("Plant stage")]
    public List<GameObject> growingBodyList;
    public List<Plant> growingPlantList;

    public List<Plant> plantList;
    public int wateringTime;
}

public class Seed : MonoBehaviour, ISuckable
{
    public AmmoStats ammoStats;
    public SeedProperties seedData;

    public int elapsedTime = 0;
    public DateTime startGrowingTime;
    public DateTime endGrowingTime;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("GrowPlant", 5);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GrowPlant()
    {
        startGrowingTime = DateTime.UtcNow.ToLocalTime();
        TimeSpan span = TimeSpan.FromSeconds(ammoStats.totalGrowingTime);
        endGrowingTime = startGrowingTime.Add(span);
        StartCoroutine(StartGrowingProcess());
        Debug.Log(startGrowingTime);
        Debug.Log(endGrowingTime);
    }

    IEnumerator StartGrowingProcess()
    {
        yield return new WaitForSeconds(ammoStats.totalGrowingTime);
        Debug.Log("Growing Process Done");
    }

    private void OnDisable()
    {
        elapsedTime = (int)startGrowingTime.Subtract(DateTime.UtcNow.ToLocalTime()).TotalSeconds;
        Debug.Log(elapsedTime);
    }
}
