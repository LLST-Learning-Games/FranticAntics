using Team;
using UnityEngine;
using UnityEngine.Serialization;

namespace Entities
{
    public class CollectableItem : MonoBehaviour
    {
        [Tooltip("Worker ant brings this to queen instead of the colony")]
        public bool ForQueen;
        public bool Mineable;

        public float SpeedMultiplier = 1f;
        public float Points = 0f;
        public float Nectar = 0f;
        [SerializeField] private GameObject _minedPiecePrefab;

        [HideInInspector] public bool ItemCollected;

        public GameObject Mine()
        {
            if (!Mineable)
                return null;

            var piece = Instantiate(_minedPiecePrefab, transform);
            return piece;
        }

        public virtual void Consume(TeamController teamController)
        {
            teamController.Nectar += Nectar;
            teamController.Score += Points;
        }
    }
}