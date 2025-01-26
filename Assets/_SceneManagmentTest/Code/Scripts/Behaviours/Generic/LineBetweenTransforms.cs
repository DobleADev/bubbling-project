using UnityEngine;

public class LineBetweenTransforms : MonoBehaviour
{
    [SerializeField] float width;
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] Transform start;
    [SerializeField] Transform end;
    public void SetStartTransform(Transform start) { this.start = start; }
    public void SetStartTransform(GameObject start) { this.start = start.transform; }
    public void SetEndTransform(Transform end) { this.end = end; }
    public void SetEndTransform(GameObject end) { this.end = end.transform; }

    void Update()
    {
        if (start == null || end == null) return;
        Vector3[] positions = { start.position, end.position };
        lineRenderer.SetPositions(positions);
        lineRenderer.startWidth = width;
        lineRenderer.endWidth = width;

    }
}
