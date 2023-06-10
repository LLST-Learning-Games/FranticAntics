using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SpawnPointData))]
public class SpawnPointDataEditor : Editor
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
        SpawnPointData spawnPointData = (SpawnPointData)target;
        Vector3 newPoint = spawnPointData.MapPosition;
        newPoint = Handles.PositionHandle(newPoint, Quaternion.identity);

        if (GUI.changed)
        {
            // for undo operation
            Undo.RecordObject(target, "Move Point");

            // apply changes back to target component
            spawnPointData.MapPosition = newPoint;
        }
    }
}