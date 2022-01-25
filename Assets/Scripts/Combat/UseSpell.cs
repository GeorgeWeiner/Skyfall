using Humanoids;
using UnityEngine;

namespace Combat
{
    public class UseSpell : MonoBehaviour
    {
        private SpellSelection _spellSelection;
        private Transform _instantiationLocation;
        private Humanoid _humanoid;
        private float _timeToFire;

        private IUseSpell _useSpell;

        private void Start()
        {
            _spellSelection = GetComponent<SpellSelection>();
            _humanoid = GetComponent<Humanoid>();
            _instantiationLocation = _humanoid.instantiationLocation;
            
            //Liskov Substitution Principle.
            _useSpell = GetComponent<IUseSpell>();
        }
        private void Update()
        {
            Use();
        }
        private void Use()
        {
            if (!Input.GetMouseButton(0)) return;

            //Checks if the player can fire again by adding value to Time.time.
            ProjectileSpellBehavior projectileSpellBehavior = _spellSelection.selectedSpell.GetComponent<ProjectileSpellBehavior>();
            if (Time.time >= _timeToFire)
            {
                _timeToFire = Time.time + 1 / projectileSpellBehavior.rateOfFire;
                _useSpell.CastSpell(this, _instantiationLocation);
            }
        }
    }
}
