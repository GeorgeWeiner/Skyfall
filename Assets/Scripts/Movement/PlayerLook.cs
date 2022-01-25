using UnityEngine;

namespace Movement
{
    public class PlayerLook : MonoBehaviour
    {
        [SerializeField] private float sensX;
        [SerializeField] private float sensY;
        [SerializeField] private Transform cam;
        [SerializeField] private Transform orientation;
        [SerializeField] private float spawnRotationY;
        private WallRun _wallRun;
        private float _mouseX;
        private float _mouseY;
        private float _multiplier = 0.01f;
        private float _xRotation;
        private float _yRotation;

        private void Start() {
            _wallRun = GetComponent<WallRun>();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        
            _yRotation = spawnRotationY;
        }

        private void Update() {
            MouseInput();
            cam.transform.localRotation = Quaternion.Euler(_xRotation, _yRotation, _wallRun.Tilt);
            orientation.transform.localRotation = Quaternion.Euler(0f, _yRotation, 0f);
        }

        private void MouseInput(){
            _mouseX = Input.GetAxisRaw("Mouse X");
            _mouseY = Input.GetAxisRaw("Mouse Y");

            _yRotation += _mouseX * sensX * _multiplier;
            _xRotation -= _mouseY * sensY * _multiplier;

            _xRotation = Mathf.Clamp(_xRotation, -90, 90);
        }
    }
}
