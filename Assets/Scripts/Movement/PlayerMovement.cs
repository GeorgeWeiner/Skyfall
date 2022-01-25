using Audio;
using GameManagement;
using UnityEngine;

namespace Movement
{
    public class PlayerMovement : MonoBehaviour
    { 
        #region variables   
        [Header("Movement")]
        public float walkingSpeed;
        public float sprintSpeed;
        public float crouchSpeed;
        public float movementMultiplier;
        private float _horizontalMovement;
        private float _verticalMovement;
        public float VerticalMovement {get; }
        private Vector3 _moveDirection;
        private Vector3 _slopeMoveDirection;
        public Transform orientation;
        [Range(0, 1)]
        [SerializeField] private float moveCounterGripForce;
    
        [Header("Drag")]
        public float groundDrag;
        public float airDrag;
        public float idleDrag;
    
        [Header("Jumping")]    
        public LayerMask layerMask;

        [SerializeField] private float jumpForce,
            groundDetectionSphereRadius,
            groundDetectionSphereYOffset,
            jumpMultiplier,
            gravityModifier;
    
        public float playerHeight;
        private SceneSwitcher _sceneSwitcher;
        private Rigidbody _rb;
        [SerializeField] private bool isGrounded;
        private bool _isCrouching;
        private RaycastHit _slopeHit;
        private GameObject _playerCapsule;
        private CameraController _moveCamera;

        public PlayerMovement(float verticalMovement)
        {
            VerticalMovement = verticalMovement;
        }

        #endregion

        private void Start() {
            _rb = GetComponent<Rigidbody>();
            _rb.freezeRotation = true;
            _sceneSwitcher = GameObject.Find("MenuManager").GetComponent<SceneSwitcher>();
            _playerCapsule = GameObject.Find("Player/PlayerCapsule");
            _moveCamera = FindObjectOfType<CameraController>();
        }

        private void Update() {
            CurrentMoveSpeed();
            MyInput();
            ControlDrag();
            Jump();
            Crouch();
            PlayMovementSound();

            isGrounded = Physics.CheckSphere(transform.position - new Vector3(0f, groundDetectionSphereYOffset, 0f), groundDetectionSphereRadius, layerMask);
            _slopeMoveDirection = Vector3.ProjectOnPlane(_moveDirection, _slopeHit.normal);
        }

        //Takes Input.GetAxisRaw as original Input values then adds them together in a normalized directional Vector.
        private void MyInput(){
            _horizontalMovement = Input.GetAxisRaw("Horizontal");
            _verticalMovement = Input.GetAxisRaw("Vertical");

            _moveDirection = orientation.forward * _verticalMovement + orientation.right * _horizontalMovement;
        }

        private void ControlDrag(){
            if (isGrounded){
                _rb.drag = groundDrag;   
            }
            if (isGrounded && _moveDirection == Vector3.zero){
                _rb.drag = idleDrag;
            }
            else {
                _rb.drag = airDrag;
            }
        }

        private void FixedUpdate() 
        {
            if (GameAssets.i.isPlayerControlling){
                MovePlayer();
            }
        }

