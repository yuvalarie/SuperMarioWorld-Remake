using System;
using UnityEngine;

namespace Mario.PowerUps.FireMario
{
    /**
     * A class that represents a fireball in the game.
     */
    public class FireBall : MonoBehaviour, IPoolable
    {
        [SerializeField] private float speed = 10f;
        [SerializeField] private FireBallPool fireballPool;
        
        private Rigidbody2D _rb;
        
        private const float MaxLifetime = 1f;
        private float _lifetime;

        void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        private void OnEnable()
        { 
            if(_rb == null)
                _rb = GetComponent<Rigidbody2D>();
            _lifetime = 0f;
        }
        
        private void Update()
        {
            _lifetime += Time.deltaTime;
            if (_lifetime >= MaxLifetime)
            {
                ReturnToPool();
            }
        }

        public void Fire(Vector2 shootDirection)
        {
            const float angle = 45f; // Angle in degrees
            Vector2 velocity = Quaternion.Euler(0, -angle, 0) * shootDirection * speed;
            _rb.linearVelocity = velocity;
        }

        public void OnTriggerEnter2D(Collider2D other)
        {
            if (other.GetComponent<Collider2D>().CompareTag("Enemy"))
            {
                Destroy(other.gameObject);
                ReturnToPool();
            }
        }
        
        private void ReturnToPool()
        {
            FireBallPool.Instance.Return(this);
        }

        public void Reset()
        {
            _lifetime = 0f;
        }
    }
}
