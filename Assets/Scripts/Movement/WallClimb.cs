using Audio;
using GameManagement;
using UnityEngine;
using UserInterface;

namespace Movement
{
    internal class WallClimb : MonoBehaviour, IWallClimb
    {
        [Range(0, 10)] [SerializeField] private float positionalOffset, yOffset;
        private CameraController _moveCamera;

        private void Awake()
        {
            _moveCamera = FindObjectOfType<CameraController>();
        }

        public void InitiateClimb(RaycastHit hit, Vector3 forward)
        {
            if (!Input.GetKeyDown(KeyCode.Space) || GameAssets.i.isWallRunning) return;
            //Calculates the offset of the player relative to the distance the player is located at the initialization of the function.                  
            var posOffset = positionalOffset * hit.distance + 0.1f;
            var bounds = hit.collider.bounds;
            var position = transform.position;

            var offset = new Vector3(forward.x * posOffset, 0f, forward.z * posOffset);

            GetComponent<Rigidbody>().velocity = Vector3.zero;                                                                                                     
            Vector3 destination = new Vector3(position.x + offset.x, bounds.max.y + .3f, position.z + offset.z);

            StartCoroutine(_moveCamera.WallClimbAnimation(bounds.max.y + yOffset));
            StartCoroutine(TooltipSystem.TooltipHideBuffer(Time.deltaTime));

            position = destination;
            transform.position = position;
            
            TriggerClimbSound();
        }

        private static void TriggerClimbSound()
        {
            SoundManager.Sound[] climbSounds =
            {
                SoundManager.Sound.ClothSound01,
                SoundManager.Sound.ClothSound02,
                SoundManager.Sound.ClothSound03,
                SoundManager.Sound.ClothSound04
            };

            SoundManager.PlaySound(0.5f, "", climbSounds);
        }
    }
}