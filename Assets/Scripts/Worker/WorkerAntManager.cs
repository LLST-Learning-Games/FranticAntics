using System;
using System.Collections.Generic;
using UnityEngine;

namespace Worker
{
    public class WorkerAntManager : MonoBehaviour
    {
        public static WorkerAntManager Instance;

        [SerializeField] private List<WorkerAntController> _allWorkerAnts;

        private void Awake()
        {
            Instance = this;

            _allWorkerAnts = new();
        }

        public void RegisterWorkerAnt(WorkerAntController workerAntController)
        {
            if(!_allWorkerAnts.Contains(workerAntController))
                _allWorkerAnts.Add(workerAntController);
        }

        public void RemoveWorkerAnt(WorkerAntController workerAntController)
        {
            if (_allWorkerAnts.Contains(workerAntController))
                _allWorkerAnts.Remove(workerAntController);
        }
    }
}