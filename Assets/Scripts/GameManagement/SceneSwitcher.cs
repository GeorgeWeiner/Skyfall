using System.Collections;
using Humanoids;
using Movement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameManagement
{
    public class SceneSwitcher : MonoBehaviour
    {
        private Animator _animator;
        private AudioSource _music, _wind;
        [SerializeField]
        private float musicLerpSpeed, musicLerpDuration;
        
        private void Awake() {
            if (GameObject.Find("UI Canvas/LevelChanger") != null)
                _animator = GameObject.Find("UI Canvas/LevelChanger").GetComponent<Animator>();
            if (GameObject.Find("MusicPlayer") != null)
                _music = GameObject.Find("MusicPlayer").GetComponent<AudioSource>();
            if (GameObject.Find("Wind_SFX") != null)
                _wind = GameObject.Find("Wind_SFX").GetComponent<AudioSource>();

            var player = FindObjectOfType<Player>();
            if (player != null) player.OnPlayerDeath += InitializeSceneSwitch;
            var falloff = FindObjectOfType<FalloffReset>();
            if (falloff != null)falloff.OnPlayerFalloff += InitializeSceneSwitch;
        } 
        private void Update() {
            SwitchToMainMenu();
        }
        
        //Probably will make this static.
        public void InitializeSceneSwitch(string scene = "CurrentScene"){
            StartCoroutine(SwitchScene(scene));
        }

        private float _timer;
        
        //Linearly Interpolates Music and SFX volume, triggers Fade Animation and after time has passed Switches Scene.
        private IEnumerator SwitchScene(string scene){
            if (_animator != null) _animator.SetTrigger(SceneSwitching);
            while (_timer <= musicLerpDuration){
                _timer += Time.deltaTime;

                Debug.LogFormat("Timer: {0} Time: {1}", _timer, Time.time);
            
                if (_music != null)
                     _music.volume = Mathf.Lerp(_music.volume, 0f, t);
                if (_wind != null)
                    _wind.volume = Mathf.Lerp(_wind.volume, 0f, t);
                
                t += musicLerpSpeed * Time.deltaTime;
            
                yield return new WaitForSeconds(Time.deltaTime);
            }
            _timer = 0f;
            SceneManager.LoadScene(scene == "CurrentScene" ? SceneManager.GetActiveScene().name : scene);
        }

        public float t;
        private static readonly int SceneSwitching = Animator.StringToHash("SceneSwitching");

        public void ExitGame(){
            Application.Quit();
            print ("Exiting Game.");
        }
        
        private void SwitchToMainMenu(){
            if (Input.GetKeyDown(KeyCode.Escape) && SceneManager.GetActiveScene() != SceneManager.GetSceneByName("MainMenu")){
                SceneManager.LoadScene("MainMenu");
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }
}
