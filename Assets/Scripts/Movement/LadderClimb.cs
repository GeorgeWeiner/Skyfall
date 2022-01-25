using System.Collections;
using GameManagement;
using UnityEngine;
using UserInterface;

namespace Movement
{
    public class LadderClimb : MonoBehaviour, ITooltipProjector
    {
        /*Spaghetti code, I know...*/

        [SerializeField] private Rigidbody rb;
        [SerializeField] private float climbingSpeed;
        [SerializeField] private float ladderJumpForce;
        [SerializeField] private float ladderJumpBuffer;

        public static LadderClimb ladderClimb;

        private bool _isJumping;
        public static bool CanClimbLadder { get; private set; }

        private void Start()
        {
            ladderClimb = this;
            
            rb = GetComponent<Rigidbody>();
            GameAssets.i.isClimbingLadder = false;
        }

        private void Update()
        {
            if (!GameAssets.i.isClimbingLadder && CanClimbLadder && Input.GetKeyDown(KeyCode.E))
            {
                StartClimbing();
            }
            LadderJump();
            HideToolTip();
        }

        private void FixedUpdate()
        {
            ClimbLadder();
        }

        private void ClimbLadder()
        {
            if (!GameAssets.i.isClimbingLadder || _isJumping) return;
            if (Input.GetKeyDown(KeyCode.E))
            {
                StopClimbing();
            }
            else if (Input.GetKey(KeyCode.W)){
                rb.velocity = new Vector3(0f, climbingSpeed, 0f);
            }
            else if (Input.GetKey(KeyCode.S)){
                rb.velocity = new Vector3(0f, -climbingSpeed, 0f);
            }
            else if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S) && !_isJumping)
            {
                rb.velocity = Vector3.zero;
            }
        }

        private void LadderJump()
        {
            if (GameAssets.i.isClimbingLadder && Input.GetKeyDown(KeyCode.Space))
            {
                StartCoroutine(JumpBufferController());

                var ladderJumpDirection = transform.up + GameAssets.i.Orientation.forward;
                rb.AddForce(ladderJumpDirection * ladderJumpForce * 100, ForceMode.Force);
                rb.useGravity = true;
                
                GameAssets.i.isClimbingLadder = false;
                GameAssets.i.isPlayerControlling = true;
            }
        }

        private IEnumerator JumpBufferController()
        {
            var t = 0f;
            while (t <= ladderJumpBuffer)
            {
                _isJumping = true;
                t += Time.deltaTime;
                yield return new WaitForSeconds(Time.deltaTime); 
            }
            _isJumping = false;
            
            yield return null;
        }
        private void StartClimbing()
        {
            rb.useGravity = false;
            rb.velocity = Vector3.zero;
            CanClimbLadder = false;
            
            GameAssets.i.isClimbingLadder = true;
            GameAssets.i.isPlayerControlling = false;
        }

        private void StopClimbing()
        {
            rb.useGravity = true;

            GameAssets.i.isClimbingLadder = false; 
            GameAssets.i.isPlayerControlling = true;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Ladder")) CanClimbLadder = true;
            ShowToolTip();
            print("In range of the ladder");
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Ladder")) CanClimbLadder = false;
            HideToolTip();
            StopClimbing();
        }

        public void ShowToolTip()
        {
            if (CanClimbLadder && !GameAssets.i.isClimbingLadder)
            {
                TooltipSystem.Show("press e to climb");
            }
        }
        public void HideToolTip()
        {
            if (GameAssets.i.isClimbingLadder)
            {
                TooltipSystem.Hide();
            }
        }
    }
}
