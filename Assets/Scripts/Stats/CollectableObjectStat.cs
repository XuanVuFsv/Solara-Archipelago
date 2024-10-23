using System.Collections.Generic;
using UnityEngine;
using VitsehLand.Assets.Scripts.Farming.General;
using VitsehLand.Scripts.Crafting;
using VitsehLand.Scripts.Weapon.General;

namespace VitsehLand.Scripts.Stats
{
    [CreateAssetMenu(fileName = "New Collectable Object", menuName = "Collectable Object")]
    public class CollectableObjectStat : ScriptableObject
    {
        [Header("Crop")]

        [Tooltip("Slot for Plant. All of ammo can used by slot 3 (collect/shoot out). Product type can't use slot 1 to attack so it's will be select slot3. While normal Plant can attack and collect/shoot out shoud be select slot 1")]
        public ActiveWeapon.WeaponSlot weaponSlot;

        public new string name;
        public int maxCount;
        public int ammoAllowedInMagazine;
        public int amplitudeGainImpulse;
        [Tooltip("Speed for plant when plant is Attack Plant and InstantiateBullet or Normal Plant")]
        public int force;
        public float fireRate;

        public Sprite artwork;
        public Crop cropPrefab;

        public enum ShootingHandleType
        {
            None = -1,
            Raycast = 0,
            InstantiateBullet = 1,
            Both = 2
        }
        public ShootingHandleType shootingHandleType;
        public GameObject bulletObject;

        public GameObjectType.FeaturedType featuredType;

        [Tooltip("What effect will be ins when shooting Plant")]
        public enum BulletEffectComponent
        {
            None = -1,
            PlaceHole = 0,
            InstantiateTrail = 1,
            Both = 2
        }
        public BulletEffectComponent bulletEffectComponent;

        public GameObject trailTracer;
        public ParticleSystem hitEffectPrefab;

        public GameObject TrailTracer
        {
            get { return trailTracer; }
        }

        [Header("Attacking Crop")]
        public int damage;
        [Tooltip("Only for attacking Crop. With specific enemy. This fruit will deal more damage")]
        public int damageDealToSpecificEnemy;

        [Tooltip("Only for attack Crop. Scope mode of Crop")]
        public enum ZoomType
        {
            NoZoom = 0,
            CanZoom = 1,
            HasScope = 2
        }
        public ZoomType zoomType;

        [Tooltip("Only for attacking Plant. Range for raycast checking")]
        public int range;

        //public float runSpeed;
        public float reloadSpeed;
        public float multiplierRecoilOnAim;
        public float multiplierForAmmo;
        public List<Vector2> recoildPattern;

        [Tooltip("number of bullet or raycast will be instantiated when player shoot. Example 1 for rifle type (berry), sniper type(star) and 5 for shotgun type(tomato)")]
        public int bulletCount = 1;
        public List<Vector3> bulletDirectionPattern;

        [Header("Normal Crop")]
        public int totalGrowingTime;
        public int numberOfFruitPerSeed;

        public enum BodyType
        {
            None = -1,
            Tree = 0,
            Vegetable = 1,
            Climbing = 2
        }
        public BodyType bodyType;
        public int wateringTime;
        public int requiredLevel;
        public GameObject growingBody;

        public string description;
        public int gemEarnWhenKillEnemy;
        public int gemEarnWhenHaverst;
        [Header("Type of Resource")]
        public float totalProducingTime;
        //public int resourceContain;

        public GameObjectType.FilteredType filteredType;
        public RecipeData recipe;
        public int cost;
    }
}