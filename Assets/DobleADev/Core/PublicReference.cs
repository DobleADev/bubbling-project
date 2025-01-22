using System;
using UnityEngine;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DobleADev.Core
{
    [Serializable]
    public class PublicReference
    {
        public UnityEngine.Object Objeto; // Ahora almacena UnityEngine.Object
        public string NombreCampo;
        [SerializeField] private string tipoCampo;
        [SerializeField] private string valorSerializado;

        public object Valor
        {
            get
            {
                if (Objeto == null || string.IsNullOrEmpty(NombreCampo))
                {
                    return null;
                }

                Type objetoType = Objeto.GetType();
                FieldInfo campo = objetoType.GetField(NombreCampo);

                if (campo != null)
                {
                    if (!string.IsNullOrEmpty(valorSerializado) && !string.IsNullOrEmpty(tipoCampo))
                    {
                        var type = Type.GetType(tipoCampo);
                        if (type != null)
                        {
                            try
                            {
                                return JsonUtility.FromJson(valorSerializado, type);
                            }
                            catch (System.ArgumentException e)
                            {
                                Debug.LogError($"Error al deserializar {NombreCampo} en {Objeto.name}. Tipo: {tipoCampo}. JSON: {valorSerializado}. Error: {e.Message}");
                            }
                        }
                    }
                    return campo.GetValue(Objeto);
                }
                else
                {
                    Debug.LogError($"Campo '{NombreCampo}' no encontrado en {Objeto.name}");
                    return null;
                }
            }
            set
            {
                if (Objeto != null && !string.IsNullOrEmpty(NombreCampo))
                {
                    Type objetoType = Objeto.GetType();
                    FieldInfo campo = objetoType.GetField(NombreCampo);
                    if (campo != null)
                    {
                        campo.SetValue(Objeto, value);
                        tipoCampo = campo.FieldType.AssemblyQualifiedName;
                        valorSerializado = JsonUtility.ToJson(value);
                    }
                }
            }
        }
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(PublicReference))]
    public class PublicReferenceDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var objetoProp = property.FindPropertyRelative("Objeto");
            var nombreCampoProp = property.FindPropertyRelative("NombreCampo");
            var tipoCampoProp = property.FindPropertyRelative("tipoCampo");

            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            Rect objetoRect = new Rect(position.x, position.y, position.width / 2, position.height);
            Rect campoRect = new Rect(position.x + position.width / 2, position.y, position.width / 2, position.height);

            EditorGUI.PropertyField(objetoRect, objetoProp, GUIContent.none);

            UnityEngine.Object objeto = objetoProp.objectReferenceValue;
            List<string> nombresMiembros = new List<string>(); // Cambiado a nombresMiembros
            int selectedIndex = -1;
            string tipoRequerido = null;

            var fieldInfo = property.serializedObject.targetObject.GetType().GetField(property.name);
            if (fieldInfo != null)
            {
                tipoRequerido = fieldInfo.FieldType.GetGenericArguments().Length > 0 ? fieldInfo.FieldType.GetGenericArguments()[0].AssemblyQualifiedName : null;
            }

            if (objeto != null)
            {
                BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

                // Obtener campos públicos
                var camposPublicos = objeto.GetType().GetFields(flags).Where(f => f.IsPublic);

                // Obtener propiedades públicas con get y set
                var propiedadesPublicas = objeto.GetType().GetProperties(flags)
                    .Where(p => p.GetGetMethod() != null && p.GetSetMethod() != null);

                // Combinar campos y propiedades en una sola lista
                nombresMiembros.AddRange(camposPublicos.Select(f => f.Name));
                nombresMiembros.AddRange(propiedadesPublicas.Select(p => p.Name));

                //Obtener propiedades serializadas usando SerializedObject
                SerializedObject serializedObject = new SerializedObject(objeto);
                SerializedProperty iterator = serializedObject.GetIterator();
                List<string> serializedFields = new List<string>();
                while (iterator.NextVisible(true))
                {
                    if (iterator.name == "m_Script") continue;
                    serializedFields.Add(iterator.name);
                }

                // Filtrar los campos y propiedades por los que estan serializados
                nombresMiembros = nombresMiembros.Where(f => serializedFields.Contains(f)).ToList();

                //Filtrar por tipo requerido
                nombresMiembros = nombresMiembros.Where(f =>
                {
                    var field = objeto.GetType().GetField(f, flags);
                    if (field != null)
                        return tipoRequerido == null || field.FieldType.AssemblyQualifiedName == tipoRequerido;

                    var propertyInfo = objeto.GetType().GetProperty(f, flags);
                    if (propertyInfo != null)
                        return tipoRequerido == null || propertyInfo.PropertyType.AssemblyQualifiedName == tipoRequerido;

                    return false;
                }).ToList();

                if (!string.IsNullOrEmpty(nombreCampoProp.stringValue))
                {
                    selectedIndex = nombresMiembros.IndexOf(nombreCampoProp.stringValue);
                }

                if (nombresMiembros.Count == 0 && tipoRequerido != null)
                {
                    var type = Type.GetType(tipoRequerido);
                    if (type != null)
                        EditorGUI.HelpBox(campoRect, $"No se encontraron miembros publicos serializados del tipo {type.Name} con get y set en {objeto.name}", MessageType.Warning);
                }
            }

            selectedIndex = EditorGUI.Popup(campoRect, selectedIndex, nombresMiembros.ToArray());

            if (selectedIndex >= 0 && selectedIndex < nombresMiembros.Count)
            {
                nombreCampoProp.stringValue = nombresMiembros[selectedIndex];
                if (objeto != null)
                {
                    var field = objeto.GetType().GetField(nombresMiembros[selectedIndex], BindingFlags.Instance | BindingFlags.Public);
                    if (field != null)
                        tipoCampoProp.stringValue = field.FieldType.AssemblyQualifiedName;
                    else
                    {
                        var propertyInfo = objeto.GetType().GetProperty(nombresMiembros[selectedIndex], BindingFlags.Instance | BindingFlags.Public);
                        if (propertyInfo != null)
                            tipoCampoProp.stringValue = propertyInfo.PropertyType.AssemblyQualifiedName;
                    }
                }
            }
            else
            {
                nombreCampoProp.stringValue = null;
                tipoCampoProp.stringValue = null;
            }

            EditorGUI.EndProperty();
        }
    }
#endif
}