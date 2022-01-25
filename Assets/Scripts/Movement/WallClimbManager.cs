using GameManagement;
using UnityEngine;
using UserInterface;

namespace Movement
{
    public class WallClimbManager : MonoBehaviour
    {

        [SerializeField] private float wallDistance;
        [SerializeField] private float maxWallHeight;
        [SerializeField] private Collider capsuleCollider;
        [SerializeField] private LayerMask layerMask;
        
        public float animationSpeed;

        private IWallClimb _wallClimb;
        private RaycastHit _hit;

        private void Awake()
        {
            _wallClimb = GetComponent<IWallClimb>();
        }

        private void Update()
        {
            CanWallClimb();
        }

        private void CanWallClimb()
        {
            //Raycast forward, to check if there is a wall in front of oneself.
            var forward = capsuleCollider.transform.forward;
            Physics.Raycast(GameAssets.i.playerHead, forward, out _hit, wallDistance, layerMask);
            
            //Checks if the top of the wall is within reach...   
            if (_hit.collider != null)
            {
                if (_hit.collider.bounds.max.y < _hit.point.y + maxWallHeight && !GameAssets.i.isClimbingLadder)
                {
                    TooltipSystem.Show("press space");
                    _wallClimb.InitiateClimb(_hit, forward);
                }
            }

            //If there is no ladder in reach, hide the tooltip.
            if (_hit.collider == null && !LadderClimb.CanClimbLadder)
            {
                TooltipSystem.Hide();
            }
        }
    }
}
                                                                                                                                                    

 