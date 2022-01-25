using UnityEngine;

namespace Movement
{
    internal interface IWallClimb
    {
        void InitiateClimb(RaycastHit hit, Vector3 forward);
    }
}