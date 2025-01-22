using UnityEngine;

namespace DobleADev
{
	[RequireComponent(typeof(Rigidbody))]
	public class KinematicBody : MonoBehaviour
	{
		[SerializeField] float _radius = 0.5f;
		[SerializeField] float _height = 2f;
		[SerializeField] Vector3 _center = Vector3.zero;
		[SerializeField] float _gravityScale = 1;
		[SerializeField] float _groundSnapThreshold = 0.5f;
		[SerializeField] LayerMask layerMask = ~0;
		Transform _transform;
		Rigidbody _rigidbody;
		Vector3 _moveVelocity, _gravityVelocity, _snapVelocity;
		Collider _mainCollider;
		RaycastHit _groundedInfo;
		public float radius { get; set; }
		public float height { get; set; }
		public Vector3 velocity { get { return _moveVelocity; } set { _moveVelocity = value; } }
		public Vector3 gravityVelocity { get { return _gravityVelocity; } set { _gravityVelocity = value; } }
		public bool isGrounded { get; private set; }
		Vector3 collisionPosition { get { return _transform.TransformPoint(_center); } }

		void Start()
		{
			_transform = transform;
			_rigidbody = GetComponent<Rigidbody>();
			_mainCollider = GetComponent<Collider>();

		}

		void FixedUpdate()
		{
			Move(_moveVelocity);
			HandleOverlap();
		}

		void Move(Vector3 moveAmount)
		{
			_gravityVelocity += _gravityScale * Time.deltaTime * Physics.gravity;
			_snapVelocity = Vector3.zero;
			if (isGrounded)
			{
				if (_gravityVelocity.y < 0) _gravityVelocity = Vector3.zero;
			}
			moveAmount = CollideAndSlideMethod(Time.deltaTime * moveAmount, collisionPosition, 0, false, Time.deltaTime * moveAmount);

			Vector3 capsuleHeight = ((0.5f * _height) - _radius) * Vector3.up;
			Physics.SphereCast(new Ray(collisionPosition - capsuleHeight + (skinWidth * Vector3.up), Vector3.down), _radius, out _groundedInfo, skinWidth + 0.002f);
			isGrounded = _groundedInfo.collider != null && Vector3.Angle(_groundedInfo.normal, Vector3.up) <= maxSlopeAngle;
			if (isGrounded && moveAmount.y <= 0
			&& Physics.SphereCast(new Ray(collisionPosition + (((0.5f * _height) - (0.5f * _radius)) * Vector3.down), Vector3.down), 0.5f * _radius, _groundSnapThreshold + skinWidth)
			&& Physics.SphereCast(new Ray(collisionPosition - capsuleHeight + (skinWidth * Vector3.up) + (_radius * _moveVelocity.normalized), Vector3.down), _radius, skinWidth + 0.1f + _groundSnapThreshold) && gravityVelocity.y <= 0) _snapVelocity = _groundSnapThreshold * Vector3.down;
			// _snapVelocity = _groundSnapThreshold * Vector3.down;
			moveAmount += CollideAndSlideMethod((Time.deltaTime * _gravityVelocity) + _snapVelocity, collisionPosition + moveAmount, 0, true, (Time.deltaTime * _gravityVelocity) + _snapVelocity);
			_rigidbody.MovePosition(transform.position + moveAmount);
		}

