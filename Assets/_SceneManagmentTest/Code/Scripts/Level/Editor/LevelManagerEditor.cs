using System;
using System.Text;
using DobleADev.Core;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelManager))]
public class LevelManagerEditor : Editor
{
    SerializedProperty areaProperty;
    SerializedProperty roomProperty;

    private void OnEnable()
    {
        areaProperty = serializedObject.FindProperty("_area");
        roomProperty = serializedObject.FindProperty("_room");
    }

    String ShowSceneDetails(SceneReference sceneReference)
    {
        if (sceneReference.sceneName.Length == 0) return "none";
        string buildIndex = sceneReference.buildIndex == -1 ? "[not in build]" : $"[id: {sceneReference.buildIndex}]";
        string assetPath = sceneReference.assetPath == string.Empty ? "not found" : sceneReference.assetPath;
        return sceneReference.sceneName + " " + buildIndex + " - " + assetPath;
    }

    // public override void OnInspectorGUI()
    // {
    //     serializedObject.Update();

    //     StringBuilder message = new StringBuilder();

    //     SceneReference area = (SceneReference)areaProperty.objectReferenceValue;
    //     SceneReference room = (SceneReference)roomProperty.objectReferenceValue;

    //     message.Append("Last area: " + ShowSceneDetails(area));
    //     message.Append("\n");
    //     message.Append("Last room: " + ShowSceneDetails(room));

    //     EditorGUILayout.HelpBox(message.ToString(), MessageType.Info);

    //     serializedObject.ApplyModifiedProperties();
    // }
}
