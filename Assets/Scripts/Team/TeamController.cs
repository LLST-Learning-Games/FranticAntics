using AntQueen;
using UnityEngine;
using Worker;

namespace Team
{
    public class TeamController : MonoBehaviour
    {
        public WorkerAntManager WorkerAntManager;
        public Queen Queen;

        [Space] 
        [SerializeField] private int InitialWorkerAntCount = 6;
    }
}