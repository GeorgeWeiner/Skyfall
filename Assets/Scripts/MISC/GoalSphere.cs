using GameManagement;
using UnityEngine;

namespace MISC
{
    public class GoalSphere : MonoBehaviour
    {
        private SceneSwitcher _sceneSwitcher;

        private void Start() {
            _sceneSwitcher = GameObject.Find("MenuManager").GetComponent<SceneSwitcher>();
        }
        private void OnTriggerEnter(Collider other) {
            if (other.CompareTag("Player")){
                _sceneSwitcher.InitializeSceneSwitch("VictoryScene");
            }
        }
    }
}
