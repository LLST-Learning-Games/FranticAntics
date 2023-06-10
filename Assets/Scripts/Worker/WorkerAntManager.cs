using System;
using System.Collections.Generic;
using System.Linq;
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

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Space))
                SendForSearch();
        }

        public void Whistle()
        {
            foreach (var workerAntController in _allWorkerAnts)
            {
                workerAntController.Whistle();
            }
        }

        public void SendForSearch()
        {
            var availableAnt = _allWorkerAnts.FirstOrDefault(x => x.Status != WorkerAntStatus.SearchFood);
            
            if(availableAnt != null)
                availableAnt.SendSearch();
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