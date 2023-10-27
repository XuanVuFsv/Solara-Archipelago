using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class PlanProperties
{
    [Header("Effect")]
    public GameObject trailTracer;
    public GameObject bulletObject;
    public ParticleSystem hitEffectPrefab;

    [Header("Plant stage")]
    public List<GameObject> growingBodyList;

    public List<Plant> plantList;
    public int wateringTime;
}

public class Plant : Suckable
{
    public PlanProperties plantData;
    public PlantState plantState;

    public int elapsedTime = 0;
    public DateTime startGrowingTime;
    public DateTime endGrowingTime;
    public GameObject seedOuterEffect, ripeOuterEffect;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void ChangeToStored()
    {
        gameObject.transform.position = CollectHandler.Instance.shootingInputData.bulletSpawnPoint.position;
        rigid.velocity = Vector3.zero;
        plantState = PlantState.Stored;
        gameObject.SetActive(false);
    }

    public override void ChangeToSeed()
    {
        if (rigid) rigid = GetComponent<Rigidbody>();
        gameObject.transform.position = CollectHandler.Instance.shootingInputData.bulletSpawnPoint.position;
        plantState = PlantState.Seed;
        gameObject.SetActive(true);
        seedOuterEffect.GetComponent<ParticleSystem>().Play(true);
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
        plantState = PlantState.Ripe;
        seedOuterEffect.SetActive(false);
        //ripeOuterEffect.SetActive(true);
        Debug.Log("Growing Process Done");
    }

    private void OnDisable()
    {
        elapsedTime = (int)DateTime.UtcNow.Subtract(startGrowingTime.ToLocalTime()).TotalSeconds;
        Debug.Log(elapsedTime);
    }
}
