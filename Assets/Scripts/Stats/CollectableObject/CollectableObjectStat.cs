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
        [Flags]
        public enum CollectableObjectComponentBitmaskEnum
        {
            None = 0,
            FarmingCropStat = 1 << 1,
            AttackingCropStat = 1 << 2,
            CraftedProductStat = 1 << 3,
            NaturalResourceStat = 1 << 4,
            All = FarmingCropStat | AttackingCropStat | CraftedProductStat | NaturalResourceStat
        }
        List<string> componentEnums;

        string namespaceName;

        [PropertySpace(SpaceBefore = 20, SpaceAfter = 10)]
        [Title("Choose Collectable Object Components")]
        [OnValueChanged("UpdateContainingComponents")]
        public CollectableObjectComponentBitmaskEnum selectedComponents;
        // Convert componentEnums to a list of component names
        List<string> strSelectedComponents;
        // Convert containing components to a list of component names
        List<string> strComponents;

        public void UpdateContainingComponents()
        {
            componentEnums =  Enum.GetNames(typeof(CollectableObjectComponentBitmaskEnum)).ToList();
            namespaceName = (new CollectableObjectStatComponent()).GetType().Namespace + ".";

            //Add valid enum names from CollectableObjectComponentBitmaskEnum to list of selected component names
            if (selectedComponents.Equals(CollectableObjectComponentBitmaskEnum.All))
            {
                //All option
                strSelectedComponents.Clear();
                for (int i = 1; i < componentEnums.Count - 1; i++)
                {
                    strSelectedComponents.Add(componentEnums[i]);
                }
            }
            else
            {
                //Other options
                strSelectedComponents = selectedComponents.ToString().Split(", ").ToList();
            }

            //Add or Remove component of components based on selected components
            if (selectedComponents.Equals(CollectableObjectComponentBitmaskEnum.None))
            {
                //Clear for None option
                components.Clear();
                strComponents.Clear();
            }
            else
            {
                //Get list of containing component names to compare with the list of selected component names
                //Then remove non-selected components or add newly selected components
                strComponents = components.Select(item => item.GetType().ToString().Replace(namespaceName, "")).ToList();

                //Removal check
                for (int i = 0; i < strComponents.Count; i++)
                {
                    //Stop removal check if there are no components or if all components will be added
                    if (strComponents[i] == null || selectedComponents.Equals(CollectableObjectComponentBitmaskEnum.All)) break;

                    //Remove components that are no longer selected
                    if (!strSelectedComponents.Contains(strComponents[i]))
                    {
                        components.Remove(components[i]);
                        strComponents.Remove(strComponents[i]);
                    }
                }

                //Addition check
                foreach (var component in strSelectedComponents)
                {
                    //Check and add all components that exist in the list of selected components
                    if (!strComponents.Contains(component))
                    {
                        components.Add((CollectableObjectStatComponent)Activator.CreateInstance(Type.GetType(namespaceName + component)));
                        strComponents.Add(component);
                    }
                }
            }
        }

        [PropertySpace(SpaceBefore = 20, SpaceAfter = 20)]
        [Title("Containing Components")]
        [InfoBox("Components are groups of data that the game uses to process the corresponding actions of the player with a Collectable Object. Not having Component A,B or C means that the player cannot perform the corresponding action on that Collectable Object.")]
        [SerializeReference]
        public List<CollectableObjectStatComponent> components;
#endif

        [Title("Base Collectable Object Attributes")]
        [InfoBox("A collectable object is set to Weapon Slot 3 by default. It will be assigned to Weapon Slot 1 or Weapon Slot 2 if it can be used in one of those respective slots.")]
        public ActiveWeapon.WeaponSlot weaponSlot;
        public GameObjectType.FilteredType filteredType;

        public string baseId;
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

        public RecipeData recipe;
        public int cost;
    }
}