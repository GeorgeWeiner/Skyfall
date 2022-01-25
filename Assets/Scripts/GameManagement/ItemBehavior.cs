using Audio;
using Humanoids;
using UnityEngine;

namespace GameManagement
{
    public class ItemBehavior : MonoBehaviour
    {
        [SerializeField] private float healthEffectAmount;

        private void OnCollisionEnter(Collision other) {
            if (other.gameObject.CompareTag("Player")){
            
                SoundManager.PlaySound(SoundManager.Sound.HealthPotionPickup, transform.position);
                Player player = other.gameObject.GetComponent<Player>();
                player.health += healthEffectAmount;
            
                Destroy(gameObject);
            }
        }
    }
}
