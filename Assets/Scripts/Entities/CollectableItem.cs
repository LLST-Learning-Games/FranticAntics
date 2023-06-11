using System.Collections.Generic;
using DG.Tweening;
using Team;
using UnityEngine;
using UnityEngine.Serialization;
using Worker;

namespace Entities
{
    public class CollectableItem : MonoBehaviour
    {
        [Tooltip("Worker ant brings this to queen instead of the colony")]
        [Header("Settings")]
        public bool ForQueen;
        public bool Mineable;
        public int MinAssignedAntsForPickup = 1;
        public List<WorkerAntController> AntsAssigned;
        public float AssignmentWaitDistance = 1f;

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

        public virtual bool CanCollect => !ItemCollected && ResourcesRemaining > 0f;

        protected virtual void Start()
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

        public virtual void AssignAnt(WorkerAntController ant) => AntsAssigned.Add(ant);
        public virtual void UnassignAnt(WorkerAntController ant) => AntsAssigned.Remove(ant);
        public bool HasEnoughAntsToCarry() => AntsAssigned.Count >= MinAssignedAntsForPickup;
        public bool CheckIfPrimaryCarrier(WorkerAntController ant) => AntsAssigned.IndexOf(ant) == 0;

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