using System;
using System.Collections;
using System.Collections.Generic;
using Team;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
    public class GameplayScreenView : MonoBehaviour
    {
        [SerializeField] private GameObject _gameOnObjects;
        [SerializeField] private GameObject _gameOverObjects;
        [SerializeField] private TextMeshProUGUI _timer;
        [SerializeField] private TextMeshProUGUI _playerOneScore;
        [SerializeField] private TextMeshProUGUI _playerTwoScore;
        [SerializeField] private TextMeshProUGUI _winnerLabel;
        [SerializeField] private TeamManager _teamManager;
        
        [SerializeField] private float _roundTimeLimit;
        private float _roundTimeRemaining;
        
        
        // Start is called before the first frame update
        void Start()
        {
            SetupRound();
        }

        private void SetupRound()
        {
            _gameOnObjects.SetActive(true);
            _gameOverObjects.SetActive(false);
            Time.timeScale = 1.0f;
            _roundTimeRemaining = _roundTimeLimit;
        }

        // Update is called once per frame
        void Update()
        {
            _playerOneScore.text = _teamManager.TeamOne.Score.ToString();
            _playerTwoScore.text = _teamManager.TeamTwo.Score.ToString();
            
            _roundTimeRemaining -= Time.deltaTime;
            _timer.text = $"{_roundTimeRemaining:0.##}";
            if (_roundTimeRemaining <= 0.0f)
            {
                GameOver();
            }
        }

        private void GameOver()
        {
            UpdateGameOverLabel();
            _gameOverObjects.SetActive(true);
            Time.timeScale = 0.0f;
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
    }
}