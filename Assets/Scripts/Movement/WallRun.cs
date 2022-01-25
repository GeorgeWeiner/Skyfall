using Audio;
using GameManagement;
using UnityEngine;

namespace Movement
{
    public class WallRun : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private Transform orientation;

        [Header("Detection")]
        [SerializeField] private float wallDistance = .5f;
        [SerializeField] private float minimumJumpHeight = 1.5f;
        [SerializeField] private float maxWallSwitchDistance;
        [SerializeField] private LayerMask wallLayer;

        [Header("Wall Running")]
        [SerializeField] private float wallRunGravity;
        [SerializeField] private float wallRunJumpForce;

        [Header("Camera")]
        [SerializeField] private Camera cam;
        [SerializeField] private float fov;
        [SerializeField] private float wallRunfov;
        [SerializeField] private float wallRunfovTime;
        [SerializeField] private float camTilt;
        [SerializeField] private float camTiltTime;

        public float Tilt { get; private set; }
        private bool _wallLeft = false;
        private bool _wallRight = false;

        RaycastHit _leftWallHit;
        RaycastHit _rightWallHit;
        RaycastHit _forwardWallHit;

        private Rigidbody _rb;

        bool CanWallRun()
        {
            return !Physics.Raycast(transform.position, Vector3.down, minimumJumpHeight);
        }

        private void Start()
        {
            _rb = GetComponent<Rigidbody>();
        }

        private void CheckWall()
        {
            var position = transform.position;
            var right = orientation.right;
        
            _wallLeft = Physics.Raycast(position, -right, out _leftWallHit, wallDistance);
            _wallRight = Physics.Raycast(position, right, out _rightWallHit, wallDistance);
        }

        private void Update()
        {
            CheckWall();
            if (CanWallRun())
            {
                if (_wallLeft)
                {
                    StartWallRun();
                }
                else if (_wallRight)
                {
                    StartWallRun();
                }
                else
                {
                    StopWallRun();
                }
            }
            else
            {
                StopWallRun();
            }
        }


        private void StartWallRun()
        {
            if (GameAssets.i.isPlayerControlling){
            
                GameAssets.i.isWallRunning = true;
                _rb.useGravity = false;

                //Adds artificial gravity for downforce during the wallrun.
                _rb.AddForce(Vector3.down * wallRunGravity, ForceMode.Force);

                cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, wallRunfov, wallRunfovTime * Time.deltaTime);

                if (_wallLeft){
                    Tilt = Mathf.Lerp(Tilt, -camTilt, camTiltTime * Time.deltaTime);
                } 
                else if (_wallRight){
                    Tilt = Mathf.Lerp(Tilt, camTilt, camTiltTime * Time.deltaTime);
                }

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    if (_wallLeft)
                    {
                        var wallRunJumpDirection = transform.up + GameAssets.i.Orientation.forward + _leftWallHit.normal;
                        var velocity = _rb.velocity;
                        velocity = new Vector3(velocity.x, 0, velocity.z);
                        _rb.velocity = velocity;
                        _rb.AddForce(wallRunJumpDirection * wallRunJumpForce * 100, ForceMode.Force);
                    }
                    else if (_wallRight)
                    {
                        var wallRunJumpDirection = transform.up + GameAssets.i.Orientation.forward * GameAssets.i.verticalMovement + _rightWallHit.normal;
                        _rb.velocity = new Vector3(_rb.velocity.x, 0, _rb.velocity.z); 
                        _rb.AddForce(wallRunJumpDirection * wallRunJumpForce * 100, ForceMode.Force);
                    }
                    SoundManager.PlaySound(SoundManager.Sound.PlayerWallJumping);
                }    
            }
        }

        private void StopWallRun()
        {
            if (GameAssets.i.isPlayerControlling){
                _rb.useGravity = true;

                cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, fov, wallRunfovTime * Time.deltaTime);
                Tilt = Mathf.Lerp(Tilt, 0, camTiltTime * Time.deltaTime);

                GameAssets.i.isWallRunning = false;
            }
        
        }
    }
}


