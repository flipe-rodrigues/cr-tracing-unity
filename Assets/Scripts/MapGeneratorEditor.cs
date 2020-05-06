using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapGenerator))]
public class MapGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        MapGenerator levelGenerator = (MapGenerator)target;

        if (GUILayout.Button("Generate"))        {
            levelGenerator.GenerateMaps();
        }
    }
}