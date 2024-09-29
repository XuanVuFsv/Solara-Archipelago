using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Collections;
using UnityEngine;
using VitsehLand.Scripts.Audio;
using VitsehLand.Scripts.Manager;

namespace VitsehLand.Scripts.Construction.Garden
{
    public class GardenBaseBuilder : MonoBehaviour
    {
        [Header("VFX")]
        public GameObject buildingVFX;
        public GameObject completeVFX;

        [Header("Object")]
        public GameObject garden;
        public GameObject buildingWall;
        //public GameObject lockedWall;
        public GameObject screenFrameActivate, screenFrame;

        public MonitorGardenController monitorScreen;

        [Header("Audio")]
        public AudioClip completeBuildingSound;
        public AudioClip startBuildSound;

        [Header("Animation value")]
        [Tooltip("Distance wall will move when excute animation: buil or complete buid")]
        public float movingWallYPosUp;
        [Tooltip("Distance wall will move when excute animation: buil or complete buid")]
        public float movingWallYPosDown;

        [Header("Interaction and timing")]
        private bool isLookAtMonitorScreen = false;
        public bool IsLookAtMonitorScreen
        {
            get
            {
                return isLookAtMonitorScreen;
            }
        }

        private bool canInteractMonitorScreen = false;
        public bool CanInteractMonitorScreen
        {
            get
            {
                return canInteractMonitorScreen && monitorScreen.gardenManager.HasBuilt;
            }
        }

        [Tooltip("Limit angle when player look at monitor screen. Calculating by two vector: vector from center of monitor to player camera and normal vector of monitor screen")]
        public float limitAngleToInteractBuildButton;
        [Tooltip("How many seconds building animation will run?")]
        public float timeForBuildingWallMove;

        public int timeToBuildGarden;
        public int unlockFee;
        public bool hasBuilt;

        private void OnTriggerStay(Collider other)
        {
            canInteractMonitorScreen = true;
            isLookAtMonitorScreen = CheckLookAtMonitorScreen(Camera.main.transform.forward, monitorScreen.screenViewPoint.transform.position - Camera.main.transform.position, limitAngleToInteractBuildButton);
            if (Input.GetKey(KeyCode.E) && isLookAtMonitorScreen)
            {
                //Debug.Log("Interactable");

                if (hasBuilt) return;

                bool canUseGem = GemManager.Instance.UseGem(unlockFee);
                if (!canUseGem)
                {
                    Debug.Log("Not enough gem");
                    return;
                }

                //Debug.Log("Build");
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

        private void OnTriggerExit(Collider other)
        {
            screenFrameActivate.SetActive(false);
            screenFrame.SetActive(true);
            canInteractMonitorScreen = false;
            isLookAtMonitorScreen = false;
        }

        TweenerCore<Vector3, Vector3, VectorOptions> StartBuildingWallAnimation(bool isActive)
        {
            if (isActive) buildingWall.SetActive(isActive);

            return buildingWall.transform.DOMoveY(isActive ? buildingWall.transform.position.y + movingWallYPosUp : buildingWall.transform.position.y - movingWallYPosDown, timeForBuildingWallMove);
        }

        IEnumerator StartBuildGarden()
        {
            yield return new WaitForSeconds(timeToBuildGarden);

            //Debug.Log("End Build");

            garden.SetActive(true);
            StartBuildingWallAnimation(false).OnComplete(() =>
            {
                buildingWall.SetActive(false);
                monitorScreen.SwitchMonitorState(true);
            });

            completeVFX.GetComponent<ParticleSystem>().Play();
            buildingVFX.SetActive(false);
            screenFrameActivate.SetActive(true);

            AudioBuildingManager.Instance.PlayAudioClip(completeBuildingSound);
        }

        public bool CheckLookAtMonitorScreen(Vector3 lookAtVector, Vector3 monitorScreenVector, float limitAngle)
        {
            float dotValue = Vector3.Dot(lookAtVector, monitorScreenVector);
            float angle = Mathf.Acos(dotValue / (lookAtVector.magnitude * monitorScreenVector.magnitude)) * Mathf.Rad2Deg;

            if (dotValue > 0)
            {
                if (angle <= limitAngle)
                {
                    screenFrameActivate.SetActive(true);
                    screenFrame.SetActive(false);
                    return true;
                }
                screenFrameActivate.SetActive(false);
                screenFrame.SetActive(true);
                return false;
            }
            screenFrameActivate.SetActive(false);
            screenFrame.SetActive(true);
            return false;
        }
    }
}