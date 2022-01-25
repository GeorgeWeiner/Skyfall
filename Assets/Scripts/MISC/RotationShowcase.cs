using UnityEngine;

// Transform.Rotate example
//
// This script creates two different cubes: one red which is rotated using Space.Self; one green which is rotated using Space.World.
// Add it onto any GameObject in a scene and hit play to see it run. The rotation is controlled using xAngle, yAngle and zAngle, modifiable on the inspector.

namespace MISC
{
    public class RotationShowcase : MonoBehaviour
    {
        public float x, y, z;
        private void Update()
        {
            gameObject.transform.Rotate(new Vector3(x, y, z) * Time.deltaTime);
        }
    }
}
