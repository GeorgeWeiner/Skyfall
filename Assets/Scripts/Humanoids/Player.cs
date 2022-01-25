using GameManagement;

namespace Humanoids
{
    public class Player : Humanoid
    {
        private SceneSwitcher _sceneSwitcher;
        public delegate void OnPlayerDeathEvent(string sceneName);
        public event OnPlayerDeathEvent OnPlayerDeath;
        
        private void Update()
        {
            Death();
        }
        private void Death()
        {
            if (!(health <= 0f)) return;
            OnPlayerDeath?.Invoke("CurrentScene");
        }
    }
}
