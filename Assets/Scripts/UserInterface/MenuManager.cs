using Audio;
using Cinemachine;
using UnityEngine;

namespace UserInterface
{
    public class MenuManager : MonoBehaviour
    {
        public CinemachineVirtualCamera frame1Cam;
        public CinemachineVirtualCamera frame2Cam;

        public GameObject[] frame;

        void Update()
        {
            SwitchFrames();
        }
        
        private void SwitchFrames()
        {
            if (Input.anyKeyDown && frame[0].activeInHierarchy)
            {
                frame[0].SetActive(false);
                frame[1].SetActive(true);
                frame1Cam.gameObject.SetActive(false);
                frame2Cam.gameObject.SetActive(true);
                
                SoundManager.PlaySound(SoundManager.Sound.MainMenuWoosh, 0.4f);
            }

            if (Input.GetKeyDown(KeyCode.Escape) && !frame[0].activeInHierarchy)
            {
                frame[1].SetActive(false);
                frame[2].SetActive(false);
                frame[0].SetActive(true);
                frame1Cam.gameObject.SetActive(true);
                frame2Cam.gameObject.SetActive(false);
                
                SoundManager.PlaySound(SoundManager.Sound.MainMenuWoosh, 0.4f);
            }
        }
        
        //Spaghetti Code! FeelsOkayMan
        public void ToggleOptionsMenu(bool isActive)
        {
            if (!isActive)
            {
                frame[0].SetActive(false);
                frame[1].SetActive(false);
                frame[2].SetActive(true);
            }
            if (isActive)
            {
                frame[1].SetActive(true);
                frame[2].SetActive(false);
            }
        }
    }
}
