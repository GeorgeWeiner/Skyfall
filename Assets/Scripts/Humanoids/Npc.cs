namespace Humanoids
{
    public class Npc : Humanoid
    {
        private void Update()
        {
            if (!(health <= 0f)) return;
            Destroy(gameObject);
        }
    }
}