using System.Collections.Generic;
using Team;
using UnityEngine;

namespace Worker.State
{
    public class WorkerDefenseManager : MonoBehaviour
    {
        [SerializeField] private TeamController _teamController;
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
            var angle = 360 / (_defenceAnts.Count > 6 ? 6 : _defenceAnts.Count);

            tempObject.transform.position = _teamController.Queen.transform.position;
            tempObject.transform.eulerAngles = _teamController.Queen.transform.eulerAngles;

            var index = 0;
            foreach (var antController in _defenceAnts)
            {
                if(antController.GetCurrentStateController() is not WorkerDefenceState defenceState)
                    continue;

                defenceState.DefencePositionOffset = tempObject.transform.forward.normalized * ((index / 6) + 1);
                tempObject.transform.eulerAngles += new Vector3(0, angle, 0);

                index++;
            }
            
            Destroy(tempObject);
        }
    }
}