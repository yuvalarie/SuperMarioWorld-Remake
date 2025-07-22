using System.Collections;
using CameraScripts;
using Sound;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using static Game.SceneFactory;

namespace Game
{
    /**
     * This class is responsible for managing the game state and transitions between scenes.
     */
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;
        
        [SerializeField] private int marioLives = 5;
        [SerializeField] private CameraMover mainCamera;
        
        private bool _openingSceneComplete = false;
        private int _finalCoins;
        private int _finalTime;
        private int _finalLives;
        
        private Coroutine _openingSceneCoroutine;
        private InputAction _clickAction;
        private bool _isTransitioning = false;


        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        private void Start()
        {
            _clickAction = InputSystem.actions.FindAction("Click");
            if (SceneManager.GetActiveScene().name == GetSceneName(SceneName.OpeningScene))
            {
                mainCamera.OnCameraReachedEnd += HandleOpeningSceneComplete;
            }
        }


        private void Update()
        {
            if (SceneManager.GetActiveScene().name == GetSceneName(SceneName.OpeningScene) && !_openingSceneComplete)
            {
                if (_clickAction.IsPressed() || _openingSceneComplete)
                {
                    LoadMapScene();
                    _openingSceneComplete = true; // Prevent multiple triggers
                }
            }
        }
        private void HandleOpeningSceneComplete()
        {
            _openingSceneComplete = true;
            LoadMapScene();
        }

        private void LoadSceneAsync(SceneName scene)
        {
            StartCoroutine(LoadSceneCoroutine(scene));
        }

        private IEnumerator LoadSceneCoroutine(SceneName scene)
        {
            // Start loading the scene
            AsyncOperation operation = SceneManager.LoadSceneAsync(GetSceneName(scene));
            operation.allowSceneActivation = false; 
            // Optional: Show loading progress
            while (!operation.isDone)
            {
                if (operation.progress >= 0.9f)
                {
                    operation.allowSceneActivation = true;
                }
                yield return null; 
            }
        }

        public void LoadMapScene()
        {
            LoadSceneAsync(SceneName.MapScene);
        }
        public void LoadMainScene()
        {
            StartCoroutine(LoadGameStartThenMainScene());
        }
        
        public void LoadGameOverScene()
        {
            LoadSceneAsync(SceneName.GameOverScene);
        }
        
        public void LoadTimeOverScene()
        {
            StartCoroutine(TimeUpThenLoadNextScene());
        }
        public void LoadGameWonScene()
        {
            StartCoroutine(WaitThenLoadGameWonScene());
        }
        
        private IEnumerator LoadGameStartThenMainScene()
        {
            LoadSceneAsync(SceneName.GameStartScene);
            yield return new WaitForSeconds(1f);
            LoadSceneAsync(SceneName.MainScene);
        }
        
        private IEnumerator TimeUpThenLoadNextScene()
        {
            if (_isTransitioning) yield break;
            _isTransitioning = true;
            SoundManager.Instance.StopBackgroundMusic();
            yield return LoadSceneCoroutine(SceneName.TimeOverScene);
            if (marioLives <= 0)
            {
               LoadGameOverScene();
            }
            else
            {

                LoadMapScene();
            }
            _isTransitioning = false;
        }
        
        private IEnumerator WaitThenLoadGameWonScene()
        {
            yield return new WaitForSeconds(1f);
            LoadSceneAsync(SceneName.GameWonScene);
        }

        public void ResetGame()
        {
            marioLives = 5;
            _openingSceneComplete = false;
            SoundManager.Instance.PlayBackgroundMusic(SoundManager.Instance.getOpeningSceneMusic());
            LoadSceneAsync(SceneName.OpeningScene);
        }
        
        public void SetGameWonStats(int coins, int time, int lives)
        {
            _finalCoins = coins;
            _finalTime = time;
            _finalLives = lives;
        }
        
        public void DecreaseMarioLives()
        {
            marioLives--;
        }
        
        public int GetMarioLives() => marioLives;
        
        public int GetFinalCoins() => _finalCoins;
        
        public int GetFinalTime() => _finalTime;
        
        public int GetFinalLives() => _finalLives;
    }
}
