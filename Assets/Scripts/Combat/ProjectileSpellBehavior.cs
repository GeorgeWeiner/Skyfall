using _3rdPartyAssets.Plugins.Pixelplacement.iTween;
using Audio;
using Humanoids;
using UnityEngine;

namespace Combat
{
    public class ProjectileSpellBehavior : MonoBehaviour, ISpellBehavior
    {
        [Header("Core")]
        private Rigidbody _spellRigidbody;
        public string spellName;
        [Range(0, 5)] [SerializeField] private float timeUntilDespawn, timeUntilImpactDespawn;
        [Range(0, 10)] public float rateOfFire;
        [Range(0, 100)] [SerializeField] private float sDamage;
        [Range(0, 100)] [SerializeField] float projectileSpeed;

        [Header("Audio-Visual")]
        [Range(0, 5)] [SerializeField] private float arcRange;
        [Range(0, 5)] [SerializeField] private float arcTimeMin, arcTimeMax;
        public GameObject impactObject;
        [SerializeField] SoundManager.Sound soundEffectSpawn, soundEffectImpact;
        private GameObject _player;
    
        private void Start()
        {
            InitializeSpellValues();
            OnSpawn();
        }
    
        private void Update()
        {
            Despawn();
        }

        public void InitializeSpellValues()
        {
            _spellRigidbody = GetComponent<Rigidbody>();
            _player = GameObject.Find("Player");
        }

        //When the prefab is Instantiated, (UseSpell.cs), it will automatically immediately add a force to itself on the forward axis.
        private void OnSpawn()
        {
            SoundManager.PlaySound(soundEffectSpawn, transform.position);
        
            _spellRigidbody.AddForce(transform.forward * projectileSpeed, ForceMode.Impulse);

            iTween.PunchPosition(gameObject, new Vector3(Random.Range(-arcRange, arcRange), Random.Range(-arcRange, arcRange), 0f), 
                Random.Range(arcTimeMin, arcTimeMax));
        }

        //After a while the Instance of the Object will despawn.
        private void Despawn()
        {
            timeUntilDespawn -= Time.deltaTime;
            if (timeUntilDespawn <= 0f)
            {
                Destroy(gameObject);
            }
        }

        //It will also despawn if it collided with another collider, if the collider is a NPC, it removes the Damage Value of the spell from the health of said NPC.
        private void OnTriggerEnter(Collider other)
        {
            var position = transform.position;
            GameObject impact = Instantiate(impactObject, other.ClosestPoint(position), Quaternion.identity);

            SoundManager.Sound soundEffect = soundEffectImpact;
            SoundManager.PlaySound(soundEffect, position);
        
            Destroy (impact, timeUntilImpactDespawn);
        
            if (other.gameObject.CompareTag("NPC"))
            {
                var hitNpc = other.gameObject.GetComponent<Npc>();
                hitNpc.health -= sDamage;
            }
            if (other.gameObject.CompareTag("Player"))
            {
                var hitPlayer = _player.GetComponent<Player>();
                hitPlayer.health -= sDamage;
            }
            if (!other.gameObject.CompareTag("Player")){
                Destroy(gameObject);
            }
        }
    }
}
