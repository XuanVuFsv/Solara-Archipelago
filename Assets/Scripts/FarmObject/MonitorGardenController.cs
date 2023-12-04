using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class MonitorGardenController : MonoBehaviour
{
    public GameObject screenOff;
    public Transform screenViewPoint;

    public TextMeshProUGUI buildingCountdown;
    public TextMeshProUGUI buildingStatus;

    [Header("Growing Status")]
    public GrowingSlotResourceManager fertilizerSlotStatuses;
    public GrowingSlotResourceManager waterSlotStatuses;

    public void Start()
    {
        UpdateBuildingStatus(true);
    }

    /// <summary> Turn on or turn off screen when garden was built. </summary>
    public void SwitchMonitorState(bool isActive)
    {
        screenOff.SetActive(!isActive);
    }

    /// <summary> When active, building status will show a guide tex with animation. When deactive, this will be disable and timer will be showed instead. </summary>
    public void UpdateBuildingStatus(bool isActive)
    {
        if (isActive)
        {
            StartInOutScaleTextMeshProUGUILoop(buildingStatus, new Vector3(1, 1, 1), new Vector3(1.5f, 1.5f, 1), LoopType.Yoyo, -1, 3);
        }
        buildingStatus.gameObject.SetActive(isActive);
    }

    /// <summary> Zoom in and zoom out text animation. </summary>
    public void StartInOutScaleTextMeshProUGUILoop(TextMeshProUGUI text, Vector3 startScale, Vector3 endScale, LoopType loopType, int loopTime, float cycleTime)
    {
        text.rectTransform.DOScale(endScale, cycleTime / 2).SetLoops(-1, loopType).SetEase(Ease.Linear);
    }

    public void SetActiveBuildingCountdown(bool isActive)
    {
        buildingCountdown.gameObject.SetActive(isActive);
    }

    public void StartCountdownAnimation(int timeToBuildingGarden)
    {
        DOVirtual.Int(timeToBuildingGarden, 0, timeToBuildingGarden, s => UpdateBuildingTime(s)).SetEase(Ease.Linear);
    }

    /// <summary> Update building time reamain. </summary>
    public void UpdateBuildingTime(float seconds)
    {
        buildingCountdown.text = seconds.ToString();
    }
}
