using System;
using System.Collections.Generic;
using UnityEngine;

namespace Worker
{
    public class WorkerAntManager : MonoBehaviour
    {
        private static WorkerAntManager _instance;

        public static WorkerAntManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindObjectOfType<WorkerAntManager>();

                return _instance;
            }
        }

        [SerializeField] private List<WorkerAntController> _allWorkerAnts;

        public void Whistle()
        {
            foreach (var workerAntController in _allWorkerAnts)
            {
                workerAntController.Whistle();
            }
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