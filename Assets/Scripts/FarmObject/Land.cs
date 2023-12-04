using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;

public class Land : MonoBehaviour
{
    public GameObject buildingVFX;
    public GameObject completeVFX;
    public GameObject garden;

    public GameObject buildingWall;
    public float movingWallYPos;
    
    public MonitorGardenController monitorScreen;
    
    public AudioClip completeBuildingSound;
    public AudioClip startBuildSound;

    public float limitAngleToInteractBuildButton;
    public float timeForBuildingWallMove;
    public int timeToBuildGarden;

    public bool hasBuilt;

    private void OnTriggerStay(Collider other)
    {
        bool isLookAtMonitorScreen = IsLookAtMonitorScreen(Camera.main.transform.forward, monitorScreen.screenViewPoint.transform.position - Camera.main.transform.position, limitAngleToInteractBuildButton);
        if (Input.GetKey(KeyCode.E) && isLookAtMonitorScreen)
        {
            //buildingWall.transform.DOMoveY(activeYPos + buildingWall.transform.position.y, timeForBuildingWallMove);

            Debug.Log("Interactable");

            if (hasBuilt) return;

            Debug.Log("Build");
            AudioBuildingManager.Instance.PlayAudioClip(startBuildSound);

            buildingVFX.SetActive(true);

            hasBuilt = true;

            StartBuildingWallAnimation(true);
            monitorScreen.UpdateBuildingStatus(false);

            monitorScreen.SetActiveBuildingCountdown(true);
            monitorScreen.StartCountdownAnimation(timeToBuildGarden);

            StartCoroutine(StartBuildGarden());
        }
    }

    TweenerCore<Vector3, Vector3, VectorOptions> StartBuildingWallAnimation(bool isActive)
    {
        if (isActive) buildingWall.SetActive(isActive);
        
        return buildingWall.transform.DOMoveY(isActive ? buildingWall.transform.position.y + movingWallYPos : buildingWall.transform.position.y - movingWallYPos, timeForBuildingWallMove);
    }

    IEnumerator StartBuildGarden()
    {
        yield return new WaitForSeconds(timeToBuildGarden);

        Debug.Log("End Build");

        garden.SetActive(true);
        StartBuildingWallAnimation(false).OnComplete(() => {
            buildingWall.SetActive(false);
            monitorScreen.SwitchMonitorState(true);
        });

        completeVFX.GetComponent<ParticleSystem>().Play();
        buildingVFX.SetActive(false);

        AudioBuildingManager.Instance.PlayAudioClip(completeBuildingSound);
    }

    bool IsLookAtMonitorScreen(Vector3 lookAtVector, Vector3 monitorScreenVector, float limitAngle)
    {
        float dotValue = Vector3.Dot(lookAtVector, monitorScreenVector);
        float angle = Mathf.Acos(dotValue / (lookAtVector.magnitude * monitorScreenVector.magnitude)) * Mathf.Rad2Deg;
        //Debug.Log(angle);

        if (dotValue > 0)
        {
            if (angle <= limitAngle) return true;
            return false;
        }
        return false;
    }
}
