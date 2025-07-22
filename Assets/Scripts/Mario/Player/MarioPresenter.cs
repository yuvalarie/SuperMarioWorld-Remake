using System.Collections;
using Game;
using Mario.PowerUps;
using PowerUps;
using Sound;
using UnityEngine;

namespace Mario.Player
{
    /**
     * The presenter class for Mario.
     */
    public class MarioPresenter : MonoBehaviour
    {
        private static readonly int IsDeath = Animator.StringToHash("IsDeath");
        private static readonly int IsFalling = Animator.StringToHash("IsFalling");
        [SerializeField] private int initialLives = 5;
        [SerializeField] private int initialTime = 300;
        
        private int _coins = 0;
        private float _timeRemaining;
        private bool _isLevelComplete;
        private bool _musicSpeedAdjusted = false;
        private bool _isDead;

        public MarioModel MarioModel;
        private MarioView _marioView;
        private Animator _animator;
        
        // Serialized fields for AnimatorControllers
        [SerializeField] private RuntimeAnimatorController regularMarioController;
        [SerializeField] private RuntimeAnimatorController superMarioController;
        [SerializeField] private RuntimeAnimatorController fireMarioController;

        private void Start()
        {
            _timeRemaining = initialTime;
            _isDead = false;
            MarioModel = new MarioModel();
            
            _animator = GetComponent<Animator>();
            UpdateAnimations();
            
            _marioView = GetComponent<MarioView>();
            _marioView.UpdateHealth(GameManager.Instance.GetMarioLives());
            _marioView.UpdateCoins(_coins);
            _marioView.UpdateTimer((int)_timeRemaining);
        }
        
        private void Update()
        {
            if (_isLevelComplete) return;
            
            _timeRemaining -= Time.deltaTime;
            _marioView.UpdateTimer((int)_timeRemaining);
            if((int) _timeRemaining <= 150 && !_musicSpeedAdjusted)
            {
                SoundManager.Instance.AdjustMusicSpeed(1.2f);
                _musicSpeedAdjusted = true;
            }

            if (_timeRemaining < 0)
            {
                TimeOver();
            }
        }
        public void AddPoints(int amount)
        {
            _coins += amount;
            _marioView.UpdateCoins(_coins);
        }

        public void TakeDamage()
        {
            bool isDead = MarioModel.TakeDamage();
            UpdateAnimations();
            if (isDead)
            {
                Die();
            }
            else
            {
                SoundManager.Instance.PlaySfx(SoundType.LosePowerUp);
            }
        }

        public void TimeOver()
        {
            GameManager.Instance.DecreaseMarioLives();
            GameManager.Instance.LoadTimeOverScene();
        }


        private void Die()
        {
            if (_timeRemaining > 0)
            {
                _animator.SetTrigger(IsDeath);
                SoundManager.Instance.PlayDeathMusic();
            }
            _isDead = true;
            GameManager.Instance.DecreaseMarioLives();
            int lives = GameManager.Instance.GetMarioLives();
            _marioView.UpdateHealth(lives);
            if (lives <= 0)
            {
                StartCoroutine(WaitThenLoadGameOverScene());
            }
            else
            {

                StartCoroutine(WaitThenLoadMapScene());
            }
        }
        public void Fall()
        {
            _animator.SetTrigger(IsFalling);
            Die();
        }
        private IEnumerator WaitThenLoadGameOverScene()
        {
            yield return new WaitForSeconds(4f);
            GameManager.Instance.LoadGameOverScene();
        }
        
        private IEnumerator WaitThenLoadMapScene()
        {
            yield return new WaitForSeconds(4f);
            GameManager.Instance.LoadMapScene();
        }
        
        public void ApplyPowerUps(MarioState powerUp)
        {
            if (PowerUpManager.ApplyPowerUp(this, powerUp))
            {
                MarioModel.ApplyPowerUp(powerUp);
                UpdateAnimations();
            }
        }
        
        private void UpdateAnimations()
        {
            switch (MarioModel.State)
            {
                case MarioState.Regular:
                    _animator.runtimeAnimatorController = regularMarioController;
                    break;
                case MarioState.Super:
                    _animator.runtimeAnimatorController = superMarioController;
                    break;
                case MarioState.Fire:
                    _animator.runtimeAnimatorController = fireMarioController;
                    break;
            }
        }
        public void HandleWinCondition()
        {
            _isLevelComplete = true;
            SoundManager.Instance.PlayGameWonMusic();
            GameManager.Instance.SetGameWonStats(_coins, (int) (initialTime - _timeRemaining), GameManager.Instance.GetMarioLives());
            GameManager.Instance.LoadGameWonScene();

        }
        
        public MarioState GetMarioState() => MarioModel.State;
        
        public bool IsDead() => _isDead;
        
    }
}