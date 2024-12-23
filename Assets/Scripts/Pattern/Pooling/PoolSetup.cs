using UnityEngine;
using VitsehLand.Scripts.Pattern.Observer;

namespace VitsehLand.Scripts.Pattern.Pooling
{

    //public class ObjectInPoolInitCount
    //{
    //    public GameObject prefab;
    //    public int count;
    //}

    public class PoolSetup : GameObserver, IPoolSetup
    {
        static readonly float DEFAULT_MAX_POOL_SIZE_MULTIPLIER = 1.2f;

        [Tooltip("Check this to mark pool has multiple object")]
        public bool isSameObject;
        [Tooltip("This prefab will be used if isSameObject true")]
        public GameObject prefab;
        //[Tooltip("This list will be used if isSameObject false")]
        //public List<ObjectInPoolInitCount> multipleDifferentObjectList = new List<ObjectInPoolInitCount>();

        public string poolManagerName;
        [SerializeField] GameEvent gameEvent;

        public Pool<ObjectInPool> pool;
        public ObjectInPool currentObject;

        public float spawnInterval;
        [Tooltip("When using multipleDifferentObjectList, init pool size is predetermined which is number of all object instantiated base on ObjectInPoolInitCount at init. Max pool size is flexible")]
        [SerializeField] int initPoolSize, maxPoolSize;

        private void Start()
        {
            poolManagerName = poolManagerName != "" ? poolManagerName : gameObject.name;

            if (spawnInterval != 0 && prefab != null)
            {
                initPoolSize = (int)(prefab.GetComponent<ObjectInPool>().lifeTime / spawnInterval) + 1;

                if (maxPoolSize <= initPoolSize) maxPoolSize = (int)(initPoolSize * DEFAULT_MAX_POOL_SIZE_MULTIPLIER);
            }

            if (spawnInterval == 0) Debug.LogWarning("You should check spawnInterval. If you target it equal to 0. Just ignore this warning");

            if (prefab == null && initPoolSize > 0) Debug.LogError("You should set initPoolSize greater than 0 to initialize the pool");
        }

        public IPool InitPool(string poolManagerName, int initPoolSize, int maxPoolSize, GameObject prefab, GameEvent gameEvent)
        {
            this.prefab = prefab;
            this.poolManagerName = poolManagerName != "" ? poolManagerName : gameObject.name;
            this.gameEvent = gameEvent;
            this.initPoolSize = initPoolSize;
            this.maxPoolSize = maxPoolSize;

            //The time between spawns will be equal to the lifetime of an object divided by the minimum number of objects that need to be spawned - initPoolSize
            //This ensures that when a call is made, there is always an object that has completed its lifetime and is ready for use
            spawnInterval = prefab.GetComponent<ObjectInPool>().lifeTime / initPoolSize;
            
            pool = new Pool<ObjectInPool>(new PrefabFactory<ObjectInPool>(prefab, transform), initPoolSize);
            return pool;
        }

        public IPool InitPool(string poolManagerName, float spawnInterval, GameObject prefab, GameEvent gameEvent, float maxPoolSizeMultiplier = 0)
        {
            this.prefab = prefab;
            this.poolManagerName = poolManagerName != "" ? poolManagerName : gameObject.name;
            this.gameEvent = gameEvent;

            this.spawnInterval = spawnInterval;
            this.initPoolSize = (int)(prefab.GetComponent<ObjectInPool>().lifeTime / spawnInterval) + 1;
            this.maxPoolSize = (int)(maxPoolSizeMultiplier == 0 ? (initPoolSize * DEFAULT_MAX_POOL_SIZE_MULTIPLIER) : initPoolSize * maxPoolSizeMultiplier);

            pool = new Pool<ObjectInPool>(new PrefabFactory<ObjectInPool>(prefab, transform), initPoolSize);
            return pool;
        }

        //public IPool InitPool(string poolManagerName, int maxPoolSize, List<ObjectInPoolInitCount> list, GameEvent gameEvent)
        //{
        //    this.multipleDifferentObjectList = list;
        //    this.poolManagerName = poolManagerName;
        //    this.gameEvent = gameEvent;

        //    int count = 0;
        //    foreach (ObjectInPoolInitCount infor in list)
        //    {
        //        count += infor.count;
        //    }
        //    this.initPoolSize = count;
        //    this.maxPoolSize = maxPoolSize;
        //    pool = new Pool<ObjectInPool>(new PrefabFactory<ObjectInPool>(prefab, transform), initPoolSize);
        //    return pool;
        //}

        public IPool InitPool()
        {
            pool = new Pool<ObjectInPool>(new PrefabFactory<ObjectInPool>(prefab, transform), initPoolSize);
            return pool;
        }

        public int GetMaxPoolSize() { return maxPoolSize; }

        public int GetPoolSize() { return pool.poolSize; }

        public string GetName() { return poolManagerName; }

        public void Get()
        {
            //Debug.Log("Get in PoolSetup");
            //Debug.Log(currentObject);
            currentObject = pool.Get();
        }

        public void Release()
        {
            pool.Release();
        }

        public void Reset()
        {

        }

        public void Dispose()
        {
            foreach (Transform poolObject in transform)
            {
                poolObject.GetComponent<ObjectInPool>().Dispose();
                //poolObject.GetComponent<BulletHoleBehaviour>().Dispose();
            }
            pool.Dispose();
        }

        public override void Execute(IGameEvent gEvent, RaycastHit hit)
        {
            //if (!currentObject)
            //{
            //    RemoveGameEvent();
            //    return;
            //}
            //MyDebug.Log("Call OnUsed");
            currentObject?.OnUsed(hit); // fix null in memory
        }


        public override void Execute(IGameEvent gEvent, Vector3 point, Vector3 normal)
        {
            //if (!currentObject)
            //{
            //    RemoveGameEvent();
            //    return;
            //}
            //Debug.Log(this);
            //Debug.Log(gameObject.transform.parent);
            //MyDebug.Log("Call OnUsed");
            currentObject?.OnUsed(point, normal); // fix null in memory
        }

        public void AddGameEvent()
        {
            AddGameEventToObserver(gameEvent);
        }

        public void RemoveGameEvent()
        {
            RemoveGameEventFromObserver(gameEvent);
        }

        void OnDestroy()
        {
            RemoveGameEvent();
        }

        void OnDisable()
        {
            RemoveGameEvent();
        }
    }
}