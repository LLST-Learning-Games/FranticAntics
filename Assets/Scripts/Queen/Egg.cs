using DG.Tweening;
using Team;
using UnityEngine;
using Worker;

public class Egg : MonoBehaviour
{
    [SerializeField] private WorkerAntController _antPrefab;
    [SerializeField] private float _travelTime;
    private TeamController _teamController;
    private Vector3 _targetPosition;

    public void Initialize(Vector3 targetPosition, TeamController teamController)
    {
        _targetPosition = targetPosition;
        _teamController = teamController;

        transform.DOLocalMove(_targetPosition, _travelTime);
    }
    
    public void Update()
    {
        if (Vector3.Distance(transform.position, _targetPosition) <= 0.1f)
        {
            SpawnAnt();
            Destroy(gameObject);
        }
    }

    private void SpawnAnt()
    {
        var newAnt = Instantiate(_antPrefab, transform.position, Quaternion.identity);
        newAnt.TeamController = _teamController;
        newAnt.Initialize();
        _teamController.workers.Add(newAnt);
    }
}