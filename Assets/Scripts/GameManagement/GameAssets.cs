using System;
using Audio;
using Movement;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameManagement
{
    
    //Central Asset Storage for the game. Not sure if this is good practice or not, please give feedback.
    public class GameAssets : MonoBehaviour
    {
        public static GameAssets _i;
        public static GameAssets i{
            get{
                if (_i == null){
                    _i = (Instantiate(Resources.Load("GameAssets")) as GameObject)?.GetComponent<GameAssets>();
                } 
                return _i; }
        }
    
        public SoundAudioClip[] soundAudioClips;

        [Serializable]
        public class SoundAudioClip{
            public SoundManager.Sound sound;
            public AudioClip audioClip;
        }
        //Determines if the player is in control or the game has taken control for him.


        #region Player States
        public bool isPlayerControlling;
        public bool isPlayerWalking;
        public bool isPlayerRunning;
        public bool isWallRunning;
        public bool isClimbingLadder;
        #endregion
    
        public Transform Orientation {get; set;}
        [FormerlySerializedAs("PlayerHead")] [HideInInspector] public Vector3 playerHead;
        [FormerlySerializedAs("PlayerCollider")] public Collider playerCollider;
    
        public float verticalMovement;
    

        private void Start() {
            SoundManager.Initialize();
            playerCollider = GameObject.Find("Player/PlayerCapsule").GetComponent<Collider>();
        }
        private void Update() {
            Orientation = FindObjectOfType<PlayerMovement>().orientation;
            verticalMovement = FindObjectOfType<PlayerMovement>().VerticalMovement;
            playerHead = playerCollider.bounds.center;
        }
    
    
    }
}

