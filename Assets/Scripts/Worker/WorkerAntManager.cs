using System.Collections.Generic;
using Team;
using UnityEngine;
using Worker.State;

namespace Worker
{
    public class WorkerAntManager : MonoBehaviour
    {
        public WorkerDefenseManager DefenseManager;
        [SerializeField] private List<WorkerAntController> _allWorkerAnts;

        public void Whistle(float whistleRange, Vector3 queenWorldPosition)
        {
            foreach (var workerAntController in _allWorkerAnts)
            {
                if(workerAntController.Status == WorkerAntStatus.CollectFood || workerAntController.Status == WorkerAntStatus.Attack)
                    continue;

                Vector3 workerWorldPosition = workerAntController.transform.position;
                float distance = Vector3.Distance(workerWorldPosition, queenWorldPosition);

                if (distance > whistleRange)
                    continue;
                
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
            
            DefenseManager.RemoveAntFromDefense(workerAntController);

            workerAntController.TeamController.workers.Remove(workerAntController);
        }
    }
}