using System;
using System.Collections.Generic;
using UnityEngine;

namespace Worker.State
{
    public class WorkerDefenseManager : MonoBehaviour
    {
        [SerializeField] private Transform _queen;
        [SerializeField] private List<WorkerAntController> _defenceAnts;

        public void AddAntToDefense(WorkerAntController workerAntController)
        {
            if(_defenceAnts.Contains(workerAntController))
                return;
                
            _defenceAnts.Add(workerAntController);
            workerAntController.Whistle(Vector3.zero);
        }

        public void RemoveAntFromDefense(WorkerAntController workerAntController)
        {
            if(_defenceAnts.Contains(workerAntController))
                _defenceAnts.Remove(workerAntController);
        }

        public WorkerAntController GetAnt()
        {
            if (_defenceAnts.Count == 0)
                return null;
            
            return _defenceAnts[0];
        }
        
        private void Update()
        {
            if(_defenceAnts.Count == 0)
                return;

            var tempObject = new GameObject("temp");
            var angle = 360 / _defenceAnts.Count;

            tempObject.transform.position = _queen.position;
            tempObject.transform.eulerAngles = _queen.eulerAngles;

            foreach (var antController in _defenceAnts)
            {
                if(antController.GetCurrentStateController() is not WorkerDefenceState defenceState)
                    continue;

                defenceState.DefencePositionOffset = tempObject.transform.forward.normalized;
                tempObject.transform.eulerAngles += new Vector3(0, angle, 0);
            }
            
            Destroy(tempObject);
        }
    }
}