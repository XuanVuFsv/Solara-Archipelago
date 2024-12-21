using System.Collections;
using UnityEngine;
using VitsehLand.Scripts.Pattern.Observer;
using VitsehLand.Scripts.Stats;
using VitsehLand.Scripts.Audio;
using VitsehLand.Scripts.Pattern.Strategy;
using VitsehLand.Scripts.Ultilities;
using VitsehLand.Scripts.Weapon.General;
using VitsehLand.Assets.Scripts.Weapon.General;
using VitsehLand.Scripts.Weapon.HandGun;
using VitsehLand.Assets.Scripts.Weapon.Collector;
using VitsehLand.Assets.Scripts.Ultility;

namespace VitsehLand.Scripts.Player
{
    public class ShootController : MonoBehaviour
    {
        public static class ShootingState
        {
            public static string None = "";
            public static string Aim = "Aim ";
        }

        [Header("Shooting")]
        public ActiveWeapon activeWeapon;
        public WeaponStatsController currentWeaponStatsController;
        public bool isFire;
        public RaycastWeapon raycastWeapon;

        [Header("Aiming")]
        [SerializeField]
        private PlayerAim playerAim;

        public InputController inputController;

        [Header("Reload")]
        public Animator rigController;
        public WeaponAnimationEvents animationEvents;
        public GameObject leftHand, magazineObject, magazineHand;
        private Vector3 newMagazineLocalPosition;
        private Vector3 newMagazineLocalEulerAngles;

        [Header("Events")]
        public GameEvent aimEvent;
        [SerializeField]
        GameEvent fireEvent;
        [SerializeField]
        GameEvent reloadEvent;

        [Header("Shooting Information")]
        private float lastFired;
        public bool isReloading = false;
        public bool inAim = false;
        public bool readyToFire = true;
        public bool canPunch = true;
        public int shootingTime = 0;

        public Collider punchCollider;

        int frame = 0;

        public AudioSource source;

        #region Advance Settings for muzzle and flash effect
        //[Header("Muzzleflash Settings")]
        //public ParticleSystem sparkParticles;
        //public ParticleSystem muzzleParticles;
        //public int maxRandomValue = 5;
        //public int minSparkEmission = 1;
        //public int maxSparkEmission = 7;
        //public bool randomMuzzleflash = false;
        //[Range(2, 25)]
        //public bool enableMuzzleflash = true;
        //public bool enableSparks = true;

        //private int minRandomValue = 1;
        //private int randomMuzzleflashValue;

        //[Header("Muzzleflash Light Settings")]
        //public Light muzzleflashLight;
        //public float lightDuration = 0.02f;
        #endregion

        #region Testing
        //float time = 0;
        #endregion

        // Start is called before the first frame update
        void Start()
        {
            //MyDebug.Log("Created");

            animationEvents.weaponAnimationEvent.AddListener(OnAnimationEvent);
            inputController = GetComponent<InputController>();
            playerAim = GetComponent<PlayerAim>();
            activeWeapon = GetComponent<ActiveWeapon>();
        }

        // Update is called once per frame
        void Update()
        {
            frame++;

            //Out of ammo check
            CheckOuOfAmmo();

            RightMouseBehaviourHandle();

            LeftMouseBehaviourHandle();

            //Reload handling
            ReloadHandle();

            Punch();
        }

        void CheckOuOfAmmo()
        {
            if (currentWeaponStatsController.IsOutOfAmmo())
            {
                //Toggle bool
                isFire = false;
            }
        }

        void Punch()
        {
            if (Input.GetKeyDown(KeyCode.LeftControl) && canPunch && currentWeaponStatsController.weaponSlot == ActiveWeapon.WeaponSlot.AttackGun)
            {
                AudioBuildingManager.Instance.audioSource.volume = 1;
                AudioBuildingManager.Instance.PlayAudioClip(AudioBuildingManager.Instance.punch);
                StartCoroutine(PunchCountDown());
            }
        }

        IEnumerator PunchCountDown()
        {
            canPunch = false;
            rigController.Play("Punch");
            punchCollider.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.25f);
            punchCollider.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            canPunch = true;
        }

