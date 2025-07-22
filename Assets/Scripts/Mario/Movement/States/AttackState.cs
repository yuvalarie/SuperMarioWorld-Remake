using DG.Tweening;
using UnityEngine;

namespace Mario.Movement.States
{
    /**
     * The state where the player is attacking in 'super' mode.
     */
    public class AttackState : IPlayerState
    {
        private bool _hasAttacked = false;
        private float _attackDuration = 0.5f;
        private float _maxRange = 4f;
        private float _duration = 1f;
        private Transform _rippleEffect;

        
        public void EnterState(PlayerController player)
        {
            if (_rippleEffect == null)
                _rippleEffect = player.transform.Find("RippleEffect"); 
            
            _hasAttacked = false;
            _attackDuration = 0.5f;
            
            if (!_hasAttacked) // Ensures the attack happens only once
            {
                _rippleEffect.localScale = Vector3.zero; 
                _rippleEffect.DOScale(new Vector3(1f, 1f, 1f), _duration)
                    .SetEase(Ease.OutCirc)
                    .OnComplete(() => _rippleEffect.localScale = Vector3.zero);
                
                // Cast rays to both sides to detect enemies
                RaycastHit2D[] hitEnemiesLeft = Physics2D.RaycastAll(player.transform.position, Vector2.left, _maxRange);
                RaycastHit2D[] hitEnemiesRight = Physics2D.RaycastAll(player.transform.position, Vector2.right, _maxRange);

                // Loop through the hits on the left
                foreach (var hit in hitEnemiesLeft)
                {
                    if (hit.collider.CompareTag("Enemy"))
                    {
                        GameObject enemy = hit.collider.gameObject;
                        GameObject.Destroy(enemy);
                    }
                }

                // Loop through the hits on the right
                foreach (var hit in hitEnemiesRight)
                {
                    if (hit.collider.CompareTag("Enemy"))
                    {
                        GameObject enemy = hit.collider.gameObject;
                        GameObject.Destroy(enemy);
                    }
                }

                // Mark the attack as complete
                _hasAttacked = true;
            }
        }

        public void UpdateState(PlayerController player)
        {
            _attackDuration -= Time.deltaTime;
            if (_attackDuration <= 0)
            {
                player.SetState(new IdleState());
            }
        }

        public void ExitState(PlayerController player)
        {
        }
    }
}