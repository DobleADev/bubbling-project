using UnityEngine;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DobleADev.Core
{
    [System.Serializable]
    public struct SceneReference
    {
        [SerializeField] Object _container;
        [SerializeField] string _sceneName;
        [SerializeField] int _buildIndex;
        [SerializeField] string _assetPath;

        public string sceneName { get => _sceneName; set => _sceneName = value; }
        public int buildIndex { get => _buildIndex; set => _buildIndex = value; }
        public string assetPath { get => _assetPath; set => _assetPath = value; }
        public SceneReference(int initialBuildIndex = -1)
        {
            _container = null;
            _sceneName = "";
            _buildIndex = initialBuildIndex;
            _assetPath = "";
        }

        public void SetDataFromOther(SceneReference other)
        {
            _container = other._container;
            _sceneName = other._sceneName;
            _buildIndex = other._buildIndex;
            _assetPath = other._assetPath;
        }
    }

#if UNITY_EDITOR

    [CustomPropertyDrawer(typeof(SceneReference))]
    public class SceneReferenceEditor : PropertyDrawer
    {
        private float fieldHeight = EditorGUIUtility.singleLineHeight;
        private float warningHeight = EditorGUIUtility.singleLineHeight;
        private float spacing = EditorGUIUtility.standardVerticalSpacing;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            SerializedProperty buildIndexProperty = property.FindPropertyRelative("_buildIndex");
            return fieldHeight + (buildIndexProperty.intValue == -1 ? warningHeight + spacing : 0);
            // return EditorGUIUtility.singleLineHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty containerProperty = property.FindPropertyRelative("_container");
            SerializedProperty sceneNameProperty = property.FindPropertyRelative("_sceneName");
            SerializedProperty buildIndexProperty = property.FindPropertyRelative("_buildIndex");
            SerializedProperty assetPathProperty = property.FindPropertyRelative("_assetPath");

            EditorGUI.BeginProperty(position, label, property);
            Rect fieldRect = new Rect(position);
            fieldRect.height = fieldHeight;
            containerProperty.objectReferenceValue = (SceneAsset)EditorGUI.ObjectField(fieldRect, containerProperty.objectReferenceValue, typeof(SceneAsset), false);

            // Actualizar los demás campos si el Scene Asset ha cambiado
            if (containerProperty.objectReferenceValue != null)
            {
                SceneAsset sceneAsset = (SceneAsset)containerProperty.objectReferenceValue;
                sceneNameProperty.stringValue = sceneAsset.name;
                assetPathProperty.stringValue = AssetDatabase.GetAssetPath(sceneAsset);
                buildIndexProperty.intValue = EditorBuildSettings.scenes.ToList().FindIndex(s => s.path == assetPathProperty.stringValue);
            }
            else
            {
                sceneNameProperty.stringValue = "";
                buildIndexProperty.intValue = -1;
                assetPathProperty.stringValue = "";
            }

            if (containerProperty.objectReferenceValue != null)
            {
                Rect warningPosition = new Rect(position);
                warningPosition.y += fieldHeight + spacing;
                warningPosition.height = warningHeight;
                if (buildIndexProperty.intValue == -1)
                {
                    EditorGUI.HelpBox(warningPosition, "Not included in build", MessageType.Warning);
                }
            }

            EditorGUI.EndProperty();
        }

    }
#endif
}