using UnityEngine;
using VitsehLand.GameCamera.Shaking;
using VitsehLand.Scripts.Pattern.Observer;
using VitsehLand.Scripts.Ultilities;

namespace VitsehLand.Scripts.Player
{
    public class MovementController : GameObserver
    {
        public LayerMask groundCheckLayers = -1;

        public float baseMovementSpeed, walkSpeed, onAimOrFireSpeed, jumpForce, multiplierSpeed;
        public float jumpDelay;
        public float groundCheckDistance = 0.05f;
        public float skinWidth = 0.02f;
        public float slopeLimit;
        public float currentSlope;
        public bool readyToJump = true, allowJump = true;
        public bool isGrounded;

        private Transform followTargetTransform;
        [SerializeField]
        private new Rigidbody rigidbody;
        [SerializeField]
        private Vector3 movementDirection;

        [SerializeField]
        private Animator rigController;
        [SerializeField] GameEvent aimEvent;
        private MainPlayerAnimator mainCharacterAnimator;
        private ShootController shootController;
        private InputController inputController;
        private CapsuleCollider capsuleCollider;
        private CameraShake cameraShake;

        private Vector3 m_GroundNormal;

        public float k_GroundCheckDistanceInAir = 0.07f;

        #region Test
        //public GameObject cubeRigidbody;
        //public int forceTest;
        #endregion

        // Start is called before the first frame update
        void Start()
        {
            rigidbody = GetComponent<Rigidbody>();
            capsuleCollider = GetComponent<CapsuleCollider>();
            inputController = GetComponent<InputController>();
            shootController = GetComponent<ShootController>();
            mainCharacterAnimator = GetComponent<MainPlayerAnimator>();
            cameraShake = GetComponent<CameraShake>();

            multiplierSpeed = 1;
        }

        private void Update()
        {
            GroundCheck();

            // jumping
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //Debug.Log("Jump");
                if (isGrounded)
                {
                    isGrounded = false;
                    m_GroundNormal = Vector3.up;
                    Jump(1);
                }
            }
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            Move();

            #region ExecuteTest
            //cubeRigidbody.transform.Rotate(Vector3.forward * forceTest);
            #endregion
        }

        public void SetMultiplierSpeed(float value)
        {
            MyDebug.Log("Set Multiplier Speed");
            multiplierSpeed = value;
        }

        void Move()
        {
            movementDirection = (transform.forward * inputController.rawVertical + transform.right * inputController.rawHorizontal).normalized;
            PlayMovingCameraEffect(!inputController.isIdle);

            rigidbody.MovePosition(rigidbody.position + movementDirection * baseMovementSpeed * multiplierSpeed * Time.deltaTime);
        }

        void GroundCheck()
        {
            // Make sure that the ground check distance while already in air is very small, to prevent suddenly snapping to ground
            float chosenGroundCheckDistance =
                isGrounded ? skinWidth + groundCheckDistance : k_GroundCheckDistanceInAir;

            m_GroundNormal = Vector3.up;

            // only try to detect ground if it's been a short amount of time since last jump; otherwise we may snap to the ground instantly after we try jumping
            if (true)
            {
                {
                    //Debug.LogFormat("GetCapsuleBottomHemisphere: {0} GetCapsuleTopHemisphere: {1} height: {2}", GetCapsuleBottomHemisphere(), GetCapsuleTopHemisphere(capsuleCollider.height), capsuleCollider.height);
                    //Debug.LogFormat("down: {0} chosenGroundCheckDistance: {1}", Vector3.down, chosenGroundCheckDistance);
                }

                // if we're grounded, collect info about the ground normal with a downward capsule cast representing our character capsule
                if (Physics.CapsuleCast(GetCapsuleBottomHemisphere(), GetCapsuleTopHemisphere(capsuleCollider.height),
                    capsuleCollider.radius, Vector3.down, out RaycastHit hit, chosenGroundCheckDistance, groundCheckLayers,
                    QueryTriggerInteraction.Ignore) || currentSlope <= slopeLimit && Physics.Raycast(transform.position, Vector3.down, out hit, 0.05f, groundCheckLayers))
                {
                    // storing the upward direction for the surface found
                    m_GroundNormal = hit.normal;

                    {
                        //Debug.Log(hit.collider.name);
                        //Debug.LogFormat("m_GroundNormal: {0} up: {1} Dot: {2} Angle: {3}", hit.point, transform.up, Vector3.Dot(hit.normal, transform.up), Vector3.Angle(transform.up, hit.normal));
                    }

                    // Only consider this a valid ground hit if the ground normal goes in the same direction as the character up
                    // and if the slope angle is lower than the character controller's limit
                    if (Vector3.Dot(hit.normal, transform.up) > 0f &&
                        IsNormalUnderSlopeLimit(m_GroundNormal))
                    {
                        isGrounded = true;
                        if (mainCharacterAnimator.animator.GetBool("isJumporFloat"))
                        {
                            mainCharacterAnimator.SetJumpAnimationParameter(isGrounded, false, 0, 0);
                        }
                    }
                    else if (!IsNormalUnderSlopeLimit(m_GroundNormal))
                    {
                        isGrounded = true;
                        //Debug.Log("Not same direction or slope");
                    }
                }
                else
                {
                    isGrounded = false;
                    if (!mainCharacterAnimator.animator.GetBool("isJumporFloat"))
                    {
                        //Debug.Log("Falling");
                        mainCharacterAnimator.SetJumpAnimationParameter(isGrounded, true, 0, 0);
                    }
                    //Debug.Log("Not collide ground");
                }
            }
        }

        //private void OnDrawGizmos()
        //{
        //    Gizmos.color = Color.red;
        //    Gizmos.DrawWireSphere(GetCapsuleBottomHemisphere(), capsuleCollider.radius);
        //    Gizmos.DrawWireSphere(GetCapsuleTopHemisphere(capsuleCollider.height), capsuleCollider.radius);
        //}

        // Returns true if the slope angle represented by the given normal is under the slope angle limit of the character controller

        bool IsNormalUnderSlopeLimit(Vector3 normal)
        {
            currentSlope = Vector3.Angle(transform.up, normal);
            return Vector3.Angle(transform.up, normal) <= slopeLimit;
        }

        // Gets the center point of the bottom hemisphere of the character controller capsule    
        Vector3 GetCapsuleBottomHemisphere()
        {
            return transform.position + transform.up * capsuleCollider.radius;
        }

        // Gets the center point of the top hemisphere of the character controller capsule    
        Vector3 GetCapsuleTopHemisphere(float atHeight)
        {
            return transform.position + transform.up * (atHeight - capsuleCollider.radius);
        }

        void Jump(float jumpProcessValue)
        {
            mainCharacterAnimator.SetJumpAnimationParameter(isGrounded, true, jumpProcessValue, 0.5f);
            readyToJump = true;
            rigidbody.AddForce(transform.up * jumpForce);
        }

        void PlayMovingCameraEffect(bool isMove)
        {
            rigController.SetBool("inMove", isMove);
        }

        public override void Execute(IGameEvent gEvent, bool val)
        {
            //MyDebug.Log($"Execute by {this} in base class with value: {val}");

            if (val)
            {
                //inAim = true
                SetMultiplierSpeed(onAimOrFireSpeed / baseMovementSpeed); //argument as scale value
            }
            else
            {
                //inAim = false
                SetMultiplierSpeed(walkSpeed / baseMovementSpeed); //argument as scale value
            }
        }

        private void OnEnable()
        {
            aimEvent.Subscribe(this);
        }

        private void OnDisable()
        {
            aimEvent.UnSubscribe(this);
        }
    }
}