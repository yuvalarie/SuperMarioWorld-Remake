using System;
using Game;
using Mario.Movement;
using UnityEngine;
using UnityEngine.InputSystem;

public class MapMario : MonoBehaviour
{
    private static readonly int IsWalking = Animator.StringToHash("IsWalking");
    private static readonly int IsEnteringLevel = Animator.StringToHash("IsEnteringLevel");
    
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float diagonalFactor = 0.7f;
    [SerializeField] private Transform targetPoint;
    
    private Rigidbody2D _playerRigidbody2D;
    private Animator _animator;
    private Vector2 _targetPosition;
    private bool _isMoving = false;
    private InputAction _clickAction;
    

    private void Start()
    {
        _clickAction = InputSystem.actions.FindAction("Click");
        _playerRigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        if (targetPoint != null)
        {
            _targetPosition = targetPoint.position;
        }
    }

    void Update()
    {
        if (_clickAction.IsPressed() && ! _isMoving)
        {
            _isMoving = true;
            _animator.SetBool(IsWalking, true);
            
            // Move the player left diagonally
            Vector2 velocity = _playerRigidbody2D.linearVelocity;
            velocity.x = -moveSpeed;                       // Move left
            velocity.y = moveSpeed * diagonalFactor;       // Move slightly upwards
            _playerRigidbody2D.linearVelocity = velocity;
            _animator.SetBool(IsWalking, true);
        }
        if (_isMoving)
        {
            MoveTowardsTarget();
        }
        
        else
        {
            // Stop the player's movement if no key is pressed
            _playerRigidbody2D.linearVelocity = Vector2.zero;
            _animator.SetBool(IsWalking, false);
        }
    }
    
    private void MoveTowardsTarget()
    {
        Vector2 currentPosition = transform.position;
        Vector2 direction = (_targetPosition - currentPosition).normalized;
        float distance = Vector2.Distance(currentPosition, _targetPosition);

        if (distance > 0.1f) // Keep moving until close enough to the target
        {
            _playerRigidbody2D.linearVelocity = direction * moveSpeed;
        }
        else
        {
            StopWalking();
        }
    }

    private void StopWalking()
    {
        _playerRigidbody2D.linearVelocity = Vector2.zero;
        _isMoving = false;
        _animator.SetBool(IsWalking, false);
    }
    
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("EnterLevelCondition"))
        {
            StopWalking();
            _animator.SetTrigger(IsEnteringLevel);
            GameManager.Instance.LoadMainScene();
        }
    }
}
