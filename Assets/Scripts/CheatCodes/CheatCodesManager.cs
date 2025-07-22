using System;
using Enemies;
using Game;
using Mario.Movement;
using Mario.Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CheatCodes
{
    public class CheatCodesManager : MonoBehaviour
    {
        [SerializeField] private GameObject dinosaurPrefab;
        [SerializeField] private GameObject blackEnemyPrefab;
        private GameObject _player;
        private PlayerController _playerController;
        private MarioPresenter _marioPresenter;
        private Vector3 _initialPosition;
        private GameObject[] _enemies;
        private Vector3[] _enemyInitialPositions;
        private EnemyType[] _enemyTypes;

        private InputAction _fallCondition;
        private InputAction _resetPosition;
        private InputAction _cycleMarioState;
        private InputAction _resetEnemies;
        private InputAction _resetGame;

        private void Input()
        {
            _fallCondition = InputSystem.actions.FindAction("FallCondition");
            _resetPosition = InputSystem.actions.FindAction("ResetPosition");
            _cycleMarioState = InputSystem.actions.FindAction("CycleMarioState");
            _resetEnemies = InputSystem.actions.FindAction("ResetEnemies");
            _resetGame = InputSystem.actions.FindAction("ResetGame");
        }

        void Start()
        {
            DontDestroyOnLoad(gameObject);
            Input();
            _player = GameObject.FindGameObjectWithTag("Player");
            _playerController = _player.GetComponent<PlayerController>();
            _marioPresenter = _player.GetComponent<MarioPresenter>();
            _initialPosition = _player.transform.position;
            _enemies = GameObject.FindGameObjectsWithTag("Enemy");
            _enemyInitialPositions = new Vector3[_enemies.Length];
            _enemyTypes = new EnemyType[_enemies.Length];
            for (int i = 0; i < _enemies.Length; i++)
            {
                _enemyInitialPositions[i] = _enemies[i].transform.position;
                var enemyBehavior = _enemies[i].GetComponent<EnemyBehavior>();
                if (enemyBehavior != null)
                {
                    _enemyTypes[i] = enemyBehavior.EnemyType;
                }
            }
        }

        void Update()
        {
            if (_fallCondition.IsPressed())
                TeleportToLoseCondition();

            if (_resetPosition.IsPressed())
                ResetToInitialPosition();

            if (_cycleMarioState.IsPressed())
                CycleMarioState();

            if (_resetEnemies.IsPressed())
                ResetEnemies();
            if (_resetGame.IsPressed())
                ResetGame();
        }
        
        private void TeleportToLoseCondition()
        {
            GameObject loseCondition = GameObject.FindGameObjectWithTag("LoseCondition");
            if (loseCondition != null)
            {
                Collider2D[] colliders = loseCondition.GetComponents<Collider2D>();
                if (colliders.Length > 0)
                {
                    _player.transform.position = colliders[0].transform.position;
                }
            }
        }

        private void ResetToInitialPosition()
        {
            _player.transform.position = _initialPosition;
        }

        private void CycleMarioState()
        {
            MarioState currentState = _marioPresenter.GetMarioState();
            MarioState nextState = currentState switch
            {
                MarioState.Regular => MarioState.Super,
                MarioState.Super => MarioState.Fire,
                MarioState.Fire => MarioState.Regular,
                _ => MarioState.Regular
            };

            _playerController.ApplyPowerUp(nextState);
        }

        private void ResetMarioHealth()
        {
            GameManager.Instance.ResetGame();
        }

        private void ResetEnemies()
        {
            for (int i = 0; i < _enemies.Length; i++)
            {
                GameObject enemy = _enemies[i];
                if (enemy == null)
                {
                    // Respawn destroyed enemies
                    GameObject prefabToSpawn = null;
                    switch (_enemyTypes[i])
                    {
                        case EnemyType.Dinosaur:
                            prefabToSpawn = dinosaurPrefab;
                            break;
                        case EnemyType.Black:
                            prefabToSpawn = blackEnemyPrefab;
                            break;
                    }

                    if (prefabToSpawn != null)
                    {
                        enemy = Instantiate(prefabToSpawn, _enemyInitialPositions[i], Quaternion.identity);
                        _enemies[i] = enemy;
                    }
                }
                else
                {
                    // Reset existing enemies
                    enemy.transform.position = _enemyInitialPositions[i];
                    var enemyBehavior = enemy.GetComponent<EnemyBehavior>();
                    if (enemyBehavior != null)
                    {
                        enemyBehavior.ResetEnemyState();
                    }
                }
            }
        }

        private void ResetGame()
        {
            GameManager.Instance.ResetGame();
        }
    }
}
