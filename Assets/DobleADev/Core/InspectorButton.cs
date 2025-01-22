using System;
using UnityEngine;
using System.Reflection;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DobleADev.Core
{
    [Serializable]
    public class InspectorButton
    {
        public string ButtonText;
        private Action _onClick;

        public InspectorButton(string buttonText, Action onClick)
        {
            ButtonText = buttonText;
            _onClick = onClick;
        }

        public void OnClick()
        {
            _onClick?.Invoke();
        }
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(InspectorButton))]
    public class InspectorButtonDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty buttonTextProp = property.FindPropertyRelative("ButtonText");

            // Calcular la altura del botón
            float buttonHeight = EditorGUIUtility.singleLineHeight;

            // Dibujar el botón
            if (GUI.Button(new Rect(position.x, position.y, position.width, buttonHeight), buttonTextProp.stringValue))
            {
                // Obtener el objeto real de la propiedad serializada
                var targetObject = property.serializedObject.targetObject;
                // Obtener el campo InspectorButton que se esta dibujando
                var fieldInfo = targetObject.GetType().GetField(property.name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (fieldInfo != null)
                {
                    //Obtener el valor del campo
                    var inspectorButton = fieldInfo.GetValue(targetObject) as InspectorButton;
                    //Invocar el metodo onClick
                    inspectorButton?.OnClick();
                }
                else
                {
                    Debug.LogError("No se encontro el campo " + property.name);
                }
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight;
        }
    }
#endif
}