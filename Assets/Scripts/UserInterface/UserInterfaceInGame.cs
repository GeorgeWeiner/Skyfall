using Humanoids;
using UnityEngine;
using UnityEngine.UI;

namespace UserInterface
{
    public class UserInterfaceInGame : MonoBehaviour
    {
        private Player _player;
        private Slider _healthSlider;
    
        void Start()
        {
            _player = GameObject.Find("Player").GetComponent<Player>();
            _healthSlider = GameObject.Find("UI Canvas/Health_Bar").GetComponent<Slider>();
            _healthSlider.maxValue = _player.health;
        }
        void Update()
        {
            ChangeHealthBarValue();
        }
        void ChangeHealthBarValue()
        {
            _healthSlider.value = _player.health;
        }
    }
}
