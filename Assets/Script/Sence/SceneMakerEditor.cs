#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Collections;



[CustomEditor(typeof(SceneMaker))]
public class SceneMakerEditor : Editor
{

    private SceneMaker sceneMaker;
    public override void OnInspectorGUI()
    {
        sceneMaker = (SceneMaker)target;

        GUILayout.Label(string.Format("Set the map size"));

        EditorGUILayout.BeginHorizontal();
        sceneMaker.mapheigth = EditorGUILayout.IntField("MapHeight", sceneMaker.mapheigth);
        sceneMaker.mapwidth = EditorGUILayout.IntField("MapWidth", sceneMaker.mapwidth);
        EditorGUILayout.EndHorizontal();


        serializedObject.Update();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("UpPrefab"), true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("DownPrefab"), true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("PortalPrefab"), true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("RoadPrefab"), true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("cornerPrefab"), true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("RoomPrefab"), true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("EndPrefab"), true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("normalPrefab"), true);

        serializedObject.ApplyModifiedProperties();

        if (GUILayout.Button("Make Normal Prefab"))
        {
            makeNormalPrefab();
        }
        if (GUILayout.Button("Create Up"))
        {
            sceneMaker.MakeUpPoint();
        }
    }
    void makeNormalPrefab()
    {
        sceneMaker.normalPrefab = new GameObject[sceneMaker.RoadPrefab.Length + sceneMaker.RoomPrefab.Length + sceneMaker.cornerPrefab.Length];

        sceneMaker.RoadPrefab.CopyTo(sceneMaker.normalPrefab, 0);
        sceneMaker.RoomPrefab.CopyTo(sceneMaker.normalPrefab, sceneMaker.RoadPrefab.Length);
        sceneMaker.cornerPrefab.CopyTo(sceneMaker.normalPrefab, sceneMaker.RoadPrefab.Length + sceneMaker.RoomPrefab.Length);
    }

}
#endif