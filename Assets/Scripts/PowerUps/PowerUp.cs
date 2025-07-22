using Mario.Movement;
using Mario.Player;
using UnityEngine;

namespace PowerUps
{
    /**
     * A power-up that can be collected by the player.
     */
    public class PowerUp : MonoBehaviour
    {
        [SerializeField] private MarioState powerUpType;
        
        private Collider2D _triggerCollider;
        private Collider2D _groundCollider;
        private Rigidbody2D _rigidbody;
        
        private bool _isGrounded = false;

        private void Start()
        {
            var colliders = GetComponents<Collider2D>();
            foreach (var collider in colliders)
            {
                if (collider.isTrigger)
                    _triggerCollider = collider;
                else
                    _groundCollider = collider;
            }

            _rigidbody = gameObject.GetComponent<Rigidbody2D>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                var player = collision.GetComponent<MarioPresenter>();
                if (player == null)
                {
                    return;
                }

                if (player == null) return;
                var playerController = player.GetComponent<PlayerController>();
                if (playerController == null)
                {
                    return;
                }

                if (playerController == null) return;
                playerController.ApplyPowerUp(powerUpType);
                Destroy(gameObject); // Destroy power-up after use
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Ground") && !_isGrounded)
            {
                if (_rigidbody != null)
                {
                    _rigidbody.bodyType = RigidbodyType2D.Static;
                }

                if (_groundCollider != null)
                {
                    _groundCollider.enabled = false;
                }
                _isGrounded = true;
            }
        }
    }
}
