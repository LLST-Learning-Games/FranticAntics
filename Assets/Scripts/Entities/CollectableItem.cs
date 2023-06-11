using UnityEngine;
using UnityEngine.Serialization;

namespace Entities
{
    public class CollectableItem : MonoBehaviour
    {
        [Tooltip("Worker ant brings this to queen instead of the colony")]
        public bool ForQueen;

        public float SpeedMultiplier = 1f;
        public bool Mineable;
        [SerializeField] private GameObject _minedPiecePrefab;

        [HideInInspector] public bool ItemCollected;

        public GameObject Mine()
        {
            if (!Mineable)
                return null;

            var piece = Instantiate(_minedPiecePrefab, transform);
            return piece;
        }
    }
}