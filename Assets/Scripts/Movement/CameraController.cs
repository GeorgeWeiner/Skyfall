using System.Collections;
using GameManagement;
using UnityEngine;

namespace Movement
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Transform cameraPosition;
        [SerializeField] private float fovTransitionSpeed;
        [SerializeField] private Camera cam;
        private const float RegularFov = 85f;

        private WallClimbManager _wallManager;
    
        private void Start() {
            _wallManager = GameObject.Find("Player").GetComponent<WallClimbManager>();
            GameAssets.i.isPlayerControlling = true;
        }
        private void Update() {
            if (GameAssets.i.isPlayerControlling || !GameAssets.i.isPlayerControlling && GameAssets.i.isClimbingLadder){
                transform.position = cameraPosition.position;
            }
        }

    
        public void SetCameraFov(float fov = RegularFov){
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, fov, Time.deltaTime * fovTransitionSpeed);
        }
    
        public IEnumerator WallClimbAnimation(float yPosition){
            var t = 0f;
            GameAssets.i.isPlayerControlling = false;
        
            while (t < 1f){
                t += _wallManager.animationSpeed * Time.deltaTime;
            
                var position = transform.position;
                transform.position = Vector3.Lerp(position, new Vector3(position.x, yPosition, position.z), t);
            
                yield return new WaitForSeconds(Time.deltaTime);
            }
        
            t = 0f;

            while (t < 1f){
            
                t += _wallManager.animationSpeed * Time.deltaTime;
            
                transform.position = Vector3.Lerp(transform.position, cameraPosition.position, t);
                yield return new WaitForSeconds(Time.deltaTime);
            }

            GameAssets.i.isPlayerControlling = true;
            yield return null;
        }
    }
}