        /*If player is on no slope, force will be added normally. If the player is on a slope the Force Vector gets projected on the normal of the slope.
    Therefore the force is applied on the normal of the slope, which results in consistent movement speed.
    Also adds force in the opposite direction, to counteract the unresponsiveness of the movement during directional changes. */
        private void MovePlayer()
        {
            switch (isGrounded)
            {
                //checks current state of player location and adds force to a rigidbody with according directional value as well as speed and downforce.
                case true when !OnSlope():
                    _rb.AddForce(_moveDirection.normalized * CurrentMoveSpeed() * movementMultiplier, ForceMode.Acceleration);
                    _rb.AddForce(-_moveDirection.normalized * CurrentMoveSpeed() * movementMultiplier * moveCounterGripForce, ForceMode.Acceleration);
                    break;
                case true when OnSlope():
                    _rb.AddForce(_slopeMoveDirection.normalized * CurrentMoveSpeed() * movementMultiplier, ForceMode.Acceleration);
                    _rb.AddForce(-_slopeMoveDirection.normalized * CurrentMoveSpeed() * movementMultiplier * moveCounterGripForce, ForceMode.Acceleration);
                    break;
                default:
                {
                    if(GameAssets.i.isWallRunning){
                        _rb.AddForce(_moveDirection.normalized * CurrentMoveSpeed() * movementMultiplier, ForceMode.Acceleration);
                        _rb.AddForce(-_moveDirection.normalized * CurrentMoveSpeed() * movementMultiplier * moveCounterGripForce, ForceMode.Acceleration);
                    }
                    else if (!isGrounded && !OnSlope()) {
                        _rb.AddForce(new Vector3(_moveDirection.x, _moveDirection.y - Gravity(), _moveDirection.z) * CurrentMoveSpeed() * movementMultiplier * jumpMultiplier, ForceMode.Acceleration);
                    }
                    break;
                }
            }
        }
    
        private float Gravity()
        {
            return GameAssets.i.isWallRunning ? 0f : gravityModifier;
        }

        //Checks if on Slope by comparing the normal with Vector3.up.
        private bool OnSlope(){
            Debug.DrawRay(transform.position, Vector3.down, Color.green, playerHeight / 2 + 0.5f);
            if (!Physics.Raycast(transform.position, Vector3.down, out _slopeHit, playerHeight + 0.5f, layerMask))
                return false;
            return _slopeHit.normal != Vector3.up;
        }
    
        private void Jump(){
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !_isCrouching && GameAssets.i.isPlayerControlling)
            {
                _rb.velocity = new Vector3(_rb.velocity.x, 0f, _rb.velocity.z);
                _rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);

                SoundManager.PlaySound(SoundManager.Sound.PlayerJumping);
            }
        }

        //To simulate a proper animation of a rig, the capsule simply scales down.
        //While the player crouches, he is unable to jump.
        private void Crouch(){
            if (Input.GetKey(KeyCode.LeftControl)){
                if (Input.GetKeyDown(KeyCode.LeftControl)){
                    _rb.AddForce(Vector3.down * 10f, ForceMode.Impulse);
                }
                _playerCapsule.transform.localScale = new Vector3(1f, 0.5f, 1f);
                _isCrouching = true;
            }
            else{
                _playerCapsule.transform.localScale = new Vector3(1f, 1f, 1f);
                _isCrouching = false;
            }
        }

        private float CurrentMoveSpeed()
        {
            if (Input.GetKey(KeyCode.LeftShift)){
                _moveCamera.SetCameraFov();
                return walkingSpeed;
            }
            if (Input.GetKey(KeyCode.LeftControl)){
                _moveCamera.SetCameraFov();
                return crouchSpeed;
            }
            _moveCamera.SetCameraFov(95f);
            return sprintSpeed;
        }

        private void PlayMovementSound()
        {
            //SoundManager.Sound[] runSounds =
            //{
            //    SoundManager.Sound.RunSound01,
            //    SoundManager.Sound.RunSound02,
            //    SoundManager.Sound.RunSound03,
            //    SoundManager.Sound.RunSound04,
            //    SoundManager.Sound.RunSound05,
            //    SoundManager.Sound.RunSound06,
            //    SoundManager.Sound.RunSound07,
            //    SoundManager.Sound.RunSound08,
            //    SoundManager.Sound.RunSound09
            //};
            //
            //if (_moveDirection != Vector3.zero && isGrounded){
            //    SoundManager.PlaySound(.5f, 1.2f, .2f, "runSounds", runSounds);
            //}

            if (_moveDirection != Vector3.zero && isGrounded)
            {
                SoundManager.PlaySound(SoundManager.Sound.PlayerRunning, 0.8f);
            }
        }
    }
}