		int maxBounces = 5;
		[SerializeField] float skinWidth = 0.015f;
		[SerializeField] float maxSlopeAngle = 55;
		private Vector3 CollideAndSlideMethod(Vector3 vel, Vector3 pos, int depth, bool gravityPass, Vector3 velInit)
		{
			if (depth >= maxBounces)
			{
				return Vector3.zero;
			}
			float dist = vel.magnitude + skinWidth;
			Vector3 capsuleHeight = ((0.5f * _height) - _radius) * Vector3.up;

			RaycastHit hit;
			// if (Physics.SphereCast(pos, bounds.extents.x, vel.normalized, out hit, dist, layerMask))
			if (Physics.CapsuleCast(pos - capsuleHeight, pos + capsuleHeight, _radius - skinWidth, vel.normalized, out hit, dist, layerMask, QueryTriggerInteraction.Ignore))
			{
				Vector3 snapToSurface = vel.normalized * (hit.distance - skinWidth);
				Vector3 leftover = vel - snapToSurface;
				float angle = Vector3.Angle(Vector3.up, hit.normal);

				if (snapToSurface.magnitude <= skinWidth && (!gravityPass || (gravityPass && vel.sqrMagnitude < 0)))
				{
					snapToSurface = Vector3.zero;
				}

				if (angle <= maxSlopeAngle) // normal ground / slope
				{

					if (gravityPass)
					{
						return snapToSurface;
					}
					leftover = ProjectAndScale(leftover, hit.normal);
				}
				else // wall or steep slope
				{
					float scale = 1 - Vector3.Dot(
						new Vector3(hit.normal.x, 0, hit.normal.z).normalized,
						-new Vector3(velInit.x, 0, velInit.z).normalized
					);

					if (isGrounded && !gravityPass)
					{
						leftover = ProjectAndScale(
							new Vector3(leftover.x, 0, leftover.z),
							new Vector3(hit.normal.x, 0, hit.normal.z).normalized);
						leftover *= scale;
					}
					else
					{
						leftover = scale * ProjectAndScale(leftover, hit.normal);
					}
				}

				return snapToSurface + CollideAndSlideMethod(leftover, pos + snapToSurface, depth + 1, gravityPass, velInit);
			}
			return vel;
		}

		Vector3 ProjectAndScale(Vector3 vec, Vector3 normal)
		{
			float mag = vec.magnitude;
			vec = Vector3.ProjectOnPlane(vec, normal).normalized;
			vec *= mag;
			return vec;
		}

		void HandleOverlap()
		{
			Vector3 overlapDirection;
			float overlapDistance;
			Vector3 pos = collisionPosition;
			Vector3 capsuleHeight = ((0.5f * _height) - _radius) * Vector3.up;
			Collider[] maxCollidersOverlap = new Collider[5];
			if (Physics.OverlapCapsuleNonAlloc(pos - capsuleHeight, pos + capsuleHeight, _radius - skinWidth, maxCollidersOverlap, layerMask, QueryTriggerInteraction.Ignore) > 0)
			{
				for (int i = 0; i < maxCollidersOverlap.Length; i++)
				{
					Collider overlapInfo = maxCollidersOverlap[i];
					if (overlapInfo == null) continue;
					Transform collisionTransform = overlapInfo.transform;
				Physics.ComputePenetration(
					_mainCollider,
					_transform.position,
					_transform.rotation,
					overlapInfo,
					collisionTransform.position,
					collisionTransform.rotation,
					out overlapDirection,
					out overlapDistance);
					_transform.position += overlapDistance * overlapDirection;
				}
			}
		}

		private void OnDrawGizmos() {
			Vector3 overlapDirection;
			float overlapDistance;
			Vector3 pos = collisionPosition;
			Vector3 capsuleHeight = ((0.5f * _height) - _radius) * Vector3.up;
			Collider[] maxCollidersOverlap = new Collider[5];
			if (Physics.OverlapCapsuleNonAlloc(pos - capsuleHeight, pos + capsuleHeight, _radius - skinWidth, maxCollidersOverlap, layerMask, QueryTriggerInteraction.Ignore) > 0)
			{
				Gizmos.color = Color.gray;
				Gizmos.DrawSphere(pos - capsuleHeight, _radius - skinWidth);
				Gizmos.DrawSphere(pos + capsuleHeight, _radius - skinWidth);

				Gizmos.color = Color.yellow;
				for (int i = 0; i < maxCollidersOverlap.Length; i++)
				{
					Collider overlapInfo = maxCollidersOverlap[i];
					if (overlapInfo == null) continue;
					Transform collisionTransform = overlapInfo.transform;
				Physics.ComputePenetration(
					_mainCollider,
					_transform.position,
					_transform.rotation,
					overlapInfo,
					collisionTransform.position,
					collisionTransform.rotation,
					out overlapDirection,
					out overlapDistance);

					
                	Gizmos.DrawRay(_transform.position, 2 * overlapDirection);
				}
			}
			// transform.position += overlapDistance * overlapDirection;
		}
	}
}
