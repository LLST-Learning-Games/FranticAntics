using DG.Tweening;
using Team;
using UnityEngine;
using UnityEngine.Serialization;

namespace Entities
{
    public class CollectableItem : MonoBehaviour
    {
        [Tooltip("Worker ant brings this to queen instead of the colony")]
        [Header("Settings")]
        public bool ForQueen;
        public bool Mineable;

        [Header("Modifiers")]
        public float SpeedMultiplier = 1f;
        public AnimationCurve SizeFromResourcesRemaining;

        [Header("Resources")]
        public Resource Resource;
        public float TotalResources;
        public float ResourcesPerPickup;
        [SerializeField] private GameObject _minedPiecePrefab;

        [Space]
        public float ResourcesRemaining;

        [HideInInspector] public bool ItemCollected;
        private float initialScale;

        private void Start()
        {
            ResourcesRemaining = TotalResources;
            initialScale = transform.lossyScale.x;
        }

        public GameObject Mine()
        {
            if (!Mineable)
                return null;

            var piece = Instantiate(_minedPiecePrefab, transform);
            return piece;
        }

        public virtual float GetResources()
        {
            float result = Mathf.Min(ResourcesRemaining, ResourcesPerPickup);
            ResourcesRemaining -= result;

            UpdateMineIndicators();

            return result;
        }

        private void UpdateMineIndicators()
        {
            if (!Mineable)
                return;

            float resourcesPercentRemaining = ResourcesRemaining / TotalResources;
            float scale = SizeFromResourcesRemaining.Evaluate(resourcesPercentRemaining) * initialScale;

            if (ResourcesRemaining <= 0)
                scale = 0.0f;

            transform.DOScale(scale, 0.2f).OnComplete(
                () =>
                {
                    if (ResourcesRemaining <= 0)
                        Destroy(gameObject);
                }
            );
        }

        public virtual void Consume(TeamController teamController, float resourcesPickedUp)
        {
            switch (Resource)
            {
                case Resource.Score:
                    teamController.Score += resourcesPickedUp;
                    break;
                case Resource.Nectar:
                    teamController.Nectar += resourcesPickedUp;
                    break;
                default:
                    throw new System.Exception("Unhandled case");
            }
        }
    }
}