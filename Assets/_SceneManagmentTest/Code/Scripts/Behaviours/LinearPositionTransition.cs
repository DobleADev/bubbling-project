using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class LinearPositionTransition : MonoBehaviour
{
    [SerializeField] float duration = 1f;
    [SerializeField] ScriptableTimeScale deltaTimeMultiplier;
    [SerializeField] Transform startPosition;
    [SerializeField] Transform endPosition;
    [SerializeField] UnityEvent onStartTransition;
    [SerializeField] UnityEvent onEndTransition;

    public void StartTransition(Transform other)
    {
        if (startPosition != null && endPosition != null)
        {
            StartCoroutine(TransitionCoroutine(other, startPosition, endPosition));
        }
    }

    IEnumerator TransitionCoroutine(Transform target, Transform start, Transform end)
    {
        RoomTransitionManager.instance.isPlaying = true;
        onStartTransition?.Invoke();
        RoomTransitionManager.instance.StartTransition();
        if (duration > 0)
        {
            target.position = start.position;
            float t = 0;
            float deltaDuration = 1 / duration;
            Vector3 startPosition = start.position;
            Vector3 endPosition = end.position;
            while (t < 1)
            {
                target.position = Vector3.Lerp(startPosition, endPosition, t);
                t += deltaDuration * Mathf.Min(Time.unscaledDeltaTime, Time.maximumDeltaTime) 
                * (deltaTimeMultiplier ? deltaTimeMultiplier.Multiplier : 1);
                yield return null;
            }
        }
        target.position = end.position;
        RoomTransitionManager.instance.isPlaying = false;
        onEndTransition?.Invoke();
        RoomTransitionManager.instance.EndTransition();
    }

    private void OnDrawGizmos() 
    {
        int positionsPresent = 0;
        if (startPosition != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(startPosition.position, 0.5f);
            positionsPresent++;
        }

        if (endPosition != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(endPosition.position, 0.5f);
            positionsPresent++;
        }
        
        if (positionsPresent == 2)
        {
            Gizmos.color = Color.gray;
            Gizmos.DrawLine(startPosition.position, endPosition.position);
        }
    }
}
