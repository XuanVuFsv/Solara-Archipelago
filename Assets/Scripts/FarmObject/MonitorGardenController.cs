using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class MonitorGardenController : MonoBehaviour
{
    public GameObject screenOff;
    public Transform screenViewPoint;

    public TextMeshProUGUI buildingCountdown;
    public TextMeshProUGUI buildingStatus;

    public void Start()
    {
        UpdateBuildingStatus(true);
    }

    public void SwitchMonitorState(bool isActive)
    {
        screenOff.SetActive(!isActive);
    }

    public void UpdateBuildingStatus(bool isActive)
    {
        if (isActive)
        {
            StartInOutScaleTextMeshProUGUILoop(buildingStatus, new Vector3(1, 1, 1), new Vector3(1.25f, 1.25f, 1), LoopType.Yoyo, -1, 3);
        }
        buildingStatus.gameObject.SetActive(isActive);
    }

    public void StartInOutScaleTextMeshProUGUILoop(TextMeshProUGUI text, Vector3 startScale, Vector3 endScale, LoopType loopType, int loopTime, float cycleTime)
    {
        text.rectTransform.DOScale(endScale, cycleTime / 2).SetLoops(-1, loopType).SetEase(Ease.Linear);
    }

    public void SetActiveBuildingCountdown(bool isActive)
    {
        buildingCountdown.gameObject.SetActive(isActive);
    }

    public void StartCountdownEffect(int timeToBuildingGarden)
    {
        DOVirtual.Int(timeToBuildingGarden, 0, timeToBuildingGarden, s => UpdateBuildingTime(s)).SetEase(Ease.Linear);
    }

    public void UpdateBuildingTime(float seconds)
    {
        buildingCountdown.text = seconds.ToString();
    }
}
