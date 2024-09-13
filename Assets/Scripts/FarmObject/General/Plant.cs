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

    public GameObject wholePlantPrefab;

    public int wateringStack;
}

public class Plant : Suckable
{
    public enum PlantState
    {
        Seed = 0,
        Stored = 1,
        GrowingBody = 2,
        Whole = 3
    }
    //public PlantState plantState;

    public PlanProperties plantData;

    public GardenManager ownerGarden;
    public GardenSlotProperties ownerSlot;

    [Header("Plant Components")]
    public GameObject growingBody;
    //public GameObject orginalBody;
    public Plant orginalPlant;

    //public int growingTime = 3;
    public int elapsedTime = 0;

    public bool onTree = false;
    public bool nonPlanting;

    public DateTime startGrowingTime;
    public DateTime endGrowingTime;

    public List<Transform> wholePlantPoss = new List<Transform>();
    public List<Plant> wholePlants = new List<Plant>();
    public GameObject seedOuterEffect, wholeOuterEffect;

    public GameObject defaultModelPlant;
    public int wholePlantCount = 0;
    public bool inCrafting;

    public float startTimeDestroy;
    public float destroyedTime;
    public bool startDestroyedTimer;

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("Start a Plant");
        rigid = GetComponent<Rigidbody>();
        suckableCollider = GetComponent<Collider>();

        SetState(new CropSeed(this));

        //growingTime = ammoStats.totalGrowingTime;
    }

    public bool HaveWholeCrop()
    {
        return wholePlants.Count > 0;
    }

    public bool CanSuckUp()
    {
        //return plantState == PlantState.Seed || plantState == PlantState.Whole;
        return state is CropSeed || state is CropWhole;
    }

    public override void ChangeToStored()
    {
        EndCurrentState();
        SetState(new CropStored(this));
        //startDestroyedTimer = false;
        //StopCoroutine("DestroyTimer");
        //inCrafting = false;

        //gameObject.transform.position = CollectHandler.Instance.shootingInputData.bulletSpawnPoint.position;

        //rigid.velocity = Vector3.zero;
        //suckableCollider.isTrigger = false;
        //rigid.useGravity = true;

        //plantState = PlantState.Stored;

        //gameObject.SetActive(false);
        //seedOuterEffect.SetActive(true);
        //wholeOuterEffect.SetActive(false);
    }

    public override void ChangeToUnStored()
    {
        EndCurrentState();
        SetState(new CropSeed(this));
        //if (rigid) rigid = GetComponent<Rigidbody>();
        //suckableCollider.isTrigger = false;
        //rigid.isKinematic = false;
        //rigid.useGravity = true;

        //gameObject.transform.position = CollectHandler.Instance.shootingInputData.bulletSpawnPoint.position;

        //plantState = PlantState.Seed;

        //gameObject.SetActive(true);
        //defaultModelPlant.SetActive(true);
        //seedOuterEffect.GetComponent<ParticleSystem>().Play(true);

        //StartCoroutine("DestroyTimer");
    }

    //public void ChangeToSeedOnTree()
    //{
    //    startDestroyedTimer = false;
    //    StopCoroutine("DestroyTimer");
    //    inCrafting = false;
    //    if (rigid) rigid = GetComponent<Rigidbody>();
    //    suckableCollider.isTrigger = true;
    //    rigid.isKinematic = false;
    //    rigid.useGravity = false;

    //    plantState = PlantState.Seed;

    //    growingBody.SetActive(false);
    //    gameObject.SetActive(true);
    //    defaultModelPlant.SetActive(true);
    //}

    public void ChangeToGrowingBody()
    {
        EndCurrentState();
        SetState(new CropGrowing(this));
        //startDestroyedTimer = false;
        //StopCoroutine("DestroyTimer");
        //inCrafting = false;
        //if (rigid) rigid = GetComponent<Rigidbody>();
        //rigid.isKinematic = true;
        //rigid.useGravity = false;
        //ResetVelocity();
        //suckableCollider.isTrigger = true;

        ////plantData.orginalBody = null;
        //plantState = PlantState.GrowingBody;
        //defaultModelPlant.SetActive(false);
        //GrowPlant();
    }

    //public void CompleteGrowingSession()
    //{
    //    //startDestroyedTimer = false;
    //    //StopCoroutine("DestroyTimer");
    //    //inCrafting = false;
    //    foreach (Transform wholePos in wholePlantPoss)
    //    {
    //        Plant newwholePlant = GameObject.Instantiate(plantData.wholePlantPrefab, wholePos.position, Quaternion.identity, wholePos).GetComponent<Plant>();
    //        wholePlants.Add(newwholePlant);
    //        newwholePlant.orginalPlant = this;
    //        wholePlantCount++;

    //        newwholePlant.onTree = true;
    //        //newwholePlant.plantData.orginalBody = plantData.growingBody;

    //        newwholePlant.ChangeToSeedOnTree();
    //        newwholePlant.SetWholePlantEffect();

    //        plantState = PlantState.Whole;
    //    }
    //}

    //public void SetWholePlantEffect()
    //{
    //    //seedOuterEffect.SetActive(false);
    //    wholeOuterEffect.SetActive(true);
    //}

    //public void GrowPlant()
    //{
    //    gameObject.SetActive(true);
    //    growingBody.SetActive(true);

    //    startGrowingTime = DateTime.UtcNow.ToLocalTime();
    //    TimeSpan span = TimeSpan.FromSeconds(ammoStats.totalGrowingTime);
    //    endGrowingTime = startGrowingTime.Add(span);
    //    StartCoroutine(StartGrowingProcess());

    //    //Debug.Log(startGrowingTime);
    //}

    //IEnumerator StartGrowingProcess()
    //{
    //    yield return new WaitForSeconds(ammoStats.totalGrowingTime);
    //    plantState = PlantState.Whole;
    //    seedOuterEffect.SetActive(false);
    //    //wholeOuterEffect.SetActive(true);
    //    CompleteGrowingSession();
    //    //Debug.Log("Growing Process Done");
    //    growingTime--;
    //}

    //IEnumerator DestroyTimer()
    //{
    //    //startDestroyedTimer = true;
    //    int t = UnityEngine.Random.Range(20, 40);
    //    yield return new WaitForSeconds(t);
    //    //if (plantState == PlantState.Seed && startDestroyedTimer && !inCrafting) DestroyThis();
    //}    

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "CheckingHarvest" && state is CropWhole)
        {
            state.End();
            //Debug.Log("Harvest");
            //plantData.orginalBody = null;
            //transform.parent = null;

            //suckableCollider.isTrigger = false;
            //rigid.isKinematic = false;
            //rigid.useGravity = true;

            //orginalPlant.wholePlantCount--;
            //if (orginalPlant.wholePlantCount == 0)
            //{
            //    orginalPlant.ResetPlantStats();
            //}
            //onTree = false;
        }
    }

    private void OnDisable()
    {
        elapsedTime = (int)DateTime.UtcNow.Subtract(startGrowingTime.ToLocalTime()).TotalSeconds;
        //Debug.Log(elapsedTime);
    }
    
    //public void ResetPlantStats()
    //{
    //    wholePlants = new List<Plant>(0);
    //    startDestroyedTimer = false;
    //    StopCoroutine("DestroyTimer");
    //}

    //public void DestroyThis()
    //{
    //    startDestroyedTimer = false;
    //    StopCoroutine("DestroyTimer");
    //    if (gameObject) Destroy(gameObject);
    //}
}
