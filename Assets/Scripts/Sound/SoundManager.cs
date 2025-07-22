using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace Sound
{
    public enum SoundType
    {
        Jump,
        Coin,
        PowerUp,
        FireBullet,
        StompEnemy,
        LosePowerUp,
        SpinJump,
        MoveCamera
    }
    /**
     * A class that manages the sound in the game.
     */
    public class SoundManager : MonoBehaviour
    {
        [Serializable]
        public class SoundEffectMapping
        {
            public SoundType soundType;
            public AudioClip audioClip;
        }
        public static SoundManager Instance { get; private set; }

        [FormerlySerializedAs("backgroundMusic")]
        [Header("Background Music")]
        [SerializeField] private AudioClip openingSceneMusic;
        [SerializeField] private AudioClip mapSceneMusic;
        [SerializeField] private AudioClip mainBackgroundMusic;
        [SerializeField] private AudioClip deathMusic;
        [SerializeField] private AudioClip endSceneMusic;
        [SerializeField] private AudioClip gameWonMusic;

        [Header("Sound Effects")]
        private Dictionary<SoundType, AudioClip> _soundEffects;
        [SerializeField] private List<SoundEffectMapping> soundEffectMappings;

        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioSource sfxSource;
        
        private AudioClip _currentMusic;
        private bool _isPlaying;
        
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
            _soundEffects = new Dictionary<SoundType, AudioClip>();
            foreach (var mapping in soundEffectMappings)
            {
                _soundEffects[mapping.soundType] = mapping.audioClip;
            }
            musicSource.loop = true;
        }
        void Start()
        {
            PlayBackgroundMusic(openingSceneMusic);
        }
        
        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
        
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            // Assign music based on scene name
            switch (scene.name)
            {
                case "MapScene":
                    PlayBackgroundMusic(mapSceneMusic);
                    break;
                case "MainScene":
                    PlayBackgroundMusic(mainBackgroundMusic);
                    break;
                case "GameOverScene":
                    PlayGameOverMusic();
                    break;
                case "GameWonScene":
                    PlayGameWonMusic();
                    break;
            }
        }
        
        public void PlayBackgroundMusic(AudioClip music)
        {
            if (music != null)
            {
                AdjustMusicSpeed(1);
                musicSource.clip = music;
                musicSource.loop = true;
                musicSource.Play();
            }
        }
        
        public void StopBackgroundMusic()
        {
            musicSource.Stop();
        }

        public void PlayDeathMusic()
        {
            AdjustMusicSpeed(1);
            StopBackgroundMusic();
            musicSource.clip = deathMusic;
            musicSource.loop = false;
            musicSource.Play();
        }
        
        public void PlayGameWonMusic()
        {
            AdjustMusicSpeed(1);
            StopBackgroundMusic();
            musicSource.clip = gameWonMusic;
            musicSource.loop = false;
            musicSource.Play();
        }

        private void PlayGameOverMusic()
        {
            AdjustMusicSpeed(1);
            musicSource.clip = endSceneMusic;
            musicSource.loop = false;
            musicSource.Play();
        }
        
        public void AdjustMusicSpeed(float speedMultiplier)
        {
            if (musicSource != null)
            {
                musicSource.pitch = speedMultiplier;
            }
        }
        
        public void PlaySfx(SoundType type)
        {
            if (_soundEffects.TryGetValue(type, out AudioClip clip) && clip != null)
            {
                sfxSource.PlayOneShot(clip);
            }
        }
        
        public AudioClip getOpeningSceneMusic()
        {
            return openingSceneMusic;
        }
    }
}
