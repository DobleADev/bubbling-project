using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Reflection;
using System.Linq;
using System;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif

namespace DobleADev.Core
{
    public class UnityEventRelay : MonoBehaviour
    {
        [Serializable]
        public class RelayEvent
        {
            public Component TargetComponent;
            public string EventName;
            public UnityEvent ResponseEvent;

            private Delegate _eventDelegate;
            private EventInfo _eventInfo;

            public void Subscribe()
            {
                if (TargetComponent == null || string.IsNullOrEmpty(EventName) || ResponseEvent == null) return;

                _eventInfo = TargetComponent.GetType().GetEvent(EventName);
                if (_eventInfo == null)
                {
                    Debug.LogError($"Evento '{EventName}' no encontrado en {TargetComponent.GetType().Name}");
                    return;
                }

                // Crear un delegado dinámico que invoque el ResponseEvent
                _eventDelegate = Delegate.CreateDelegate(_eventInfo.EventHandlerType, this, nameof(InvokeResponseEvent));

                _eventInfo.AddEventHandler(TargetComponent, _eventDelegate);
            }

            public void Unsubscribe()
            {
                if (_eventInfo != null && _eventDelegate != null && TargetComponent != null)
                {
                    _eventInfo.RemoveEventHandler(TargetComponent, _eventDelegate);
                    _eventDelegate = null;
                    _eventInfo = null;
                }
            }

            private void InvokeResponseEvent(params object[] args)
            {
                ResponseEvent?.Invoke();
            }
        }

        public List<RelayEvent> EventsToRelay = new List<RelayEvent>();

        private void OnEnable()
        {
            foreach (var relayEvent in EventsToRelay)
            {
                relayEvent.Subscribe();
            }
        }

        private void OnDisable()
        {
            foreach (var relayEvent in EventsToRelay)
            {
                relayEvent.Unsubscribe();
            }
        }
    }

#if UNITY_EDITOR
[CustomEditor(typeof(UnityEventRelay))]
public class UnityEventRelayEditor : Editor
{
    private ReorderableList _reorderableList;

    private void OnEnable()
    {
        _reorderableList = new ReorderableList(serializedObject, serializedObject.FindProperty("EventsToRelay"), true, true, true, true);

        _reorderableList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            var element = _reorderableList.serializedProperty.GetArrayElementAtIndex(index);
            rect.y += 2;

            EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width / 2, EditorGUIUtility.singleLineHeight), element.FindPropertyRelative("TargetComponent"), GUIContent.none);

            var targetComponentProp = element.FindPropertyRelative("TargetComponent");
            var eventNameProp = element.FindPropertyRelative("EventName");
            var responseEventProp = element.FindPropertyRelative("ResponseEvent");

            Component targetComponent = targetComponentProp.objectReferenceValue as Component;
            string[] eventNames = new string[0];
            int selectedIndex = -1;

            if (targetComponent != null)
            {
                // Incluir BindingFlags.NonPublic para eventos privados
                BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

                // Obtener eventos que son de tipo UnityEvent
                eventNames = targetComponent.GetType().GetEvents(flags)
                    .Where(e => e.EventHandlerType.Name == "UnityAction")
                    .Select(e => e.Name).ToArray();

                selectedIndex = Array.IndexOf(eventNames, eventNameProp.stringValue);
            }

            selectedIndex = EditorGUI.Popup(new Rect(rect.x + rect.width / 2, rect.y, rect.width / 2, EditorGUIUtility.singleLineHeight), selectedIndex, eventNames);

            if (selectedIndex >= 0 && selectedIndex < eventNames.Length)
            {
                eventNameProp.stringValue = eventNames[selectedIndex];
            }
            else
            {
                eventNameProp.stringValue = null;
            }

            // Calcular la altura correcta para el siguiente campo
            rect.y += EditorGUIUtility.singleLineHeight + 2;
            EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), responseEventProp);
        };

        _reorderableList.elementHeightCallback = (int index) =>
        {
            return EditorGUIUtility.singleLineHeight * 2 + 6;
        };

        _reorderableList.drawHeaderCallback = (Rect rect) =>
        {
            EditorGUI.LabelField(rect, "Events to Relay");
        };
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        _reorderableList.DoLayoutList();
        serializedObject.ApplyModifiedProperties();
    }
}
#endif
}