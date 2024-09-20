using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ammo", menuName = "Ammo")]
public class AmmoStats : ScriptableObject
{
    [Header("Type of Plant")]

    [Tooltip("Slot for Plant. All of ammo can used by slot 3 (collect/shoot out). Product type can't use slot 1 to attack so it's will be select slot3. While normal Plant can attack and collect/shoot out shoud be select slot 1")]
    public ActiveWeapon.WeaponSlot weaponSlot;

    public new string name;
    public int maxCount;
    public int ammoAllowedInMagazine;
    public int amplitudeGainImpulse;
    [Tooltip("Speed for plant when plant is Attack Plant and InstantiateBullet or Normal Plant")]
    public int force;
    public float fireRate;

    [Tooltip("Type of Plant. Now, it's can be same as Plant name")]
    public enum FruitType
    {
        Null = -1,
        Berry = 0,
        Chilli = 1,
        Onion = 2,
        Apple = 4,
        Star = 5,
        Tomato = 6,
        Waternelon = 7,
        Strawberry = 8
    }

    public Sprite artwork;
    public Plant fruitPrefab;

    public enum ShootingHandleType
    {
        None = -1,
        Raycast = 0,
        InstantiateBullet = 1,
        Both = 2
    }
    public ShootingHandleType shootingHandleType;

    public enum FeaturedType
    {
        None = -1,
        Product = 1,
        Normal = 2
    }
    public FeaturedType featuredType;

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
    public GameObject bulletObject;
    public ParticleSystem hitEffectPrefab;

    public GameObject GetTrailTracer()
    {
        return trailTracer;
    }

    [Header("Attacking Plant")]
    public int damage;
    [Tooltip("Only for attacking Plant. With Specific Enemy. This fruit will deal more damage")]
    public int damageDealToSpecificEnemy;

    [Tooltip("Only for attack Plant. Scope mode of Plant")]
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

    [Header("Normal Plant")]
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
    public enum FilteredType
    {
        Crop = 0,
        NaturePlant = 1,
        Fertilizer = 2,
        Mining = 3,
        Power = 4
    }
    public FilteredType filteredType;
    public RecipeData recipe;
}