        void RightMouseBehaviourHandle()
        {
            if (inputController.isAim)
            {
                if (raycastWeapon.weaponHandler is IPrimaryWeaponStrategy && !isReloading)
                {
                    raycastWeapon.HandleRightMouseClick();
                    MyDebug.LogCaller();
                }
                else
                {
                    if (raycastWeapon.weaponHandler is ICollectorWeaponStrategy)
                    {
                        if (Time.time - lastFired > 1 / 5f)
                        {
                            lastFired = Time.time;
                            raycastWeapon.HandleRightMouseClick();
                            MyDebug.LogCaller();
                        }
                    }
                    else
                    {
                        raycastWeapon.HandleRightMouseClick();
                        MyDebug.LogCaller();
                    }
                }
            }

            if (inputController.isHoldAim)
            {
                if (raycastWeapon.weaponHandler is ICollectorWeaponStrategy)
                {
                    if ((raycastWeapon.weaponHandler as CollectHandler).waterMode != CollectHandler.WaterMode.Off)
                    {
                        raycastWeapon.HandleRightMouseClick();
                        MyDebug.LogCaller();
                    }
                }
            }

            if (inputController.isStopFire)
            {
                //AudioBuildingManager.Instance.suckUpSound.Stop();
                AudioBuildingManager.Instance.suckUpSound.mute = true;
            }
        }

        void LeftMouseBehaviourHandle()
        {
            #region Logic for PrimaryWeapon
            if (raycastWeapon.weaponHandler is IPrimaryWeaponStrategy)
            {
                if ((inputController.isFire)
                //|| inputController.isSingleFire && activeWeapon.activeWeaponIndex != 0)
                && !currentWeaponStatsController.IsOutOfAmmo() && !isReloading)
                {
                    MyDebug.Log("Shoot");
                    //Shoot automatic
                    if (Time.time - lastFired > 1 / currentWeaponStatsController.currentCollectableObjectStatController.fireRate)
                    {
                        readyToFire = true;

                        //Debug.Log("Shoot");
                        lastFired = Time.time;

                        //Remove 1 bullet from ammo
                        //currentWeaponStatsController.UseAmmo(currentWeaponStatsController.currentCropStatsController.bulletCount);
                        raycastWeapon.HandleLeftMouseClick();
                        shootingTime++;
                        //currentWeaponStatsController.UpdateAmmoUI();

                        isFire = true;

                        if (currentWeaponStatsController.currentCollectableObjectStatController.collectableObjectStat.zoomType == CollectableObjectStat.ZoomType.HasScope && inAim)
                        {
                            //MyDebug.Log("Handle Right Click");
                            //MyDebug.Log(frame);
                            MyDebug.LogCaller();
                            raycastWeapon.HandleRightMouseClick();
                        }
                    }
                    else
                    {
                        DeactivateShooting();
                    }
                }

                if (inputController.isStopFire)
                {
                    isFire = false;
                    raycastWeapon.StopFiring();
                    DeactivateShooting();
                }
            }
            else if (raycastWeapon.weaponHandler is IHandGunWeaponStrategy)
            {
                if (inputController.isSingleFire && !(raycastWeapon.weaponHandler as ActionHandler).inGrapple) raycastWeapon.HandleLeftMouseClick();
            }
            else if (raycastWeapon.weaponHandler is ICollectorWeaponStrategy)
            {
                if (inputController.isFire) raycastWeapon.HandleLeftMouseClick();
            }
            #endregion
        }

        void DeactivateShooting()
        {
            readyToFire = false;
            rigController.SetBool("inAttack", false);
        }

        void ReloadHandle()
        {
            if ((inputController.isReload && !currentWeaponStatsController.IsFullMagazine()
                || currentWeaponStatsController.autoReload && currentWeaponStatsController.IsOutOfAmmo())
                && currentWeaponStatsController.weaponSlot != ActiveWeapon.WeaponSlot.AxieCollector
                && !isReloading
                && currentWeaponStatsController.IsContainAmmo())
            {
                //MyDebug.Log("Reload " + inputController.isReload + currentWeaponStatsController.IsFullMagazine() + currentWeaponStatsController.IsOutOfAmmo() + isReloading + currentWeaponStatsController.IsContainAmmo());
                StartCoroutine(Reload());
            }
        }

