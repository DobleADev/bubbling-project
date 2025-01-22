using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Joystick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
	[SerializeField] float _radius = 10;
	[SerializeField] Image _knob;
	[SerializeField, Range(0, 1)] float _deadZone;
	[SerializeField] Vector2Event _onValueChange;
	Vector2 _lastPosition;
	PointerEventData knobData;

    // Use this for initialization
    void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (knobData == null) return;

		EvaluateInput(knobData.position);
		// Debug.Log(knobData.position);
	}

	public void OnPointerDown(PointerEventData eventData)
    {
		knobData = eventData;
        EvaluateInput(knobData.position);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        knobData = null;
		EvaluateInput(transform.position);
    }

	void EvaluateInput(Vector2 input)
	{
		input = (Vector2)transform.position + Vector2.ClampMagnitude(input - (Vector2)transform.position, _radius);
		_knob.transform.position = input;
		if (_onValueChange != null) _onValueChange.Invoke((input - (Vector2)transform.position) / _radius);
	}

}

[System.Serializable]
class Vector2Event : UnityEvent<Vector2> {}