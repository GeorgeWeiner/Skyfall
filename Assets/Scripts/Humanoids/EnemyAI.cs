using System;
using System.Collections;
using Combat;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Humanoids
{
    public class EnemyAI : MonoBehaviour
    {
        private Npc _npc;
        private SpellSelection _spellSelection;
        private NavMeshAgent _agent;
        private Transform _player;
        private float _maxHealth;

        [SerializeField]
        private float enemySightRadius, enemyAttackRadius, enemyAttackInterval, enemyAllyAlertRange;
        [SerializeField] LayerMask playerMask;
        private bool _playerInSightRange, _playerInAttackRange, _attackCoroutineRunning;
        

        private void Start()
        {
            _npc = GetComponent<Npc>();
            _spellSelection = GetComponent<SpellSelection>();
            _agent = GetComponent<NavMeshAgent>();
            _player = GameObject.Find("Player").transform;

            _maxHealth = _npc.health;
        }

        private void Update()
        {
            var position = transform.position;
            _playerInSightRange = Physics.CheckSphere(position, enemySightRadius, playerMask);
            _playerInAttackRange = Physics.CheckSphere(position, enemyAttackRadius, playerMask);

            EnemyStateController();

            //Debug.LogFormat("Player in Sight Range {0}, Player in Attack Range{1}, Patrolling{2}", playerInSightRange, playerInAttackRange, isPatrolling);
        }

        private void EnemyStateController()
        {
            if (_playerInSightRange && _playerInAttackRange)
            {
                Attack();
            }
            if (_playerInSightRange && !_playerInAttackRange)
            {
                Chase();
            }
            if (!_playerInSightRange && !_playerInAttackRange)
            {
                OnPlayerAttacking();
            }
        }

        //Walks Towards and Looks at the player. Triggers the Shoot Projectile coroutine if in Range.
        private void Attack()
        {
            var position = transform.position;
            _agent.SetDestination(position);
        
            var playerPosition = _player.position;
            transform.LookAt(new Vector3(playerPosition.x, position.y, playerPosition.z));
        
            if (!_attackCoroutineRunning)
            {
                StartCoroutine(ShootProjectile());
                _attackCoroutineRunning = true;
            }
        }

        private void Chase()
        {
            _agent.SetDestination(_player.transform.position);
        }

        //Shoots projectile in forward direction (LookAt(Player)), in intervals specified by enemyAttackInterval.
        private IEnumerator ShootProjectile()
        {
            while (_playerInAttackRange)
            {   
                GameObject spell = _spellSelection.availableSpells[Random.Range(0, _spellSelection.availableSpells.Count - 1)];
                Instantiate(spell, _npc.instantiationLocation.position, _npc.instantiationLocation.rotation);
            
                yield return new WaitForSeconds(enemyAttackInterval);
            }
            _attackCoroutineRunning = false;
        }

        //If the player attacks the enemy, it will be aggroed, regardless of range.
        private void OnPlayerAttacking(){
            if (Math.Abs(_npc.health - _maxHealth) > .5f){
                _agent.SetDestination(_player.transform.position);

                //Alert other enemies in a range. (Broken)

                RaycastHit[] alertedAllies = Physics.SphereCastAll(transform.position, enemyAllyAlertRange, Vector3.zero, 0f);
            
                foreach(RaycastHit ally in alertedAllies){
                    if (ally.collider.CompareTag("NPC")){
                        ally.collider.GetComponent<EnemyAI>().OnPlayerAttacking();
                    }
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            var position = transform.position;
            Gizmos.DrawWireSphere(position, enemySightRadius);
            Gizmos.DrawWireSphere(position, enemyAttackRadius);
        }
    }
}
