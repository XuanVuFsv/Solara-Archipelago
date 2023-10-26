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

public class Plant : Suckable
{
    public SeedProperties seedData;

    public int elapsedTime = 0;
    public DateTime startGrowingTime;
    public DateTime endGrowingTime;
    public bool isSeed = true;
    public GameObject seedOuterEffect, ripeOuterEffect;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        Invoke("GrowPlant", 5);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GrowPlant()
    {
        startGrowingTime = DateTime.UtcNow.ToLocalTime();
        TimeSpan span = TimeSpan.FromSeconds(GetAmmoStats().totalGrowingTime);
        endGrowingTime = startGrowingTime.Add(span);
        StartCoroutine(StartGrowingProcess());
        Debug.Log(startGrowingTime);
        Debug.Log(endGrowingTime);
    }

    IEnumerator StartGrowingProcess()
    {
        yield return new WaitForSeconds(GetAmmoStats().totalGrowingTime);
        isSeed = false;
        seedOuterEffect.SetActive(false);
        ripeOuterEffect.SetActive(true);
        Debug.Log("Growing Process Done");
    }

    private void OnDisable()
    {
        elapsedTime = (int)DateTime.UtcNow.Subtract(startGrowingTime.ToLocalTime()).TotalSeconds;
        Debug.Log(elapsedTime);
    }
}
