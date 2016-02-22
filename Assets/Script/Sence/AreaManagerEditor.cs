#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Collections;



[CustomEditor(typeof(AreaManager))]
public class AreaManagerEditor : Editor
{
    private AreaManager areaManager;
    public override void OnInspectorGUI()
    {
        areaManager = (AreaManager)target;
        //int pix = 10;
        //GUILayout.Label(string.Format("World : Map(virtual) = {0} : 1", pix));
        areaManager.height = EditorGUILayout.IntField("Height", areaManager.height);
        areaManager.width = EditorGUILayout.IntField("Width", areaManager.width);
        areaManager.centerPointUp = EditorGUILayout.Vector3Field("centerPointUp", new Vector3(0, 0, areaManager.height / 2));
        areaManager.centerPointDown = EditorGUILayout.Vector3Field("centerPointDown", new Vector3(0, 0, -areaManager.height / 2));
        areaManager.centerPointLeft = EditorGUILayout.Vector3Field("centerPointLeft", new Vector3(-areaManager.width / 2, 0, 0));
        areaManager.centerPointRight = EditorGUILayout.Vector3Field("centerPointRight", new Vector3(areaManager.width / 2, 0, 0));

        areaManager.centerPointUpRoted = EditorGUILayout.Vector3Field("centerPointUpRoted", new Vector3(0, 0, areaManager.width / 2));
        areaManager.centerPointDownRoted = EditorGUILayout.Vector3Field("centerPointDownRoted", new Vector3(0, 0, -areaManager.width / 2));
        areaManager.centerPointLeftRoted = EditorGUILayout.Vector3Field("centerPointLeftRoted", new Vector3(-areaManager.height / 2, 0, 0));
        areaManager.centerPointRightRoted = EditorGUILayout.Vector3Field("centerPointRightRoted", new Vector3(areaManager.height / 2, 0, 0));

        serializedObject.Update();
        SerializedProperty areaOutList = serializedObject.FindProperty("areaOut");
        EditorGUILayout.PropertyField(areaOutList);
        EditorGUI.indentLevel += 1;
        if (areaOutList.isExpanded)
        {
            EditorGUILayout.PropertyField(areaOutList.FindPropertyRelative("Array.size"));
            for (int i = 0; i < areaOutList.arraySize; i++)
            {
                SerializedProperty areaOut = areaOutList.GetArrayElementAtIndex(i);
                EditorGUILayout.PropertyField(areaOut);
                EditorGUI.indentLevel += 1;
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(areaOut.FindPropertyRelative("position"));

                if (GUILayout.Button("Create Entry"))
                {
                    if (areaManager.transform.Find(string.Format("entry{0}", i)) == null)
                    {
                        GameObject entryGameObj = new GameObject(string.Format("entry{0}", i));
                        entryGameObj.transform.parent = areaManager.transform;
                        entryGameObj.transform.localPosition = Vector3.zero;
                        entryGameObj.transform.localScale = Vector3.one;
                        areaManager.areaOut[i].position = entryGameObj;
                    }
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.PropertyField(areaOut.FindPropertyRelative("direction"));
                EditorGUI.indentLevel -= 1;
            }
        }
        EditorGUI.indentLevel -= 1;
        serializedObject.ApplyModifiedProperties();
    }
}

#endif