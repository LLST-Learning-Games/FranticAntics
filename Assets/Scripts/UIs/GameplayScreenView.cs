using System;
using System.Collections;
using System.Collections.Generic;
using Team;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace UI
{
    public class GameplayScreenView : MonoBehaviour
    {
        public static GameplayScreenView Instance;
        
        [Header("Controller Buttons")]
        [SerializeField] private ControllerButtonDirection _startKey = ControllerButtonDirection.South;

        [Header("Sounds")]
        [SerializeField] private AudioClip backgroundClip;

        [Header("UI")]
        [SerializeField] private GameObject _gameOnObjects;
        [SerializeField] private GameObject _gameOverObjects;
        [SerializeField] private TextMeshProUGUI _timer;
        [SerializeField] private TextMeshProUGUI _winnerLabel;
        [SerializeField] private TeamManager _teamManager;
        
        [SerializeField] private float _roundTimeLimit;
        [SerializeField] private float _countdownTime = 3;
        [SerializeField] private TextMeshProUGUI _countdownText;
        [SerializeField] private int _titleSceneIndex = 0;
        private float _roundTimeRemaining;
        private float _countdownTimeRemaining;
        public bool GameRunning;
        public bool GameIsOver = false;
        public float GameOverDelay = 2f;
        
        // Start is called before the first frame update
        void Start()
        {
            Instance = this;
            SetupRound();
            AudioSource listener = gameObject.AddComponent<AudioSource>();
            listener.clip = backgroundClip;
            listener.loop = true;
            listener.Play();
        }

        private void SetupRound()
        {
            _gameOnObjects.SetActive(true);
            _gameOverObjects.SetActive(false);
            Time.timeScale = 1.0f;
            _roundTimeRemaining = _roundTimeLimit;
            _countdownTimeRemaining = _countdownTime;
        }

        // Update is called once per frame
        void Update()
        {
            if (!GameRunning)
            {
                if (_countdownTimeRemaining > 0)
                {
                    _countdownText.gameObject.SetActive(true);
                    _countdownTimeRemaining -= Time.deltaTime;
                    _countdownText.text = ((int)_countdownTimeRemaining).ToString();
                }
                else
                {
                    _countdownText.gameObject.SetActive(false);
                    GameRunning = true;
                }
                return;
            }

            if (GameIsOver)
            {
                if (InputUtility.IsButtonDown(0, _startKey) || InputUtility.IsButtonDown(1, _startKey))
                {
                    ReturnToTitle();
                }
            }

            _roundTimeRemaining -= Time.deltaTime;
            TimeSpan timeRemaining = TimeSpan.FromSeconds(_roundTimeRemaining);
            _timer.text = $"{timeRemaining.Minutes}:{timeRemaining.Seconds:00}";

            if (_roundTimeRemaining <= 0.0f)
            {
                GameOver();
                _roundTimeRemaining = 0.0f;
            }
        }

        private void GameOver()
        {
            UpdateGameOverLabel();
            _gameOverObjects.SetActive(true);
            Time.timeScale = 0.0f;
            StartCoroutine(DelayGameOver(GameOverDelay));
        }

        private IEnumerator DelayGameOver(float delay)
        {
            float timer = 0f;
            while (timer < delay)
            {
                timer += Time.unscaledDeltaTime;
                yield return null;
            }
            GameIsOver = true;
        }

        private void UpdateGameOverLabel()
        {

            if (_teamManager.TeamOne.Score - _teamManager.TeamTwo.Score <= Double.Epsilon)
            {
                _winnerLabel.text = "It's a tie!";
                return;
            }
            if (_teamManager.TeamOne.Score > _teamManager.TeamTwo.Score)
            {
                _winnerLabel.text = "Colony One Wins!";
                return;
            }
            _winnerLabel.text = "Colony Two Wins!";
            
        }

        public void ReturnToTitle()
        {
            SceneManager.LoadScene(_titleSceneIndex);
        }
    }
}