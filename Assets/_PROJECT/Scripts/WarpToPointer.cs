using UnityEngine;
using UnityEngine.EventSystems;

public class WarpToPointer : MonoBehaviour
{
    [SerializeField] Camera _camera;
    public void Warp(BaseEventData eventData)
    {
        Warp((PointerEventData) eventData);
    }

    public void Warp(PointerEventData eventData)
    {
        if (_camera == null)
        {
            _camera = Camera.main;
            if (_camera == null)
            {
                Debug.Log("No compatible camera found");
                return;
            }
        }
        // Debug.Log(eventData.position);
        Vector2 finalPosition = _camera.ViewportToWorldPoint(eventData.position / new Vector2(Screen.width, Screen.height));
        transform.position = new Vector3(finalPosition.x, finalPosition.y, transform.position.z);
    }
}
