using UnityEngine;

namespace Combat
{
    public class ProjectileLauncher : MonoBehaviour, IUseSpell
    {
        private SpellSelection _spellSelection;
        private void Awake()
        {
            _spellSelection = GetComponent<SpellSelection>();
        }

        public void CastSpell(UseSpell useSpell, Transform instantiationLocation)
        {
            Instantiate(_spellSelection.selectedSpell, instantiationLocation.position, instantiationLocation.rotation);
        }
    }
}
