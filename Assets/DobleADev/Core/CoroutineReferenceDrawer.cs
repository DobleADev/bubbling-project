using UnityEngine;
using System.Collections;
using System.Reflection;
using System;

using UnityEditor;

[System.Serializable]
public struct CoroutineReference
{
    public GameObject gameObject;
    public string componentName;
    public string coroutineName;
    public string[] parameterNames;
}

[CustomPropertyDrawer(typeof(CoroutineReference))]
public class CoroutineReferenceDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Obtener las propiedades serializadas
        SerializedProperty gameObjectProp = property.FindPropertyRelative("gameObject");
        SerializedProperty componentNameProp = property.FindPropertyRelative("componentName");
        SerializedProperty coroutineNameProp = property.FindPropertyRelative("coroutineName");
        SerializedProperty parameterNamesProp = property.FindPropertyRelative("parameterNames");

        // Dibujar el campo para seleccionar el GameObject
        gameObjectProp.objectReferenceValue = (GameObject) EditorGUI.ObjectField(position, new GUIContent("GameObject"), gameObjectProp.objectReferenceValue, typeof(GameObject), true);
        // Debug.Log(gameObjectProp.objectReferenceValue);
        position.y += EditorGUIUtility.singleLineHeight + 2;

        // Obtener el GameObject
        GameObject gameObjectReference = gameObjectProp.objectReferenceValue as GameObject;

        // Si hay un GameObject seleccionado, mostrar el popup para seleccionar el componente
        if (gameObjectReference == null)
        {
            return;
        }
        
        // Obtener los componentes del GameObject
        Component[] components = gameObjectReference.GetComponents<Component>();
        string[] componentNames = new string[components.Length+1];
        componentNames[0] = "";
        for (int i = 1; i < components.Length; i++)
        {
            componentNames[i] = components[i].GetType().Name;
        }
        Debug.Log(gameObjectReference.name + " has " + (components.Length-1) + " components");

        // Dibujar el popup para seleccionar el componente
        int selectedComponentIndex = Array.IndexOf(componentNames, componentNameProp.stringValue);
        selectedComponentIndex = EditorGUI.Popup(position, "Componente", selectedComponentIndex, componentNames);
        componentNameProp.stringValue = componentNames[selectedComponentIndex];
        position.y += EditorGUIUtility.singleLineHeight + 2;

        if (selectedComponentIndex == 0)
        {
            return;
        }

        // Obtener el componente seleccionado
        Component component = gameObjectReference.GetComponent(componentNames[selectedComponentIndex]);

        // Si hay un componente seleccionado, mostrar el popup para seleccionar la corutina
        if (component != null)
        {
            // ... (Código para obtener las corutinas del componente y mostrar el popup)
        }


        
    }
}