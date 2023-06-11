using System;
using UnityEngine;

namespace PowerUps
{
    [Serializable]
    public class PowerUpData
    {
        public Action<PowerUpData> OnPowerUpFinished;

        public PowerUpType PowerUpType;
        public int PowerUpMultiplier;
        public float PowerUpDuration;

        [SerializeField] private float _powerUpTimer;

        public void Update()
        {
            if(_powerUpTimer < 0)
                return;

            _powerUpTimer -= Time.deltaTime;
            
            if(_powerUpTimer < 0)
                OnPowerUpFinished?.Invoke(this);
        }
    }
}