        //Reload handling
        IEnumerator Reload()
        {
            if (activeWeapon.activeWeaponIndex == (int)ActiveWeapon.WeaponSlot.AttackGun)
            {
                MyDebug.Log("Reload");

                rigController.SetTrigger("ReloadAK");
                rigController.SetBool("reloading", true);
                rigController.SetBool("inAim", false);
            }

            isReloading = true;
            MyDebug.Log(isReloading);
            if (inAim)
            {
                inAim = false;
                aimEvent.Notify(inAim);
            }

            yield return currentWeaponStatsController.currentCollectableObjectStatController.reloadTimer;

            //Restore ammo when reloading
            UpdateEndedReloadStats(true);
        }

        public void UpdateEndedReloadStats(bool ended)
        {
            if (ended)
            {
                if (activeWeapon.activeWeaponIndex == (int)ActiveWeapon.WeaponSlot.AttackGun)
                {
                    MyDebug.Log("Reload");
                    rigController.SetTrigger("ReloadAK");
                }
            }

            //if (ActiveWeapon.equippedWeapon[activeWeapon.activeWeaponIndex])
            rigController.SetBool("reloading", false);
            RefillMagazine();
        }

        public void ApplyAttackAnimation()
        {
            //MyDebug.Log("Checking inAttack");
            if (activeWeapon.GetActiveWeaponPickup().weaponSlot == ActiveWeapon.WeaponSlot.HandGun) rigController.SetBool("inAttack", inputController.isSingleFire);
            else rigController.SetBool("inAttack", true);
        }

        public void ApplyAimingAttributes()
        {

        }

        //Apply aim value to reference class. Affect to other attributes. Ex: recoild pattern, animation, player's movement speed
        public void ApllyAimValue(float val)
        {
            //MyDebug.Log($"Apply this value: {val} when aim");
        }

        #region Animation Events Handling
        void OnAnimationEvent(string eventName)
        {
            switch (eventName)
            {
                case "detach_magazine":
                    DeTachMagazine();
                    MyDebug.Log("Detach event");
                    break;
                case "drop_magazine":
                    MyDebug.Log("Drop event");
                    DropMagazine();
                    break;
                case "refill_magazine":
                    MyDebug.Log("Refill event");
                    //Avoid delay and unexpected behaviour. RefillMagazine will execute in UpdateEndedReloadStats()
                    break;
                case "attach_magazine":
                    MyDebug.Log("Attach event");
                    AttachMagazine();
                    break;
                case "take_new_magazine":
                    MyDebug.Log("Take new event");
                    TakeNewMagazine();
                    break;
            }
        }

        void DeTachMagazine()
        {
            MyDebug.Log("Run Detach funtion");
            if (!magazineHand) magazineHand = Instantiate(magazineObject, leftHand.transform, true);

            newMagazineLocalPosition = magazineHand.transform.localPosition;
            newMagazineLocalEulerAngles = magazineHand.transform.localEulerAngles;

            magazineObject.SetActive(false);
        }

        void DropMagazine()
        {
            MyDebug.Log("Run Drop funtion");
            magazineHand.GetComponent<Rigidbody>().isKinematic = false;
        }

        void TakeNewMagazine()
        {
            MyDebug.Log("Run Take New funtion");
            magazineHand.GetComponent<Rigidbody>().isKinematic = true;
            magazineHand.transform.localPosition = newMagazineLocalPosition;
            magazineHand.transform.localEulerAngles = newMagazineLocalEulerAngles;
        }

        void RefillMagazine()
        {
            MyDebug.Log("Run Refill function");
            isReloading = false;
            currentWeaponStatsController.UpdateAmmoAfterReload();
        }

        public void AttachMagazine()
        {
            MyDebug.Log("Run AttachMagazine function");
            if (magazineObject != null) magazineObject.SetActive(true);
            Destroy(magazineHand);
        }

        public void ResetMagazine()
        {
            MyDebug.Log("Run ResetMagazine funtion");
            if (magazineHand) Destroy(magazineHand);
        }
        #endregion
    }
}