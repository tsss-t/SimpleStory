#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;



[CustomEditor(typeof(AreaPrefabManager))]
public class AreaPrefabManagerEditor : Editor
{
    private AreaPrefabManager areaManager;
    public override void OnInspectorGUI()
    {
        areaManager = (AreaPrefabManager)target;

        if (areaManager.AreaOutGOList == null)
        {
            areaManager.AreaOutGOList = new AreaPrefabOutGO[0];
        }


        GUILayout.Label(string.Format("Input the Area's type and next Area's weight point"));
        areaManager.type= (UnitType)EditorGUILayout.EnumPopup("AreaType", areaManager.type);
        SerializedProperty basePoint = serializedObject.FindProperty("basePoint");
        EditorGUILayout.PropertyField(basePoint);

        if (basePoint.isExpanded)
        {
            EditorGUI.indentLevel += 1;
            areaManager.basePoint.roadPoint = EditorGUILayout.IntField("roadPoint", areaManager.basePoint.roadPoint);
            areaManager.basePoint.roomPoint = EditorGUILayout.IntField("roomPoint", areaManager.basePoint.roomPoint);
            areaManager.basePoint.endPoint = EditorGUILayout.IntField("endPoint", areaManager.basePoint.endPoint);
            areaManager.basePoint.cornerPoint = EditorGUILayout.IntField("cornerPoint", areaManager.basePoint.cornerPoint);
            EditorGUI.indentLevel -= 1;
        }




        GUILayout.Label(string.Format("Input the data in rota 0°,then put the button to compute other data"));

        serializedObject.Update();
        SerializedProperty areaOutGOList = serializedObject.FindProperty("AreaOutGOList");
        EditorGUILayout.PropertyField(areaOutGOList);
        if (areaOutGOList.isExpanded)
        {
            EditorGUI.indentLevel += 1;
            EditorGUILayout.PropertyField(areaOutGOList.FindPropertyRelative("Array.size"));
            for (int j = 0; j < areaOutGOList.arraySize; j++)
            {
                SerializedProperty areaOut = areaOutGOList.GetArrayElementAtIndex(j);
                EditorGUILayout.PropertyField(areaOut);
                if (areaOut.isExpanded)
                {
                    EditorGUI.indentLevel += 1;
                    if (GUILayout.Button("Create Entry", GUILayout.Width(300)))
                    {
                        if (areaManager.transform.Find(string.Format("entry{0}", j)) == null)
                        {
                            GameObject entryGameObj = new GameObject(string.Format("entry{0}", j));
                            entryGameObj.transform.parent = areaManager.transform;
                            entryGameObj.transform.localPosition = Vector3.zero;
                            entryGameObj.transform.localScale = Vector3.one;
                            areaManager.AreaOutGOList[j].location = entryGameObj;
                        }
                    }
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.PropertyField(areaOut.FindPropertyRelative("location"));
                    EditorGUILayout.PropertyField(areaOut.FindPropertyRelative("direction"));
                    EditorGUILayout.EndHorizontal();

                    EditorGUI.indentLevel -= 1;
                }

            }
            EditorGUI.indentLevel -= 1;

        }
        serializedObject.ApplyModifiedProperties();


        for (int angle = 0; angle < 360; angle += 90)
        {
            AreaPrefabInfo areaInfo = areaManager.AreaAngle0;
            switch (angle)
            {
                case 90:
                    {
                        areaInfo = areaManager.AreaAngle90;
                        break;
                    }
                case 180:
                    {
                        areaInfo = areaManager.AreaAngle180;
                        break;
                    }
                case 270:
                    {
                        areaInfo = areaManager.AreaAngle270;
                        break;
                    }
                default:
                    break;
            }


            if (angle == 90)
            {
                if (GUILayout.Button("Compute other location info"))
                {
                    ComputeRotInfo();
                }
            }
            serializedObject.Update();
            SerializedProperty areaInfoPro = serializedObject.FindProperty(string.Format("AreaAngle{0}", angle));
            EditorGUILayout.PropertyField(areaInfoPro);
            EditorGUI.indentLevel += 1;

            if (areaInfoPro.isExpanded)
            {
                GUILayout.Label(string.Format("This is the data after rota {0}", angle));
                areaInfo.height = EditorGUILayout.IntField("Height", areaInfo.height);
                areaInfo.width = EditorGUILayout.IntField("Width", areaInfo.width);
                areaInfo.centerPointUp = EditorGUILayout.Vector3Field("centerPointUp", new Vector3(-areaInfo.height / 2, 0, 0));
                areaInfo.centerPointDown = EditorGUILayout.Vector3Field("centerPointDown", new Vector3(areaInfo.height / 2, 0, 0));
                areaInfo.centerPointLeft = EditorGUILayout.Vector3Field("centerPointLeft", new Vector3(0, 0, -areaInfo.width / 2));
                areaInfo.centerPointRight = EditorGUILayout.Vector3Field("centerPointRight", new Vector3(0, 0, areaInfo.width / 2));
                SerializedProperty areaOutList = areaInfoPro.FindPropertyRelative("areaOut");

                EditorGUILayout.PropertyField(areaOutList);
                if (areaOutList.isExpanded)
                {
                    areaOutList.FindPropertyRelative("Array.size").intValue = areaManager.AreaOutGOList.Length;

                    EditorGUI.indentLevel += 1;
                    for (int j = 0; j < areaManager.AreaOutGOList.Length; j++)
                    {
                        SerializedProperty areaOut = areaOutList.GetArrayElementAtIndex(j);
                        EditorGUILayout.PropertyField(areaOut);
                        if (areaOut.isExpanded)
                        {
                            EditorGUI.indentLevel += 1;
                            EditorGUILayout.BeginHorizontal();
                            SerializedProperty postion = areaOut.FindPropertyRelative("position");
                            SerializedProperty direction = areaOut.FindPropertyRelative("direction");
                            if (angle == 0)
                            {
                                if (areaManager.AreaOutGOList[j].location.Equals(null) || areaManager.AreaOutGOList[j].direction.Equals(null))
                                {
                                    postion.vector3Value = Vector3.zero;
                                    direction.intValue = -1;
                                }
                                else
                                {
                                    postion.vector3Value = areaManager.AreaOutGOList[j].location.transform.localPosition;
                                    direction.intValue = (int)areaManager.AreaOutGOList[j].direction;
                                }
                            }
                            EditorGUILayout.LabelField(string.Format("Position:  x:{0} y:{1} z:{2}", postion.vector3Value.x, postion.vector3Value.y, postion.vector3Value.z));
                            EditorGUILayout.LabelField(string.Format("Direction: {0} ", (OutDirection)direction.intValue));
                            EditorGUILayout.EndHorizontal();
                            EditorGUI.indentLevel -= 1;
                        }
                    }
                    EditorGUI.indentLevel -= 1;
                }
            }
            EditorGUI.indentLevel -= 1;
            serializedObject.ApplyModifiedProperties();
        }
        //int pix = 10;
        //GUILayout.Label(string.Format("World : Map(virtual) = {0} : 1", pix));
    }
    public void ComputeRotInfo()
    {
        //height
        areaManager.AreaAngle90.height = areaManager.AreaAngle0.width;
        areaManager.AreaAngle180.height = areaManager.AreaAngle0.height;
        areaManager.AreaAngle270.height = areaManager.AreaAngle0.width;
        //width
        areaManager.AreaAngle90.width = areaManager.AreaAngle0.height;
        areaManager.AreaAngle180.width = areaManager.AreaAngle0.width;
        areaManager.AreaAngle270.width = areaManager.AreaAngle0.height;

        for (int j = 0; j < areaManager.AreaOutGOList.Length; j++)
        {
            areaManager.AreaAngle90.areaOut[j] = areaManager.AreaAngle0.areaOut[j].Rot(AngleFix.Angle90);
            areaManager.AreaAngle180.areaOut[j] = areaManager.AreaAngle90.areaOut[j].Rot(AngleFix.Angle90);
            areaManager.AreaAngle270.areaOut[j] = areaManager.AreaAngle180.areaOut[j].Rot(AngleFix.Angle90);
        }
    }
}

#endif