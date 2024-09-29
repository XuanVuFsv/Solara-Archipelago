using UnityEngine;
using UnityEngine.AI;
using VitsehLand.Scripts.Stats;

namespace VitsehLand.Scripts.Enemy
{
    public class EnemyNavMesh : MonoBehaviour
    {
        public Transform movePosTransform;

        private NavMeshAgent navMeshAgent;
        public EnemyStats stats;

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
            navMeshAgent.acceleration = stats.acceleration;
            navMeshAgent.angularSpeed = stats.angularSpeed;
            navMeshAgent.areaMask = stats.areaMask;
            navMeshAgent.avoidancePriority = stats.avoidancePriority;
            navMeshAgent.baseOffset = stats.baseOffset;
            navMeshAgent.height = stats.height;
            navMeshAgent.obstacleAvoidanceType = stats.obstacleAvoidanceType;
            navMeshAgent.radius = stats.radius;
            navMeshAgent.speed = stats.speed;
            navMeshAgent.stoppingDistance = stats.stoppingDistance;
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