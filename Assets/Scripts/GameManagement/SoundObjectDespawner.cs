using UnityEngine;

namespace GameManagement
{
    public class SoundObjectDespawner : MonoBehaviour
    {
        public void DespawnSoundObject(float despawnTime){
            Destroy(gameObject, despawnTime);
        }
    }
}
