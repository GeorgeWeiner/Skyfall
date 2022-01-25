using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    public class SpellSelection : MonoBehaviour
    {
        //Give the player a selection of different spells to choose from.
        //Stores these spells in a List<Spell>

        public List<GameObject> availableSpells = new List<GameObject>();
        public GameObject selectedSpell;
    
        private int _spellIndex = 0;
        private void Update()
        {
            ChangeSpells();
        }

        //Change through list of spells via tab.
        private void ChangeSpells()
        {
            if (!Input.GetKeyDown(KeyCode.Tab)) return;
        
            _spellIndex = Mathf.Clamp(_spellIndex, 0, availableSpells.Count - 1);
            _spellIndex++;
            if (_spellIndex > availableSpells.Count - 1)
            {
                _spellIndex = 0;
            }
            selectedSpell = availableSpells[_spellIndex];
        }
    }
}
