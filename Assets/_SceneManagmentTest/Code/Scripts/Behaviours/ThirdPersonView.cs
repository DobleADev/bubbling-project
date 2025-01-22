using UnityEngine;

[ExecuteInEditMode]
public class ThirdPersonView : MonoBehaviour 
{
	[SerializeField] Transform target;
	[SerializeField] float _sensiblity = 5f;
	[SerializeField, Range(-1, 0)] float _minY = -0.7f;
	[SerializeField, Range(0, 1)] float _maxY = 0.7f;
	[SerializeField] float _centerYOffset = 0.25f;
	
	[SerializeField] float distance;
	[SerializeField] Vector2 rotation;

	void LateUpdate () 
	{
		if (target == null) return;
		rotation.x = _sensiblity * Input.GetAxisRaw("Mouse X");
		rotation.y += _sensiblity * Input.GetAxisRaw("Mouse Y") * Time.deltaTime;
		rotation.y = Mathf.Clamp(rotation.y, _minY, _maxY);

		Vector3 point = (transform.position - target.position).normalized;
		point = Quaternion.AngleAxis(rotation.x, Vector3.up) * point;
		// point = Quaternion.AngleAxis(rotation.y, Vector3.left) * point;
		point.y = -rotation.y;
		point.Normalize();
		// point.y = _pointYOffset;

		transform.position = target.position + (distance * point);
		transform.LookAt(target.position + (_centerYOffset * Vector3.up));
	}
}
