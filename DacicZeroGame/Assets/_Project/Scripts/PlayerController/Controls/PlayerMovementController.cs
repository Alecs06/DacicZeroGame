using UnityEditor;
using UnityEngine;
namespace PlayerController
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(CapsuleCollider))]
    public class PlayerMovementController : MonoBehaviour
    {
        #region Fields
        [SerializeField] InputReader inputReader;
        [SerializeField] Rigidbody rb;
        [SerializeField] public Transform groundCheckPoint;
        [field: SerializeField] public bool Grounded { get; protected set; }
        [SerializeField] bool onSlope;
        RaycastHit slopeHit;
        Vector2 inputVector;
        #endregion

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            rb.useGravity = false;
            rb.freezeRotation = true;
        }

        private void OnEnable()
        {
            inputReader.EnablePlayerActions();
            inputReader.Jump += OnJump;
            inputReader.Move += OnMove;
        }

        private void OnDisable()
        {
            inputReader.Move -= OnMove;
            inputReader.Jump -= OnJump;
            inputReader.DisablePlayerActions();
        }

        private void Update()
        {
            //ground and slope check
            GroundCheck();
        }
        void GroundCheck()
        {
            Grounded = Physics.CheckSphere(groundCheckPoint.position, GlobalPlayerConfig.PlayerGroundCheckRadius, GlobalPlayerConfig.GroundLayerMask);
            if (Grounded)
            {
                SlopeCheck();
            }
            else
            {
                onSlope = false;
            }
        }
        void SlopeCheck()
        {
            Physics.Raycast(groundCheckPoint.position, -groundCheckPoint.up, out slopeHit, GlobalPlayerConfig.PlayerGroundCheckRadius, GlobalPlayerConfig.GroundLayerMask);
            onSlope = slopeHit.normal != Vector3.up;
        }
        private void FixedUpdate()
        {
            if (!Grounded)
            {
                //apply gravity
                rb.velocity -= transform.up * GlobalPlayerConfig.Gravity * Time.fixedTime;
            }
            Vector3 dir = (transform.forward * inputVector.y + transform.right * inputVector.x).normalized * GlobalPlayerConfig.PlayerSpeed;
            if (onSlope)
            {
                rb.velocity = Vector3.ProjectOnPlane(dir, slopeHit.normal);
            }
            else
            {
                rb.velocity = new Vector3(dir.x, rb.velocity.y, dir.z);
            }
        }
        public void OnMove(Vector2 inputVector)
        {
            this.inputVector = inputVector;
        }

        public void OnJump()
        {
            if (Grounded)
            {
                rb.AddForce(transform.up * GlobalPlayerConfig.JumpForce, ForceMode.Impulse);
            }
        }
    }
#if UNITY_EDITOR
    [CustomEditor(typeof(PlayerMovementController))]
    public class PlayerMovementDebug : Editor
    {
        public void OnSceneGUI()
        {
            var t = (PlayerMovementController)target;
            Handles.color = Color.yellow;
            Handles.DrawWireArc(t.groundCheckPoint.position, Vector3.up, Vector3.forward,
                360, GlobalPlayerConfig.PlayerGroundCheckRadius);
            Handles.DrawWireArc(t.groundCheckPoint.position, Vector3.forward, Vector3.up,
                360, GlobalPlayerConfig.PlayerGroundCheckRadius);
            Handles.DrawWireArc(t.groundCheckPoint.position, Vector3.right, Vector3.forward,
                360, GlobalPlayerConfig.PlayerGroundCheckRadius);
            Handles.color = Color.blue;
            Handles.DrawLine(t.groundCheckPoint.position, t.groundCheckPoint.position -
                new Vector3(0, GlobalPlayerConfig.PlayerGroundCheckRadius, 0));
        }
    }
#endif
}