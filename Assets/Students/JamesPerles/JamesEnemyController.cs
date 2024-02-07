using UnityEngine;

namespace Scenes.Scripts
{
    public class JamesEnemyController : MonoBehaviour
    {
        public float moveSpeed = 5f;            
        public float attackRange = 1f;          
        public float attackDelay = 2f;          
        public int damage = 1;                  

        private Transform _playerTransform;      
        private float _attackTimeRemaining;      
        private bool _isFollowing;              

        
        private void Start()
        {
            _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }

        
        private void Update()
        {
            if (_playerTransform != null)
            {
                
                float distanceToPlayer = Vector2.Distance(transform.position, _playerTransform.position);

                if (distanceToPlayer <= attackRange && _attackTimeRemaining <= 0f)
                {
                    
                    AttackPlayer();
                    _attackTimeRemaining = attackDelay;
                }
                else if (distanceToPlayer > attackRange && _isFollowing)
                {
                    
                    _isFollowing = false;
                }

                
                if (_isFollowing)
                {
                    MoveTowardsPlayer();
                }

                
                if (_attackTimeRemaining > 0f)
                {
                    _attackTimeRemaining -= Time.deltaTime;
                }
            }
        }
        
        private void MoveTowardsPlayer()
        {
            transform.position = Vector2.MoveTowards(transform.position, _playerTransform.position, moveSpeed * Time.deltaTime);
        }
        
        private void AttackPlayer()
        {
            

            
            _isFollowing = true;
        }
    }
}
