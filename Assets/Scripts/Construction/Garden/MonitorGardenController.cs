using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using VitsehLand.Scripts.Stats;
using VitsehLand.Scripts.Manager;
using VitsehLand.Scripts.UI.MonitorGarden;

namespace VitsehLand.Scripts.Construction.Garden
{
    public class MonitorGardenController : MonoBehaviour
    {
        [System.Serializable]
        public class DisplayUIElementState
        {
            public TextMeshProUGUI text;

            public string defaultText;
            public string altText;
            public bool inDefault;

            public void ChangeState()
            {
                if (inDefault)
                {
                    text.text = altText;
                }
                else
                {
                    text.text = defaultText;
                }

                inDefault = !inDefault;
            }
        }
        [SerializeField]
        private bool seeGuide = false;

        [SerializeField]
        private GardenBaseBuilder gardenBaseBuilder;

        public GameObject screenOff;
        public Transform screenViewPoint;

        public TextMeshProUGUI buildingCountdown;
        public TextMeshProUGUI buildingStatus;
        [Range(0, 2)]
        public float startScale;
        [Range(0, 2)]
        public float endScale;

        public GardenManager gardenManager;

        [Header("Growing Status")]
        public GrowingSlotResourceManager fertilizerSlotStatuses;
        public GrowingSlotResourceManager waterSlotStatuses;

        [Header("Display element")]
        [SerializeField]
        private CropStats currentCropStats;
        public List<DisplayUIElementState> displayUIElementStates;

        public Image currentCropIcon;
        private Tween currentCropIconTween;
        public Image disableUnlockNewSlot;

        public GameObject wholeCircleUI;
        public bool inWholeState = false;

        public TextMeshProUGUI productName;
        public TextMeshProUGUI timeElapsed;
        public TextMeshProUGUI information;

        [Header("Upgrading element")]
        public List<GameObject> upgradingLevelStars;

        [Header("LandSlot element")]
        public List<GameObject> slotCells = new List<GameObject>();
        public RectTransform landSelectedElement;

        public int currentSlotIndex = 0;
        public int unlockFee = 100;

        public void Start()
        {
            UpdateBuildingStatus(true);
            if (currentCropStats) SetupDisplayCropStats();
        }

        private void Update()
        {
            if (gardenBaseBuilder.CanInteractMonitorScreen && gardenBaseBuilder.IsLookAtMonitorScreen)
            {
                if (Input.GetKey(KeyCode.F1))
                {
                    if (!seeGuide)
                    {
                        seeGuide = true;
                        foreach (DisplayUIElementState e in displayUIElementStates)
                        {
                            e.ChangeState();
                        }
                    }
                }

                if (Input.GetKeyDown(KeyCode.Z))
                {
                    UnlockNewSlot();
                }

                if (Input.GetKeyDown(KeyCode.X))
                {
                    ClearCurrentField();
                }

                if (Input.GetKeyDown(KeyCode.F))
                {
                    PreviousSlot();
                }

                if (Input.GetKeyDown(KeyCode.G))
                {
                    NextSlot();
                }
            }

            if (seeGuide)
            {
                if (Input.GetKeyUp(KeyCode.F1) || !gardenBaseBuilder.CanInteractMonitorScreen)
                {
                    seeGuide = false;
                    foreach (DisplayUIElementState e in displayUIElementStates)
                    {
                        e.ChangeState();
                    }
                }
            }

            timeElapsed.text = gardenManager.GetCurrentAvailableSlotIndex().countDownUI.GetTimeElapsed().ToString();
        }

        public void NextSlot()
        {
            currentSlotIndex = gardenManager.NextSlot();
            AssignCurrentCropStats(gardenManager.GetCurrentAvailableSlotIndex().GetCurrentCropStats());
            SetupDisplayCropStats();
            landSelectedElement.anchoredPosition = slotCells[currentSlotIndex].GetComponent<RectTransform>().parent.GetComponent<RectTransform>().anchoredPosition;
        }

