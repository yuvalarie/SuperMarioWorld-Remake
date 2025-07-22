using System.Collections;
using Mario.Player;
using Sound;
using UnityEngine;

namespace Enemies
{
    public enum EnemyType
    {
        Dinosaur,
        Black
    }
    
    /**
     * This class is responsible for the behavior of the enemy.
     */
    public class EnemyBehavior : MonoBehaviour
    {
        private static readonly int IsHurt = Animator.StringToHash("IsHurt");
        
        [SerializeField] private float speed = 1f;
        [SerializeField] private EnemyType enemyType;
        [SerializeField] private int walkDuration;
        [SerializeField] private float damageCooldown = 1.0f;
        
        private Rigidbody2D _rb;
        private Animator _animator;
        private SpriteRenderer _spriteRenderer;
        
        private bool _isHurt = false;
        private bool _isFacingRight = false;
        private bool _canTakeDamage;
    
        void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            StandingCollider = GetComponent<CapsuleCollider2D>();
            CrouchingCollider = GetComponent<BoxCollider2D>();
            CrouchingCollider.enabled = false;
            StartCoroutine(FlipDirectionRoutine());
            _canTakeDamage = true;
        }

        private void FixedUpdate()
        {
            float direction = _isFacingRight ? 1f : -1f;
            _rb.linearVelocity = new Vector2(direction * speed, _rb.linearVelocity.y);        
        }
        
        private IEnumerator FlipDirectionRoutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(walkDuration);
                FlipDirection();
            }
        }

        private void FlipDirection()
        {
            _isFacingRight = !_isFacingRight; // Toggle direction
            if (enemyType == EnemyType.Black)
            {
                _spriteRenderer.flipX = !_isFacingRight;
            }
            else
            {
                _spriteRenderer.flipX = _isFacingRight; // Flip sprite            
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                
                var player = collision.gameObject.GetComponent<MarioPresenter>();
                if (player != null)
                {
                        // Check if the player is coming from above
                        var playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
                        if (playerRb != null && playerRb.linearVelocity.y < 0 &&
                            collision.bounds.min.y > _rb.GetComponent<Collider2D>().bounds.max.y - 0.1f)
                        {
                            HandlePlayerBounce(playerRb);
                            MarioPresenter marioPresenter = player.GetComponent<MarioPresenter>();
                            marioPresenter.AddPoints(200);
                            if (enemyType == EnemyType.Dinosaur)
                            {
                                if (_isHurt)
                                {
                                    Destroy(gameObject);
                                }
                                else
                                {
                                    _animator.SetTrigger(IsHurt);
                                    _isHurt = true;
                                    StandingCollider.enabled = false;
                                    CrouchingCollider.enabled = true;
                                }
                            }
                            else if (enemyType == EnemyType.Black)
                            {
                                Destroy(gameObject);
                            }
                            SoundManager.Instance.PlaySfx(SoundType.StompEnemy);
                        }
                        else
                        {
                            if (_canTakeDamage && !player.IsDead())
                            {
                                // Hurt the player if not jumped from above
                                player.TakeDamage();
                                StartCoroutine(ResetDamageCooldown());
                            }
                        }
                }
            }
        }
        
        // Coroutine to reset the damage cooldown
        private IEnumerator ResetDamageCooldown()
        {
            _canTakeDamage = false; // Disable taking damage
            yield return new WaitForSeconds(damageCooldown); // Wait for the cooldown duration
            _canTakeDamage = true; // Re-enable taking damage
        }
        
        private void HandlePlayerBounce(Rigidbody2D playerRb)
        {
            playerRb.AddForce(Vector2.up * 80f, ForceMode2D.Impulse);
        }
        
        public void ResetEnemyState()
        {
            if (enemyType == EnemyType.Dinosaur)
            {
                _isHurt = false;
                _animator.ResetTrigger(IsHurt);
            }
            StandingCollider.enabled = true;
            CrouchingCollider.enabled = false;
        }

        private CapsuleCollider2D StandingCollider { get; set; }

        private BoxCollider2D CrouchingCollider { get; set; }
        
        public EnemyType EnemyType { get; set; }
    }
}
