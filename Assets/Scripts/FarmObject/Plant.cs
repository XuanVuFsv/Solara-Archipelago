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
    public GameObject growingBody;
    public GameObject orginalBody;
    public Plant orginalPlant;

    public GameObject ripePlantPrefab;

    public int wateringStack;
}

public class Plant : Suckable
{
    public PlanProperties plantData;
    public PlantState plantState;

    public int growingTime = 3;
    public int elapsedTime = 0;
    public bool onTree = false;
    public bool nonPlanting;
    public DateTime startGrowingTime;
    public DateTime endGrowingTime;
    public List<Transform> ripePlantPoss = new List<Transform>();
    public GameObject seedOuterEffect, ripeOuterEffect;
    public List<Plant> ripePlants = new List<Plant>();
    public int ripePlantCount = 0;
    public GameObject defaultModelPlant;
    public bool inCrafting;

    public float startTimeDestroy;
    public float destroyedTime;
    public bool startDestroyedTimer;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();

        //if (ammoStats.name == "AXS")
        //{
        //    int x = UnityEngine.Random.Range(AXSManager.Instance.starRandomRangeValue, AXSManager.Instance.endRandomRangeValue);
        //    if (x <= AXSManager.Instance.smallestReward)
        //    {
        //        ammoContain = 1;
        //    }
        //    else if(x <= AXSManager.Instance.mediumReward)
        //    {
        //        ammoContain = 10;
        //    }
        //    else
        //    {
        //        ammoContain = 100;
        //    }
        //}
    }

    public override void ChangeToStored()
    {
        startDestroyedTimer = false;
        StopCoroutine("DestroyTimer");
        inCrafting = false;

        gameObject.transform.position = CollectHandler.Instance.shootingInputData.bulletSpawnPoint.position;

        rigid.velocity = Vector3.zero;
        collider.isTrigger = false;
        rigid.useGravity = true;

        plantState = PlantState.Stored;

        gameObject.SetActive(false);
        seedOuterEffect.SetActive(true);
        ripeOuterEffect.SetActive(false);
    }

    public override void ChangeToSeed()
    {
        if (rigid) rigid = GetComponent<Rigidbody>();
        collider.isTrigger = false;
        rigid.isKinematic = false;
        rigid.useGravity = true;

        gameObject.transform.position = CollectHandler.Instance.shootingInputData.bulletSpawnPoint.position;

        plantState = PlantState.Seed;

        gameObject.SetActive(true);
        defaultModelPlant.SetActive(true);
        seedOuterEffect.GetComponent<ParticleSystem>().Play(true);

        StartCoroutine("DestroyTimer");
    }

    public void ChangeToSeedOnTree()
    {
        startDestroyedTimer = false;
        StopCoroutine("DestroyTimer");
        inCrafting = false;
        //Debug.Log("OnTree");
        if (rigid) rigid = GetComponent<Rigidbody>();
        collider.isTrigger = true;
        rigid.isKinematic = false;
        rigid.useGravity = false;

        plantState = PlantState.Seed;

        plantData.growingBody.SetActive(false);
        gameObject.SetActive(true);
        defaultModelPlant.SetActive(true);
    }

    public void ChangeToGrowingBody()
    {
        startDestroyedTimer = false;
        StopCoroutine("DestroyTimer");
        inCrafting = false;
        if (rigid) rigid = GetComponent<Rigidbody>();
        rigid.isKinematic = true;
        rigid.useGravity = false;
        ResetVelocity();
        collider.isTrigger = true;

        plantData.orginalBody = null;
        plantState = PlantState.GrowingBody;
        defaultModelPlant.SetActive(false);
        GrowPlant();
    }

    public void ChangeToRipe()
    {
        startDestroyedTimer = false;
        StopCoroutine("DestroyTimer");
        inCrafting = false;
        foreach (Transform ripePos in ripePlantPoss)
        {
            Plant newRipePlant = GameObject.Instantiate(plantData.ripePlantPrefab, ripePos.position, Quaternion.identity, ripePos).GetComponent<Plant>();
            //Debug.Log(newRipePlant.name);
            ripePlants.Add(newRipePlant);
            newRipePlant.plantData.orginalPlant = this;
            ripePlantCount++;

            //newRipePlant.transform.localScale = new Vector3(1, 1, 1);

            newRipePlant.onTree = true;
            newRipePlant.plantData.orginalBody = plantData.growingBody;

            newRipePlant.ChangeToSeedOnTree();
            newRipePlant.SetRipePlantEffect();

            plantState = PlantState.Ripe;
        }
    }

    public void SetRipePlantEffect()
    {
        //seedOuterEffect.SetActive(false);
        ripeOuterEffect.SetActive(true);
    }

    public void GrowPlant()
    {
        gameObject.SetActive(true);
        plantData.growingBody.SetActive(true);

        startGrowingTime = DateTime.UtcNow.ToLocalTime();
        TimeSpan span = TimeSpan.FromSeconds(ammoStats.totalGrowingTime);
        endGrowingTime = startGrowingTime.Add(span);
        StartCoroutine(StartGrowingProcess());
        //Debug.Log(startGrowingTime);
    }

    IEnumerator StartGrowingProcess()
    {
        yield return new WaitForSeconds(ammoStats.totalGrowingTime);
        plantState = PlantState.Ripe;
        seedOuterEffect.SetActive(false);
        //ripeOuterEffect.SetActive(true);
        ChangeToRipe();
        //Debug.Log("Growing Process Done");
        growingTime--;
    }

    //IEnumerator DelayGrowPlant()
    //{
    //    yield return new WaitForSeconds(10f);
    //    if (growingTime == 0) Destroy(gameObject);
    //    ChangeToGrowingBody();        
    //}

    IEnumerator DestroyTimer()
    {
        startDestroyedTimer = true;
        int t = UnityEngine.Random.Range(20, 40);
        yield return new WaitForSeconds(t);
        if (plantState == PlantState.Seed && startDestroyedTimer && !inCrafting) DestroyThis();
    }    

    private void OnDisable()
    {
        elapsedTime = (int)DateTime.UtcNow.Subtract(startGrowingTime.ToLocalTime()).TotalSeconds;
        //Debug.Log(elapsedTime);
    }

    public void ResetPlantStats()
    {
        ripePlants = new List<Plant>(0);
        startDestroyedTimer = false;
        StopCoroutine("DestroyTimer");
        //StartCoroutine(DelayGrowPlant());
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "CheckingHarvest")
        {
            //Debug.Log("Harvest");
            plantData.orginalBody = null;
            transform.parent = null;

            collider.isTrigger = false;
            rigid.isKinematic = false;
            rigid.useGravity = true;

            plantData.orginalPlant.ripePlantCount--;
            if (plantData.orginalPlant.ripePlantCount == 0)
            {
                plantData.orginalPlant.ResetPlantStats();
            }
            onTree = false;
        }
    }

    public bool CanSuckUp()
    {
        return plantState == PlantState.Seed;
    }

    public void DestroyThis()
    {
        startDestroyedTimer = false;
        StopCoroutine("DestroyTimer");
        if (gameObject) Destroy(gameObject);
    }
}
