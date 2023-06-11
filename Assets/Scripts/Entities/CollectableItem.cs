using UnityEngine;
using UnityEngine.Serialization;

namespace Entities
{
    public class CollectableItem : MonoBehaviour
    {
        [Tooltip("Worker ant brings this to queen instead of the colony")]
        public bool ForQueen;

        public float SpeedMultiplier = 1f;

        [FormerlySerializedAs("Collected")] public bool ItemCollected;
    }
}