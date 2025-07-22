using System;
using Game;
using Mario.Movement.States;
using Mario.Player;
using Mario.PowerUps.FireMario;
using Sound;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.InputSystem;

namespace Mario.Movement
{
    /**
     * The player controller for Mario.
     */
    public class PlayerController : MonoBehaviour
    {
        private static readonly int IsVictory = Animator.StringToHash("IsVictory");
        private MarioPresenter _marioPresenter;
        private bool _isDead;
        [Header("Fireball Settings")]
        [SerializeField] private FireBallPool fireballPool;
        [SerializeField] private Transform fireballSpawnPoint;
        private int _fireballCount = 0;
        private const int MaxFireballsInSpinJump = 3;
        [Header("Movement Settings")]
        private IPlayerState _currentState;
        [SerializeField] private float walkSpeed = 5f;
        [SerializeField] private float runSpeed = 8f;
        [SerializeField] private float jumpForce = 10f;
        [SerializeField] private float worldLimitX = -8f;
        [Header("Camera Settings")]
        [SerializeField] private float cameraPanSpeed = 2f;
        [SerializeField] private float cameraLeftLimit = -10f;
        [SerializeField] private float cameraRightLimit = 10f;
        [SerializeField] private CinemachineCamera cinemachineCam;
        private Transform _cameraFollowTarget;
        
        private InputAction _moveAction;
        private InputAction _lookAction;
        private InputAction _attackAction;
        private InputAction _fire;
        private InputAction _cameraRight;
        private InputAction _cameraLeft;
        private InputAction _crouch;
        private InputAction _jumpAction;
        private InputAction _sprint;
        private InputAction _spinJump;


        private void Input()
        {
            _moveAction = InputSystem.actions.FindAction("Move");
            _lookAction = InputSystem.actions.FindAction("Look");
            _attackAction = InputSystem.actions.FindAction("Attack");
            _fire = InputSystem.actions.FindAction("Fire");
            _cameraRight = InputSystem.actions.FindAction("CameraRight");
            _cameraLeft = InputSystem.actions.FindAction("CameraLeft");
            _crouch = InputSystem.actions.FindAction("Crouch");
            _jumpAction = InputSystem.actions.FindAction("Jump");
            _sprint = InputSystem.actions.FindAction("Sprint");
            _spinJump = InputSystem.actions.FindAction("SpinJump");
        }    
        void Start()
        {
            Input();
            Rigidbody2D = GetComponent<Rigidbody2D>();
            Animator = GetComponent<Animator>();
            
            StandingCollider = GetComponent<CapsuleCollider2D>();
            CrouchingCollider = GetComponent<CircleCollider2D>();
            CrouchingCollider.enabled = false;
            
            transform.localScale = new Vector3(-4, 4, 4); // Facing right (flipped scale)
            _isDead = false;
            
            SetState(new IdleState());
            _marioPresenter = GetComponent<MarioPresenter>();
        }

        void Update()
        {
            // Clamp Mario's position to prevent escaping the world limit
            Vector3 clampedPosition = transform.position;
            clampedPosition.x = Mathf.Max(clampedPosition.x, worldLimitX);
            transform.position = clampedPosition;
            _currentState.UpdateState(this);
            HandleCamera();
            HandleShooting();
        }
    
        public void SetState(IPlayerState newState)
        {
            _currentState?.ExitState(this);
            _currentState = newState;
            _currentState.EnterState(this);
        }
    
        public bool IsRunning { get; set; }
    
        public float WalkSpeed { get => walkSpeed; set => walkSpeed = value; }
    
        public float RunSpeed { get => runSpeed; set => runSpeed = value; }

        public bool IsDead()
        {
            return _marioPresenter.IsDead();
        }
        
        public MarioState GetMarioState()
        {
            return _marioPresenter.GetMarioState();
        }
        
        public Rigidbody2D Rigidbody2D { get; private set; }

        public Animator Animator { get; private set; }

        public bool FacingRight { get; set; } = true;

        public float JumpForce { get => jumpForce; set => jumpForce = value; }
        
        public CapsuleCollider2D StandingCollider { get; private set; }

        public CircleCollider2D CrouchingCollider { get; private set; }
        
        public InputAction MoveAction => _moveAction;
        public InputAction LookAction => _lookAction;
        public InputAction AttackAction => _attackAction;
        public InputAction Fire => _fire;
        public InputAction CameraRight => _cameraRight;
        public InputAction CameraLeft => _cameraLeft;
        public InputAction Crouch => _crouch;
        public InputAction JumpAction => _jumpAction;
        public InputAction Sprint => _sprint;
        public InputAction SpinJump => _spinJump;
        

        public bool IsGrounded()
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1f, LayerMask.GetMask("Ground"));
            return hit.collider != null;
        }

        private void HandleCamera()
        {
            if (_cameraFollowTarget == null)
            {
                return;
            }

            // Adjust the follow target position based on input
            if (_cameraRight.IsPressed())
            {
                _cameraFollowTarget.position += Vector3.right * (cameraPanSpeed * Time.deltaTime);
                SoundManager.Instance.PlaySfx(SoundType.MoveCamera);
            }
            if (_cameraLeft.IsPressed())
            {
                _cameraFollowTarget.position += Vector3.left * (cameraPanSpeed * Time.deltaTime);
                SoundManager.Instance.PlaySfx(SoundType.MoveCamera);
            }

            // Clamp the follow target position to the specified limits
            Vector3 clampedPosition = _cameraFollowTarget.position;
            clampedPosition.x = Mathf.Clamp(clampedPosition.x, cameraLeftLimit, cameraRightLimit);
            _cameraFollowTarget.position = clampedPosition;
        }

        public void ApplyPowerUp(MarioState powerUp)
        {
            _marioPresenter?.ApplyPowerUps(powerUp);
            SoundManager.Instance.PlaySfx(SoundType.PowerUp);
        }

        private void HandleShooting()
        {
            if (_marioPresenter.GetMarioState() == MarioState.Fire)
            {
                if (Fire.WasPerformedThisFrame())
                {
                    ShootFireball();
                }
                if (_currentState is SpinJumpState && _fireballCount < MaxFireballsInSpinJump)
                {
                    ShootFireball();
                    _fireballCount++;
                }
            }
        }

        private void ShootFireball()
        {
            if (_marioPresenter.GetMarioState() == MarioState.Fire)
            {
                var fireball = FireBallPool.Instance.Get();
                fireball.transform.position = fireballSpawnPoint.position;
                Vector2 direction = FacingRight ? Vector2.right : Vector2.left;
                fireball.GetComponent<FireBall>().Fire(direction);
                SoundManager.Instance.PlaySfx(SoundType.FireBullet);
            }
        }
        
        public void ResetFireballCount()
        {
            _fireballCount = 0;
        }

        public bool IsSuperMario()
        {
            return _marioPresenter.GetMarioState() == MarioState.Super;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag($"WinCondition"))
            {
                Animator.SetBool(IsVictory, true);;
                _marioPresenter.HandleWinCondition();
                GameManager.Instance.LoadGameWonScene();
            }
            if(other.CompareTag("LoseCondition"))
                _marioPresenter.Fall();
        }
        
    }
}
