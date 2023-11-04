using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Land : MonoBehaviour
{
    public GameObject buildingVFX, completeVFX;
    public GameObject garden;

    public AudioClip completeBuildingSound, startBuildSound;

    public float minDistanceToInteractBuildButton;
    public float timeToBuildGarden;
    public bool hasBuilt;

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerStay(Collider other)
    {
        if (hasBuilt) return;

        if (other.tag == "Player")
        {
            if (Input.GetKeyDown(KeyCode.E) && Vector3.Distance(other.transform.position, transform.position) <= minDistanceToInteractBuildButton)
            {
                Debug.Log("Build");
                AudioBuildingManager.Instance.PlayAudioClip(startBuildSound);
                hasBuilt = true;
                buildingVFX.SetActive(true);
                StartCoroutine(StartBuildGarden());
            }    
        }
    }

    public IEnumerator StartBuildGarden()
    {
        yield return new WaitForSeconds(timeToBuildGarden);
        Debug.Log("End Build");
        Invoke("PlayCompeleteBuilding", 0.25f);
        AudioBuildingManager.Instance.PlayAudioClip(completeBuildingSound);
        buildingVFX.SetActive(false);
        garden.SetActive(true);
    }

    public void PlayCompeleteBuilding()
    {
        completeVFX.GetComponent<ParticleSystem>().Play();
    }
}
