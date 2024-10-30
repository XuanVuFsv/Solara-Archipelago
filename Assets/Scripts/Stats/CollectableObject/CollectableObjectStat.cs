using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VitsehLand.Scripts.Crafting;
using VitsehLand.Scripts.Farming.General;
using VitsehLand.Scripts.Weapon.General;

namespace VitsehLand.Scripts.Stats
{
    [CreateAssetMenu(fileName = "New Collectable Object", menuName = "Collectable Object")]
    public class CollectableObjectStat : ScriptableObject
    {
#if UNITY_EDITOR
        [System.Flags]
        public enum CollectableObjectComponentBitmaskEnum
        {
            FarmingCropStat = 1 << 1,
            AttackingCropStat = 1 << 2,
            FarmingProductStat = 1 << 3,
            NaturalResourceStat = 1 << 4,
            PowerStat = 1 << 5,
            WaterStat = 1 << 6,
            FertilizerStat = 1 << 7,
            All = FarmingCropStat | AttackingCropStat |
                FarmingProductStat | NaturalResourceStat |
                PowerStat | WaterStat | FertilizerStat
        }
        public List<string> componentEnums = Enum.GetNames(typeof(CollectableObjectComponentBitmaskEnum)).ToList();

        [Title("Choose Collectable Object Components")]
        [PropertySpace(SpaceBefore = 20, SpaceAfter = 10)]
        [OnValueChanged("UpdateContainingComponents")]
        public CollectableObjectComponentBitmaskEnum selectingComponents;

        public List<string> strSelectingComponents;
        public List<string> strComponents;
        public string namespaceName;

        private void UpdateContainingComponents()
        {
            namespaceName = (new CollectableObjectStatComponent()).GetType().Namespace + ".";

            strSelectingComponents = selectingComponents.ToString().Split(", ").ToList();

            if (selectingComponents.ToString() == "0")
            {
                Debug.Log("Clear");
                components.Clear();
                strComponents.Clear();
            }
            else
            {
                strComponents = components.Select(item => item.GetType().ToString().Replace(namespaceName, "")).ToList();

                Debug.Log("Remove: ");
                for (int i = 0; i < strComponents.Count; i++)
                {
                    if (strComponents[i] == null) continue;

                    if (!strSelectingComponents.Contains(strComponents[i]))
                    {
                        Debug.Log(strComponents[i] + " don't exist in selecting components anymore so...");

                        Debug.Log("Remove " + strComponents[i]);
                        components.Remove(components[i]);
                        strComponents.Remove(strComponents[i]);
                    }
                }

                Debug.Log("Add: ");
                foreach (var component in strSelectingComponents)
                {
                    if (!strComponents.Contains(component))
                    {
                        Debug.Log(component + " hasn't added to components so ...");
                        Debug.Log("Add new " + namespaceName + component);
                        components.Add((CollectableObjectStatComponent)Activator.CreateInstance(Type.GetType(namespaceName + component)));
                        strComponents.Add(component);
                    }
                }
            }
        }
#endif
        [PropertySpace(SpaceBefore = 20, SpaceAfter = 20)]
        [Title("Containing Components")]
        [InfoBox("Components are groups of data that the game uses to process the corresponding actions of the player with a Collectable Object. Not having Component A,B or C means that the player cannot perform the corresponding action on that Collectable Object.")]
        [SerializeReference]
        public List<CollectableObjectStatComponent> components;

        [Title("Base Collectable Object Attributes")]
        [InfoBox("A collectable object is set to Weapon Slot 3 by default. It will be assigned to Weapon Slot 1 or Weapon Slot 2 if it can be used in one of those respective slots.")]
        public ActiveWeapon.WeaponSlot weaponSlot;

        public string collectableObjectName;
        public int maxCount;

        /**/
        public int ammoAllowedInMagazine;
        public int amplitudeGainImpulse;
        /**/

        /**/
        [Tooltip("Speed for plant when plant is Attack Plant and InstantiateBullet or Normal Plant")]
        public int force;
        public float fireRate;
        /**/

        public Sprite artwork;
        public Crop cropPrefab;

        /**/
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

        public GameObjectType.FilteredType filteredType;
        public RecipeData recipe;
        public int cost;
    }
}