using System;
using UnityEditor;
using UnityEngine;

namespace Worker.Editor
{
    [CustomEditor(typeof(WorkerAntManager))]
    public class WorkerAntManagerEditor : UnityEditor.Editor
    {
        private WorkerAntManager _workerAntManager;

        private void OnEnable()
        {
            _workerAntManager = target as WorkerAntManager;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Whistle"))
            {
                _workerAntManager.Whistle();
            }
        }
    }
}