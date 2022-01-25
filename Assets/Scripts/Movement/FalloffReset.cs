using UnityEngine;

namespace Movement
{
    public class FalloffReset : MonoBehaviour
    {
        private GameObject _player;

        private void Awake()
        {
            _player = GameObject.Find("Player");
        }

        private void Update()
        {
            ResetPlayerOnFallOff();
        }

        
        public delegate void OnPlayerFalloffEvent(string scene);
        public event OnPlayerFalloffEvent OnPlayerFalloff;
        
        //How is this even being called? The player is at Y -50????
        private void ResetPlayerOnFallOff()
        {
            if (!(_player.transform.position.y <= -5f)) return;
            OnPlayerFalloff?.Invoke("CurrentScene");
        }
    }
}