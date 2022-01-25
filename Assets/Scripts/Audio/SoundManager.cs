using System.Collections.Generic;
using GameManagement;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Audio
{
    public static class SoundManager 
    {
        public enum Sound{
            PlayerRunning,
            PlayerJumping,
            PlayerWallJumping,
            PlayerClimbing,
            ShootProjectileFire,
            ShootProjectileIce,
            ShootProjectileVoid,
            ImpactProjectileFire,
            ImpactProjectileIce,
            ImpactProjectileVoid,
            HealthPotionPickup,
            MainMenuWoosh,
            ClothSound01,
            ClothSound02,
            ClothSound03,
            ClothSound04,
            RunSound01,
            RunSound02,
            RunSound03,
            RunSound04,
            RunSound05,
            RunSound06,
            RunSound07,
            RunSound08,
            RunSound09
        }
        private static Dictionary<Sound, float> _soundTimerDictionary;
        private static Dictionary<Sound[], float> _soundArrayTimerDictionary;

        public static void Initialize(){
            _soundTimerDictionary = new Dictionary<Sound, float>();
            _soundArrayTimerDictionary  = new Dictionary<Sound[], float>();
        
            _soundTimerDictionary[Sound.PlayerRunning] = 0f;

        }
    
        public static void PlaySound(Sound sound){
            if (CanPlaySound(sound)){
            

                GameObject soundGameObject = new GameObject("Sound");
                AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
                SoundObjectDespawner despawner = soundGameObject.AddComponent<SoundObjectDespawner>();
                despawner.DespawnSoundObject(GetAudioClip(sound).length);
            
                audioSource.pitch = Random.Range(0.9f, 1.1f);
                audioSource.PlayOneShot(GetAudioClip(sound));
            }
        }
        //plays static sound without directional properties.

        //Plays sound with directional properties.
        public static void PlaySound(Sound sound, Vector3 position){
            if (CanPlaySound(sound)){
            
                GameObject soundGameObject = new GameObject("Sound");
                soundGameObject.transform.position = position;
                AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();

                SoundObjectDespawner despawner = soundGameObject.AddComponent<SoundObjectDespawner>();
                despawner.DespawnSoundObject(GetAudioClip(sound).length);
            
                audioSource.pitch = Random.Range(0.9f, 1.1f);
                audioSource.clip = GetAudioClip(sound);

                audioSource.Play(); 
            }
        }
    
        //Volume property
        public static void PlaySound(Sound sound, float volume){
            if (CanPlaySound(sound)){
            
                GameObject soundGameObject = new GameObject("Sound");
                AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();

                SoundObjectDespawner despawner = soundGameObject.AddComponent<SoundObjectDespawner>();
                despawner.DespawnSoundObject(GetAudioClip(sound).length);
            
                audioSource.pitch = Random.Range(0.9f, 1.1f);
                audioSource.volume = volume;
                audioSource.PlayOneShot(GetAudioClip(sound));
            }
        }
    
        //Volume and Positional Property
        public static void PlaySound(Sound sound, Vector3 position, float volume){
            if (CanPlaySound(sound)){
            
                GameObject soundGameObject = new GameObject("Sound");
                soundGameObject.transform.position = position;
                AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();

                SoundObjectDespawner despawner = soundGameObject.AddComponent<SoundObjectDespawner>();
                despawner.DespawnSoundObject(GetAudioClip(sound).length);
            
                audioSource.pitch = Random.Range(0.9f, 1.1f);
                audioSource.volume = volume;
                audioSource.PlayOneShot(GetAudioClip(sound));
            }
        }
    
        //Add variety to sounds by choosing a Random sound each time.
        public static void PlaySound(float volume, string soundArrayName = "", params Sound[] sounds)
        {
            Sound sound = sounds[Random.Range(0, sounds.Length)];
            if (CanPlaySound(soundArrayName, sounds)){
                GameObject soundGameObject = new GameObject("Sound");
                AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();

                SoundObjectDespawner despawner = soundGameObject.AddComponent<SoundObjectDespawner>();
                despawner.DespawnSoundObject(GetAudioClip(sound).length);
            
                audioSource.pitch = Random.Range(0.9f, 1.1f);
                audioSource.volume = volume;
                audioSource.PlayOneShot(GetAudioClip(sound));
            }
        }
    
        //Variety of sound, including the control of the replay time for walking / running for example
        public static void PlaySound(float volume, float pitch, float duration, string soundArrayName = "", params Sound[] sounds)
        {
            Sound sound = sounds[Random.Range(0, sounds.Length)];
            if (CanPlaySound(duration, soundArrayName, sounds)){
                GameObject soundGameObject = new GameObject("Sound");
                AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();

                SoundObjectDespawner despawner = soundGameObject.AddComponent<SoundObjectDespawner>();
                despawner.DespawnSoundObject(GetAudioClip(sound).length);

                audioSource.pitch = pitch;
                audioSource.volume = volume;
                audioSource.PlayOneShot(GetAudioClip(sound));
            }
        }

        //Checks if the sound can be played again.
        private static bool CanPlaySound(Sound sound){
            switch(sound){
                case Sound.PlayerRunning:
                    if (_soundTimerDictionary.ContainsKey(sound)){
                
                        float lastTimePlayed = _soundTimerDictionary[sound];
                        float playerMoveTimerMax = GetAudioClip(Sound.PlayerRunning).length;
                
                        if (lastTimePlayed + playerMoveTimerMax < Time.time){
                            _soundTimerDictionary[sound] = Time.time;
                            return true;
                        }
                        return false;
                    }
                    else return true;
            
                default:
                    return true;
            }
        }
    
        private static bool CanPlaySound(string soundArrayName = "", params Sound[]sounds)
        {
            Sound sound = sounds[Random.Range(0, sounds.Length)];
            switch(soundArrayName){
                case "runSounds":
                    if (_soundTimerDictionary.ContainsKey(sound))
                    {

                        float lastTimePlayed = 0f; 
                        float playerMoveTimerMax = GetAudioClip(Sound.PlayerRunning).length;
                
                        if (lastTimePlayed + playerMoveTimerMax < Time.time){
                            _soundTimerDictionary[sound] = Time.time;
                            return true;
                        }
                        return false;
                    }
                    else return true;
            
                default:
                    return true;
            }
        }
    
        private static bool CanPlaySound(float duration, string soundArrayName = "", params Sound[] sounds)
        {
            switch(soundArrayName){
                case "runSounds":
                    if (_soundArrayTimerDictionary.ContainsKey(sounds))
                    {
                        float lastTimePlayed = _soundArrayTimerDictionary[sounds];
                        float playerMoveTimerMax = duration;
                
                        if (lastTimePlayed + playerMoveTimerMax < Time.time){
                            _soundArrayTimerDictionary[sounds] = Time.time;
                            return true;
                        }
                        return false;
                    }
                    else return true;
            
                default:
                    return true;
            }
        }

        //Loops through all audio clips, and returns the correct one for the instance.
        private static AudioClip GetAudioClip(Sound sound){
            foreach(GameAssets.SoundAudioClip soundAudioClip in GameAssets.i.soundAudioClips){
                if (soundAudioClip.sound == sound){
                    return soundAudioClip.audioClip;
                }
            }
            return null;
        }
    }
}
