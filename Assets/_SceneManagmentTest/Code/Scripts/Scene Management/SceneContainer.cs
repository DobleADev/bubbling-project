using UnityEngine;
using DobleADev.Core;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

[CreateAssetMenu(fileName = "NewSceneContainer", menuName = "Scriptable Object/Scene Container")]
public class SceneContainer : ScriptableObject
{
    [SerializeField] SceneReference[] _content;

    public SceneReference[] content { get { return _content; } set { _content = value;} }
    public void SetContentAsScene(SceneReference scene) {_content = new SceneReference[1]{scene}; }
    public void SetDataFromOther(SceneContainer other)
    {
        if (other == null)
        {
            _content = new SceneReference[0];
            return;
        }
        _content = other._content;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(SceneContainer))]
public class SceneContainerInspector : Editor 
{
    internal void OnSceneDrag(SceneView sceneView) {
       
        Event e = Event.current;

        // Si el evento es DragUpdated (actualización del arrastre)
        if (e.type == EventType.DragUpdated)
        {
            if (DragAndDrop.objectReferences.Length > 0)
            {
                // Verificamos si el objeto arrastrado es de tipo SceneContainer
                if (DragAndDrop.objectReferences[0] is SceneContainer)
                {
                    DragAndDrop.visualMode = DragAndDropVisualMode.Link;  // Mostramos el modo de enlace
                }
                else
                {
                    DragAndDrop.visualMode = DragAndDropVisualMode.Rejected;  // Si no es, rechazamos el arrastre
                }
            }
            e.Use();  // Usamos el evento para evitar que se propague más
        }

        // Si el evento es DragPerform (cuando el arrastre se completa)
        else if (e.type == EventType.DragPerform)
        {
            if (DragAndDrop.objectReferences.Length > 0)
            {
                foreach (var draggedObject in DragAndDrop.objectReferences)
                {
                    if (draggedObject is SceneContainer sceneContainer)
                    {
                        // Acción cuando se realiza el arrastre exitoso
                        // Debug.Log("SceneContainer arrastrado: " + sceneContainer.name);
                        foreach (var scene in sceneContainer.content)
                        {
                            if (scene.assetPath == "" || scene.sceneName == "") continue;
                            EditorSceneManager.OpenScene(scene.assetPath, OpenSceneMode.Additive);
                        }
                        // Realizar alguna acción, como cargar una escena (ejemplo):
                        // SceneManager.LoadScene(sceneContainer.sceneName);
                    }
                }
            }
            DragAndDrop.AcceptDrag();  // Aceptamos el arrastre
            e.Use();  // Usamos el evento para evitar que se propague más
        }

        // Usamos HandleUtility.PickGameObject para detectar sobre qué parte de la jerarquía estamos
        // if (e.type == EventType.MouseMove || e.type == EventType.MouseDrag)
        // {
        //     var go = HandleUtility.PickGameObject(e.mousePosition, false);
        //     if (go != null)
        //     {
        //         Debug.Log("Arrastre sobre GameObject: " + go.name);
        //     }
        // }
    }
 


}
#endif