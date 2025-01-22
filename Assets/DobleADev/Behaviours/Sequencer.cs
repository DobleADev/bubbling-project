using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;


#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif

public class Sequencer : MonoBehaviour
{
    public List<SequenceAction> sequenceItems;

    public void StartSequence()
    {
        StartCoroutine(PlaySequence());
    }

    private IEnumerator PlaySequence()
    {
        foreach (var item in sequenceItems)
        {
            // Obtener el componente por nombre
            Component component = item.gameObject.GetComponent(item.componentName);

            // Obtener el método de la corutina por reflexión
            MethodInfo methodInfo = component.GetType().GetMethod(item.coroutineName);

            // Invocar la corutina
            StartCoroutine((IEnumerator)methodInfo.Invoke(component, null));

            yield return null;
        }
    }
}

[Serializable]
public class SequenceAction
{
    public GameObject gameObject;
    public string componentName;
    public string coroutineName;
}

#if UNITY_EDITOR
[CustomEditor(typeof(Sequencer))]
public class SequencePlayerEditor : Editor
{
    private ReorderableList sequenceList;

    public override void OnInspectorGUI()
    {
        Sequencer myScript = (Sequencer)target;

        sequenceList = new ReorderableList(myScript.sequenceItems, typeof(SequenceAction), true, true, true, true);
        sequenceList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            var sequenceItem = myScript.sequenceItems[index];

            // Usamos BeginVertical y EndVertical para agrupar los campos
            EditorGUILayout.BeginVertical(GUILayout.Width(rect.width));
            EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, EditorGUI.GetPropertyHeight(serializedObject.FindProperty("sequenceItems").GetArrayElementAtIndex(index).FindPropertyRelative("gameObject"), true)),
                                    serializedObject.FindProperty("sequenceItems").GetArrayElementAtIndex(index).FindPropertyRelative("gameObject"));
            EditorGUI.PropertyField(new Rect(rect.x, rect.y + EditorGUI.GetPropertyHeight(serializedObject.FindProperty("sequenceItems").GetArrayElementAtIndex(index).FindPropertyRelative("gameObject"), true), rect.width, EditorGUI.GetPropertyHeight(serializedObject.FindProperty("sequenceItems").GetArrayElementAtIndex(index).FindPropertyRelative("componentName"), true)),
                                    serializedObject.FindProperty("sequenceItems").GetArrayElementAtIndex(index).FindPropertyRelative("componentName"));
            EditorGUI.PropertyField(new Rect(rect.x, rect.y + EditorGUI.GetPropertyHeight(serializedObject.FindProperty("sequenceItems").GetArrayElementAtIndex(index).FindPropertyRelative("gameObject"), true) + EditorGUI.GetPropertyHeight(serializedObject.FindProperty("sequenceItems").GetArrayElementAtIndex(index).FindPropertyRelative("componentName"), true), rect.width, EditorGUI.GetPropertyHeight(serializedObject.FindProperty("sequenceItems").GetArrayElementAtIndex(index).FindPropertyRelative("coroutineName"), true)),
                                    serializedObject.FindProperty("sequenceItems").GetArrayElementAtIndex(index).FindPropertyRelative("coroutineName"));
            EditorGUILayout.EndVertical();
        };

        sequenceList.drawHeaderCallback = (Rect rect) =>
        {
            EditorGUI.LabelField(rect, "Sequence Items");
        };

        sequenceList.DoLayoutList();

        
    }
}
#endif