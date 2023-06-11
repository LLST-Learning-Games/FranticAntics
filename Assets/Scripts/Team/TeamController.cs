using AntQueen;
using System.Collections.Generic;
using UnityEngine;
using Worker;

namespace Team
{
    public class TeamController : MonoBehaviour
    {
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
        public float InitialAnts = 6;

        [Header("Game State")]
        public float Score;
        public float Nectar;

        private void Start()
        {
            Nectar = InitialNectar;
        }

        private void Update()
        {
            Nectar += NectarGainPerSecond * Time.deltaTime;
        }
    }
}