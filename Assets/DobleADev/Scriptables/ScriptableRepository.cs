using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using DobleADev.Core;
using UnityEngine;
using Newtonsoft.Json;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DobleADev.Scriptables
{

    [CreateAssetMenu(fileName = "NewScriptableRepository", menuName = "DobleADev/Scriptables Repository")]
    public class ScriptableRepository : ScriptableObject
    {
        [SerializeField] InspectorButton saveButton;
        [SerializeField] InspectorButton loadButton;
        [SerializeField] InspectorButton resetButton;
        [SerializeField] InspectorButton deleteButton;
        [SerializeField] private string _relativePath = "repository"; // Ruta relativa
        [SerializeField] private bool _usePersistentDataPath = true; // Usar PersistentDataPath por defecto
        [SerializeField] private List<ScriptableVariableBase> _variables = new List<ScriptableVariableBase>();

        private void OnEnable()
        {
            saveButton = new InspectorButton("Save to file", Save);
            loadButton = new InspectorButton("Load from file", Load);
            resetButton = new InspectorButton("Reset variables Data", Reset);
            deleteButton = new InspectorButton("Delete file", Delete);

        }

        [Serializable]
        public class VariableData
        {
            public string Name;
            public string Value;
        }

        public void AddVariable(string name, ScriptableVariableBase variable)
        {
            if (!_variables.Exists(x => x.name == name))
            {
                _variables.Add(variable);
            }
        }

        public T GetVariable<T>(string name) where T : ScriptableVariableBase
        {
            foreach (var variable in _variables)
            {
                if (variable.name == name && variable is T)
                {
                    return (T)variable;
                }
            }
            Debug.LogWarning($"No se encontró una variable con la clave '{name}' del tipo {typeof(T).Name}.");
            return null;
        }

        public ScriptableVariableBase GetVariable(string name)
        {
            foreach (var variable in _variables)
            {
                if (variable.name == name)
                {
                    return variable;
                }
            }
            Debug.LogWarning($"No se encontró una variable con la clave '{name}'.");
            return null;
        }

        private string GetFilePath()
        {
            string fileName = Path.GetFileName(_relativePath);
            string directoryPath = Path.GetDirectoryName(_relativePath);
            string path;

            if (_usePersistentDataPath)
            {
                path = Path.Combine(Application.persistentDataPath, directoryPath, fileName + ".json");
            }
            else
            {
                path = Path.Combine(Application.dataPath, directoryPath, fileName + ".json");
            }

            return path;
        }

        public void Save()
        {
            string path = GetFilePath();

            List<VariableData> dataToSave = new List<VariableData>();
            foreach (var variable in _variables)
            {
                dataToSave.Add(new VariableData()
                {
                    Name = variable.name,
                    Value = Newtonsoft.Json.JsonConvert.SerializeObject(variable.GetValue(), Newtonsoft.Json.Formatting.Indented)
                });
            }

            string json = Newtonsoft.Json.JsonConvert.SerializeObject(new SerializationWrapper<VariableData>(dataToSave), Newtonsoft.Json.Formatting.Indented);

            string directoryPath = Path.GetDirectoryName(path);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            File.WriteAllText(path, json);

#if UNITY_EDITOR
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
#endif
            Debug.Log("Datos guardados en " + path);
        }

        public void Load()
        {
            string path = GetFilePath();

            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                try
                {
                    var wrapper = Newtonsoft.Json.JsonConvert.DeserializeObject<SerializationWrapper<VariableData>>(json);
                    if (wrapper != null && wrapper.Items != null)
                    {
                        foreach (var data in wrapper.Items)
                        {
                            var variable = _variables.Find(x => x.name == data.Name);
                            if (variable != null)
                            {
                                try
                                {
                                    variable.SetValue(Newtonsoft.Json.JsonConvert.DeserializeObject(data.Value));
                                }
                                catch (Newtonsoft.Json.JsonSerializationException e)
                                {
                                    Debug.LogError($"Error de serialización JSON al cargar la variable {data.Name}. JSON: {data.Value}. Error: {e.Message}");
                                }
                                catch (Exception e)
                                {
                                    Debug.LogError($"Error al cargar la variable {data.Name}. Error: {e.Message}");
                                }
                            }
                            else
                            {
                                Debug.LogWarning($"No se encontro la variable {data.Name} en la lista de variables del repositorio.");
                            }
                        }
                    }
                }
                catch (Newtonsoft.Json.JsonReaderException e)
                {
                    Debug.LogError($"Error al leer el JSON: {e.Message}. Archivo: {path}");
                }
                catch (Newtonsoft.Json.JsonSerializationException e)
                {
                    Debug.LogError($"Error al deserializar el JSON: {e.Message}. Archivo: {path}");
                }
                Debug.Log("Datos cargados desde " + path);
            }
            else
            {
                Debug.LogWarning("No se encontró el archivo de guardado en " + path);
            }
        }

        public void Delete()
        {
            string path = GetFilePath();

            if (File.Exists(path))
            {
                File.Delete(path);
#if UNITY_EDITOR
                AssetDatabase.Refresh();//Refrescar la base de datos de assets
#endif
                Debug.Log("Archivo de guardado eliminado: " + path);
            }
            else
            {
                Debug.LogWarning("No se encontró el archivo para eliminar: " + path);
            }
        }

        public void Reset()
        {
            foreach (var variable in _variables)
            {
                //Obtener el tipo de la variable
                var type = variable.GetVariableType();
                //Crear una instancia del tipo de la variable
                var instance = Activator.CreateInstance(type);
                //Asignar el valor por defecto a la variable
                variable.SetValue(instance);

#if UNITY_EDITOR
                EditorUtility.SetDirty(variable);
#endif
            }
#if UNITY_EDITOR
            AssetDatabase.SaveAssets();
#endif
            Debug.Log("Variables reseteadas.");
        }
    }

    //Clase necesaria para serializar listas en unity
    [Serializable]
    public class SerializationWrapper<T>
    {
        public List<T> Items;

        public SerializationWrapper(List<T> items)
        {
            Items = items;
        }

        public SerializationWrapper() { }
    }
}