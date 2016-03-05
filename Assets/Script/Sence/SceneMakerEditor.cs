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
        EditorGUILayout.PropertyField(serializedObject.FindProperty("CornerPrefab"), true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("RoomPrefab"), true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("EndPrefab"), true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("WallPrefab"), true);

        EditorGUILayout.PropertyField(serializedObject.FindProperty("normalEnemyPrefab"), true);

        
        serializedObject.ApplyModifiedProperties();

        if (GUILayout.Button("Create Start"))
        {
            sceneMaker.CreateStart();
        }
    }


}
#endif