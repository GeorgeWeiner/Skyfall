using UnityEngine;

namespace Combat
{
    public class ProjectileImpact : MonoBehaviour, IProjectileImpact
    {
        private GameObject _impactObject;
        private void Start()
        {
            var projectileSpellBehavior = GetComponent<ProjectileSpellBehavior>();
            _impactObject = projectileSpellBehavior.impactObject;
        }

        public void ImpactEffect(ProjectileSpellBehavior spellBehavior)
        {
            var impact = Instantiate(_impactObject, transform.position, Quaternion.identity);
        }

        public void ImpactCausality()
        {
            
        }
    }
}