        public void PreviousSlot()
        {
            currentSlotIndex = gardenManager.PreviousSLot();
            AssignCurrentCropStats(gardenManager.GetCurrentAvailableSlotIndex().GetCurrentCropStats());
            SetupDisplayCropStats();
            landSelectedElement.anchoredPosition = slotCells[currentSlotIndex].GetComponent<RectTransform>().parent.GetComponent<RectTransform>().anchoredPosition;
        }

        public void UnlockNewSlot()
        {
            ////DisableUnlockNewSlotButton();
            bool canUseGem = GemManager.Instance.UseGem(unlockFee);
            Debug.Log(unlockFee);
            if (!canUseGem)
            {
                Debug.Log("Not enough gem");
                return;
            }

            int slotindex = gardenManager.UnLockNewSlot();
            if (slotindex != -1)
            {
                slotCells[slotindex].SetActive(false);
            }
        }

        public void DisableUnlockNewSlotButton()
        {
            disableUnlockNewSlot.gameObject.SetActive(true);
        }

        public void ClearCurrentField()
        {
            bool canClear = gardenManager.ClearCurrentField();

            if (!canClear) return;
            AssignCurrentCropStats(gardenManager.GetCurrentAvailableSlotIndex().GetCurrentCropStats());
            SetupDisplayCropStats();
        }

        public void AssignCurrentCropStats(CropStats newCropStats)
        {
            currentCropStats = newCropStats;
        }

        /// <summary> Setup what you will see about crop on monitor by pass crop stats as CropStats </summary>
        public void SetupDisplayCropStats()
        {
            CheckCropUIState();

            currentCropIcon.sprite = currentCropStats.artwork;
            productName.text = currentCropStats.name.ToString();
            information.text = currentCropStats.description;
        }

        public void CheckCropUIState()
        {
            if (gardenManager.gardenSlotProperties[currentSlotIndex].HaveWholeCrop() && !inWholeState)
            {
                ChangeCropUIToWhole();
                inWholeState = true;
            }

            if (!gardenManager.gardenSlotProperties[currentSlotIndex].HaveWholeCrop() && inWholeState)
            {
                ChangeCropUIToOtherState();
                inWholeState = false;
            }
        }

        public void ChangeCropUIToWhole()
        {
            //Debug.Log("ChangeCropUIToWhole");
            StartInOutScaleTextMeshProUGUILoop(currentCropIcon, new Vector3(endScale, endScale, 1), LoopType.Yoyo, -1, 3);
            wholeCircleUI.SetActive(true);
        }

        public void ChangeCropUIToOtherState()
        {
            //Debug.Log("ChangeCropUIToOtherState");
            Debug.Log("Kill");
            currentCropIconTween.Kill();
            wholeCircleUI.SetActive(false);
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
                StartInOutScaleTextMeshProUGUILoop(buildingStatus, new Vector3(endScale, endScale, 1), LoopType.Yoyo, -1, 3);
            }
            buildingStatus.gameObject.SetActive(isActive);
        }

        /// <summary> Zoom in and zoom out text animation. </summary>
        public void StartInOutScaleTextMeshProUGUILoop(TextMeshProUGUI text, Vector3 endScale, LoopType loopType, int loopTime, float cycleTime)
        {
            text.rectTransform.DOScale(endScale, cycleTime / 2).SetLoops(-1, loopType).SetEase(Ease.Linear);
        }

        /// <summary> Zoom in and zoom out text animation. </summary>
        public void StartInOutScaleTextMeshProUGUILoop(Image icon, Vector3 endScale, LoopType loopType, int loopTime, float cycleTime)
        {
            currentCropIcon.rectTransform.localScale = Vector3.one;
            currentCropIconTween = icon.rectTransform.DOScale(endScale, cycleTime / 4).SetLoops(-1, loopType).SetEase(Ease.Linear).OnComplete(() => currentCropIcon.rectTransform.localScale = Vector3.one);
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
}