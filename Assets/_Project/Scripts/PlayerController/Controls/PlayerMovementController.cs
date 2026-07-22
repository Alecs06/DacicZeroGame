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
        [SerializeField] Rigidbody PlayerBody;
        [SerializeField] CapsuleCollider capsuleCollider;
        [SerializeField] Transform camPivot;
        [SerializeField] Camera playerCamera;
        [SerializeField] public Transform groundCheckPoint;
        [field: SerializeField] public bool Grounded { get; protected set; }
        [SerializeField] bool onSlope;
        RaycastHit slopeHit;
        [SerializeField] Vector2 inputVector;
        #endregion

        private void Awake()
        {
            PlayerBody = GetComponent<Rigidbody>();
            capsuleCollider = GetComponent<CapsuleCollider>();
            PlayerBody.useGravity = false;
            PlayerBody.freezeRotation = true;
        }

        private void OnEnable()
        {
            inputReader.EnablePlayerActions();
            inputReader.Jump += OnJump;
            inputReader.Move += OnMove;
            inputReader.Crouch += OnCrouch;
            inputReader.Sprint += OnSprint;
            
        }

        private void OnDisable()
        {
            inputReader.Move -= OnMove;
            inputReader.Jump -= OnJump;
            inputReader.DisablePlayerActions();
            inputReader.Crouch -= OnCrouch;
            inputReader.Sprint -= OnSprint;
        }

        private void Update()
        {
            //ground and slope check
            //GroundCheck();   why is this in not in fixedupdate
            playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, targetFOV, 8f * Time.deltaTime);
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
            // NOTE: the movement might be a bit too floaty, but we ARE presumably chromed up so not sure if we want to make it too stiff
            GroundCheck();
            if (!Grounded)
            {
                //apply gravity
                PlayerBody.linearVelocity -= transform.up * GlobalPlayerConfig.Gravity * Time.fixedDeltaTime;
            }

            Vector3 targetDir = (transform.forward * inputVector.y + transform.right * inputVector.x).normalized * GlobalPlayerConfig.PlayerSpeed;
            targetFOV = (isSprinting && !isCrouching && Grounded) ? 65 : 60; // messing around, unsure if i like this but i think its the best so far
            if (isCrouching)
            {
                if (!isSprinting)
                {
                    targetDir *= GlobalPlayerConfig.PlayerCrouchSpeedMultiplier;
                }
                if (!Grounded && !isSliding && PlayerBody.linearVelocity.y <=0)
                {
                    // groundpound! (might not make the final cut)
                    PlayerBody.linearVelocity = new Vector3(0, GlobalPlayerConfig.JumpForce*(-2), 0);
                    isSprinting = false;
                }
                if (isSliding)
                {
                    if(PlayerBody.linearVelocity.magnitude < GlobalPlayerConfig.PlayerSpeed)
                    {
                        isSliding = false; // stop the slide if youre too slow
                        isSprinting = false;
                    }
                }
            }
            else if (isSprinting)
            {
                targetDir *= GlobalPlayerConfig.PlayerSprintSpeedMultiplier;
            }

            if (onSlope)
            {
                targetDir = Vector3.ProjectOnPlane(targetDir, slopeHit.normal);
            }

            float accel = isSliding ? 0 : 
                           Grounded ? GlobalPlayerConfig.PlayerAcceleration : 
                            GlobalPlayerConfig.PlayerAcceleration * GlobalPlayerConfig.AirControlMultiplier;

            Vector3 currentHorizontalVel = new Vector3(PlayerBody.linearVelocity.x, 0f, PlayerBody.linearVelocity.z);
            Vector3 newHorizontalVel = Vector3.MoveTowards(currentHorizontalVel, targetDir, accel * Time.fixedDeltaTime);

            PlayerBody.linearVelocity = new Vector3(newHorizontalVel.x, PlayerBody.linearVelocity.y, newHorizontalVel.z);
        }
        public void OnMove(Vector2 inputVector)
        {
            this.inputVector = inputVector;
        }

        public void OnJump()
        {
            if (Grounded)
            {
                PlayerBody.AddForce(transform.up * GlobalPlayerConfig.JumpForce, ForceMode.Impulse);
                if (isSliding)
                {
                    PlayerBody.AddForce(transform.forward * GlobalPlayerConfig.JumpForce, ForceMode.Impulse);
                }
            }
        }


        [SerializeField] bool isCrouching;
        [SerializeField] bool isSliding;
        public void OnCrouch(bool isHeld)
        {
            //TODO: hardcoded :)
            isCrouching = isHeld;

            if (isHeld)
            {
                capsuleCollider.height = 1f;
                //capsuleCollider.center = new Vector3(0, 1f, 0);
                camPivot.localPosition = new Vector3(camPivot.localPosition.x, 0.2f, camPivot.localPosition.z);
            }
            else
            {
                capsuleCollider.height = 2f;
                //capsuleCollider.center = new Vector3(0, 0f, 0);
                camPivot.localPosition = new Vector3(camPivot.localPosition.x, 0.5f, camPivot.localPosition.z);
            }
            isSliding = isHeld && isSprinting && Grounded;
            Physics.SyncTransforms();
        }

        [SerializeField] bool isSprinting;
        [SerializeField] float targetFOV = 60;
        public void OnSprint(bool isHeld)
        {
            if (!isCrouching) // cant start sprinting while crouched
            {
                isSprinting = isHeld;
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