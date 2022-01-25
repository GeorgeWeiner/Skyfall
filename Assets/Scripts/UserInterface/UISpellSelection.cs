using Combat;
using TMPro;
using UnityEngine;

namespace UserInterface
{
    public class UISpellSelection : MonoBehaviour
    {
        private SpellSelection _spellSelection;
        private TextMeshProUGUI _textField;
    
        private void Awake()
        {
            _spellSelection = GameObject.Find("Player").GetComponent<SpellSelection>();
            _textField = GameObject.Find("UI Canvas/SpellselectionBackground/SpellselectionText")
                .GetComponent<TextMeshProUGUI>();
            
            ChangeText();
        }

        private void Update()
        {
            ChangeText();
        }

        private void ChangeText()
        {
            _textField.text = _spellSelection.selectedSpell.gameObject.GetComponent<ProjectileSpellBehavior>().spellName;
        }
    }
}


