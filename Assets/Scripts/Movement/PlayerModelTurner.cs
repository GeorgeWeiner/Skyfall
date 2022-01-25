using UnityEngine;

namespace Movement
{
    public class PlayerModelTurner : MonoBehaviour
    {
        public Transform targetRotation;

        //Just sets the Y-Rotation of the Playermodel equal to the "CameraHolders" Y-Rotation.
        void Update()
        {
            var eulerRotation = new Vector3(0f, targetRotation.localEulerAngles.y, 0f);
            transform.rotation = Quaternion.Euler(eulerRotation);
        }
    }
}
