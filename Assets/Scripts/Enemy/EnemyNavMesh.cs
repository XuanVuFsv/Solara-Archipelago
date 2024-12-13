using UnityEngine;
using UnityEngine.AI;
using VitsehLand.Scripts.Stats;

namespace VitsehLand.Scripts.Enemy
{
    public class EnemyNavMesh : MonoBehaviour
    {
        public Transform movePosTransform;

        private NavMeshAgent navMeshAgent;
        public EnemyStat enemyStat;

        private void Awake()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            navMeshAgent.destination = movePosTransform.position;
        }

        public virtual void SetupAgentFromConfiguration()
        {
            navMeshAgent.acceleration = enemyStat.acceleration;
            navMeshAgent.angularSpeed = enemyStat.angularSpeed;
            navMeshAgent.areaMask = enemyStat.areaMask;
            navMeshAgent.avoidancePriority = enemyStat.avoidancePriority;
            navMeshAgent.baseOffset = enemyStat.baseOffset;
            navMeshAgent.height = enemyStat.height;
            navMeshAgent.obstacleAvoidanceType = enemyStat.obstacleAvoidanceType;
            navMeshAgent.radius = enemyStat.radius;
            navMeshAgent.speed = enemyStat.speed;
            navMeshAgent.stoppingDistance = enemyStat.stoppingDistance;
        }

        public virtual void OnEnable()
        {
            SetupAgentFromConfiguration();
        }

        private void OnDisable()
        {
            navMeshAgent.enabled = false;
        }
    }
}