using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Worker.State;

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

        public WorkerDefenseManager DefenseManager;
        [SerializeField] private List<WorkerAntController> _allWorkerAnts;

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Space))
                SendForSearch();
            
            if(Input.GetKeyDown(KeyCode.D))
                Whistle();
        }

        public void Whistle()
        {
            foreach (var workerAntController in _allWorkerAnts)
            {
                workerAntController.Whistle(Vector3.zero);
            }
        }

        public void SendForSearch()
        {
            var availableAnt = DefenseManager.GetAnt();
            
            if(availableAnt != null)
            {
                DefenseManager.RemoveAntFromDefense(availableAnt);
                availableAnt.SendSearch();
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