using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LevelSpawnData))]
public class LevelSpawnDataEditor : Editor
{
    void OnEnable()
    {
        SceneView.duringSceneGui += OnSceneGUI;
    }

    void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
    }

    void OnSceneGUI(SceneView sv)
    {
        LevelSpawnData levelSpawnData = (LevelSpawnData)target;
        foreach (var spawnPointData in levelSpawnData.LevelSpawnPoints)
        {
            Handles.PositionHandle(spawnPointData.MapPosition, Quaternion.identity);
        }
    }
}