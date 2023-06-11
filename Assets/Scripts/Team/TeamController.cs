using System;
using AntQueen;
using System.Collections.Generic;
using PowerUps;
using UnityEngine;
using Worker;

namespace Team
{
    public class TeamController : MonoBehaviour
    {
        public Action<PowerUpData> OnPowerUpStarted;
        public Action<PowerUpData> OnPowerUpFinished;

        [SerializeField] private List<PowerUpData> _activePowerUps;

        public WorkerAntManager WorkerAntManager;
        public Queen Queen;
        public Colony Colony;
        public Player Player;
        public HashSet<WorkerAntController> workers = new HashSet<WorkerAntController>();

        [Header("Settings")]
        public int MaximumAnts;
        public float InitialNectar = 100.0f;
        public float AntNectarCost = 20.0f;
        public float NectarGainPerSecond = 2.0f;
        public float StopNectarGainAtAmount = 20.0f;
        public float InitialAnts = 6;
        public Color WorkerAntColor = Color.white;

        [Header("Game State")]
        public float Score;
        public float Nectar;

        private void Start()
        {
            Nectar = InitialNectar;
        }

        private void Update()
        {
            if (Nectar < StopNectarGainAtAmount)
            {
                Nectar += NectarGainPerSecond * Time.deltaTime;
            }

            if (_activePowerUps.Count != 0)
            {
                foreach (var activePowerUp in _activePowerUps)
                {
                    activePowerUp.Update();
                }
            }
        }

        public void AddPowerUP(PowerUpData powerUpData)
        {
            _activePowerUps.Add(powerUpData);

            OnPowerUpStarted?.Invoke(powerUpData);
            
            powerUpData.OnPowerUpFinished += data =>
            {
                OnPowerUpFinished?.Invoke(data);
            };
        }
    }
}