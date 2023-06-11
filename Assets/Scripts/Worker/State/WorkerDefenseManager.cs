using System;
using System.Collections.Generic;
using Team;
using UnityEngine;

namespace Worker.State
{
    public class WorkerDefenseManager : MonoBehaviour
    {
        private const int BASE_DEFENSE_COUNT = 6;
        
        [SerializeField] private TeamController _teamController;
        [SerializeField] private List<WorkerAntController> _defenceAnts;

        [SerializeField] private float _lineRange = 1f;
        
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

            tempObject.transform.position = _teamController.Queen.transform.position;
            tempObject.transform.eulerAngles = _teamController.Queen.transform.eulerAngles;
            
            var lineCount = GetDefenseLineCount();
            var antIndex = 0;

            for (var line = 0; line < lineCount; line++)
            {
                var antCountInLine = BASE_DEFENSE_COUNT * (int)Math.Pow(2, line);
                var angle = 360 / antCountInLine;
                
                for (var xx = 0; xx < antCountInLine; xx++)
                {
                    if(antIndex >= _defenceAnts.Count)
                        break;

                    var antController = _defenceAnts[antIndex];
                    if(antController.GetCurrentStateController() is not WorkerDefenceState defenceState)
                        continue;
                
                    defenceState.DefencePositionOffset = tempObject.transform.forward.normalized * ((line + 1) * _lineRange);
                    tempObject.transform.eulerAngles += new Vector3(0, angle, 0);
                
                    antIndex++;
                }
            }

            Destroy(tempObject);
        }

        private int GetDefenseLineCount()
        {
            var antCount = _defenceAnts.Count;
            var lineCount = 0;
            while (antCount > 0)
            {
                antCount -= BASE_DEFENSE_COUNT * (int) Math.Pow(2, lineCount);
                lineCount++;
            }
            
            return lineCount;
        }
    }
}