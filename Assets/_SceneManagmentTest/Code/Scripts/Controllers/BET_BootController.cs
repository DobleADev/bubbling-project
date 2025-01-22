using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DobleADev.BootEnemyTest
{
	[RequireComponent(typeof(Rigidbody)), RequireComponent(typeof(SphereCollider))]
	public class BET_BootController : MonoBehaviour
	{
		[SerializeField] BootJumpPattern[] _jumpPattern;
		[Header("Seek Settings")]
		[SerializeField] LayerMask _obstaclesLayer = 0;
		[SerializeField] float _foundTargetResponseDelay = 1.5f;
		[SerializeField] float _attackCooldown = 1f;
		[SerializeField] float _stunDuration = 2f;
		[SerializeField, Min(0.02f)] float _seekTimeStep = 0.1f;
		[SerializeField] float _sightLineStartHeightOffset = 0;
		[SerializeField] float _sightLineEndHeightOffset = 0;
		[SerializeField] float _skinWidth = 0.03f;
		[SerializeField, Range(0, 90)] float _groundSlopeLimit = 45;
		[Header("Default Events")]
		[SerializeField] DropdownUnityEventTransform _onTargetFound;
		[SerializeField] DropdownUnityEvent _onTargetLost;
		[SerializeField] DropdownUnityEvent _onStunStart;
		[SerializeField] DropdownUnityEvent _onStunLand;
		[SerializeField] DropdownUnityEvent _onStunEnd;
		[SerializeField] DropdownUnityEvent _onJumpBuildUp;
		[SerializeField] DropdownUnityEventVector3 _onJumpStart;
		[SerializeField] DropdownUnityEventFloat _whileJumping;
		[SerializeField] DropdownUnityEvent _onJumpEnd;
		[SerializeField] DropdownUnityEvent _onRecovery;
		[SerializeField] DropdownUnityEvent _onReset;
		[SerializeField] bool _enableLogs;
		bool _isAttacking, _isRecovered = true, _isAttackCoroutineRunning;
		Transform _transform, _target, _lastTarget;
		Vector3 _currentJumpDirection, _currentJumpStartPoint, _currentJumpEndPoint;
		Coroutine _attackRoutine;
		GameObject[] _detectedObjects;
		Rigidbody _physics;
		SphereCollider _collider;

		IEnumerator Start()
		{
			_transform = transform;
			_physics = GetComponent<Rigidbody>();
			_collider = GetComponent<SphereCollider>();
			_physics.hideFlags = HideFlags.HideAndDontSave;
			while (true)
			{
				if (!_isAttacking && _isRecovered)
				{
					Transform nextTarget = null;

					GameObject[] targetObjects = null;
					if (_detectedObjects != null)
					{
						targetObjects = _detectedObjects
							.Where(go => Physics.Linecast(
								transform.position + new Vector3(0, _sightLineStartHeightOffset, 0)
								, go.transform.position + new Vector3(0, _sightLineEndHeightOffset, 0)
								, _obstaclesLayer
								) == false)
							.ToArray();
					}

					if (targetObjects != null)
					{
						if (targetObjects.Length > 0)
						{
							nextTarget = targetObjects
								.OrderBy(go => Vector3.Distance(
									go.transform.position
									, _transform.position
									))
								.FirstOrDefault().transform;
						}
					}

					if (_target == null)
					{
						if (nextTarget != null)
						{
							SetTarget(nextTarget);
							_attackRoutine = StartCoroutine(AttackCoroutine());
						}
					}
					else
					{
						if (nextTarget != _target)
						{
							LoseTarget();
							if (nextTarget != null) SetTarget(nextTarget);
						}
						else
						{
							if (!_isAttackCoroutineRunning) _attackRoutine = StartCoroutine(AttackCoroutine());
						}
					}
					_detectedObjects = null;
				}
				yield return new WaitForSeconds(_seekTimeStep);
			}
		}

		void SetTarget(Transform nextTarget)
		{
			_target = nextTarget;
			_onTargetFound.Invoke(_target);
			ShowLog("Target found! - " + Time.time);
		}

		void LoseTarget()
		{
			_lastTarget = _target = null;
			_onTargetLost.Invoke();
			ShowLog("Target lost! - " + Time.time);
			if (_attackRoutine != null) StopCoroutine(_attackRoutine);
		}

		IEnumerator AttackCoroutine()
		{
			_isAttackCoroutineRunning = true;
			if (_lastTarget != _target)
			{
				yield return new WaitForSeconds(_foundTargetResponseDelay);
				_lastTarget = _target;
			}
			else
			{
				yield return new WaitForSeconds(_attackCooldown);
			}

			_isAttacking = true;
			_physics.isKinematic = true;
			ShowLog("Attack Phase Started.");
			_currentJumpDirection = (_target.position - _transform.position).normalized;
			for (int i = 0; i < _jumpPattern.Length; i++)
			{
				if (_jumpPattern[i].Properties == null) continue;
				var jump = _jumpPattern[i];

				_onJumpBuildUp.Invoke();
				jump.OnJumpBuildUp.Invoke();

				if (jump.Properties.AnticipationDuration > 0)
					yield return new WaitForSeconds(jump.Properties.AnticipationDuration);

				float progression = 0;
				float deltaDuration = 1 / jump.Properties.JumpDuration;

				_currentJumpStartPoint = _transform.position;
				Vector3 differencePosition = _target.position - _transform.position;
				_currentJumpDirection = Vector3.Slerp(_currentJumpDirection, differencePosition.normalized, jump.Properties.TargetRedirection);
				float jumpLength = jump.Properties.LengthType == BET_BootJump.JumpLengthType.Target ?
					differencePosition.magnitude : jump.Properties.LengthValue;
				_currentJumpEndPoint = _currentJumpStartPoint + (jumpLength * _currentJumpDirection);
				_currentJumpEndPoint.y = _target.position.y;
				_onJumpStart.Invoke(_currentJumpEndPoint);
				jump.OnJumpStart.Invoke(_currentJumpEndPoint);
				ShowLog("Jumping!");

				while (progression < 1)
				{
					Vector3 currentPoint = Vector3.Lerp(
						_currentJumpStartPoint
						, _currentJumpEndPoint
						, jump.Properties.Progression.Evaluate(progression)
						);

					float currentHeight = Mathf.Lerp(
						0, jump.Properties.MaxHeight
						, jump.Properties.Height.Evaluate(progression)
					);

					Vector3 nextPosition = currentPoint + currentHeight * Vector3.up;
					Vector3 jumpVelocity = nextPosition - _transform.position;

					if (jumpVelocity.y < 0 && GroundCheck()) 
					{
						Debug.LogWarning("Jump end by grounded");
						break;
					}
					_physics.MovePosition(nextPosition);

					_whileJumping.Invoke(progression);
					jump.WhileJumping.Invoke(progression);
	
					CheckObstacleInterruption(jumpVelocity);
					progression += deltaDuration * Time.deltaTime;
					yield return new WaitForFixedUpdate();
				}
				if (progression >= 1) _physics.MovePosition(_currentJumpEndPoint); // DONT ENDED TILL IT ENDS
				_onJumpEnd.Invoke();
				jump.OnJumpEnd.Invoke();
				ShowLog("Landed!");

				yield return new WaitForSeconds(jump.Properties.LandingDuration);

				if (jump.RecoveryDuration > 0)
				{
					_onRecovery.Invoke();
					jump.OnRecovery.Invoke();
					ShowLog("Recovered!");
					yield return new WaitForSeconds(jump.RecoveryDuration);
				}
			}
			_isAttacking = false;
			_physics.isKinematic = false;
			_onReset.Invoke();
			ShowLog("Attack Phase Ended.");
			_isAttackCoroutineRunning = false;
		}

		IEnumerator InterruptionCoroutine(Vector3 clashVelocity)
		{
			_isRecovered = false;
			_physics.AddForce(clashVelocity, ForceMode.Impulse);
			_onStunStart.Invoke();
			bool isGrounded = GroundCheck();
			ShowLog("Ouch");

			bool wasOnAir = false;
			while (!isGrounded)
			{
				wasOnAir = true;
				isGrounded = GroundCheck();
				yield return new WaitForFixedUpdate();
			}
			if (wasOnAir)
			{
				_onStunLand.Invoke();
				ShowLog("Landed in interruption");
			}
			yield return new WaitForSeconds(_stunDuration);

			_isRecovered = true;
			_onStunEnd.Invoke();
			ShowLog("Recovered");
		}

		private void OnValidate()
		{
			for (int i = 0; i < _jumpPattern.Length; i++)
			{
				_jumpPattern[i].Name = _jumpPattern[i].Properties == null ?
					"Not assigned yet"
					: (i + 1) + ". " + _jumpPattern[i].Properties.name;
			}
		}

		bool GroundCheck(float magnitude = 0)
		{
			bool grounded = Physics.SphereCast(
				_transform.TransformPoint(_collider.center),
				_collider.radius - _skinWidth,
				Vector3.down,
				out RaycastHit groundInfo, 
				(2 * _skinWidth) + magnitude
				, _obstaclesLayer,
				QueryTriggerInteraction.Ignore);
			if (grounded) ShowLog("Grounded Detection angle: " + Vector3.Angle(groundInfo.normal, Vector3.up));
			return grounded && Vector3.Angle(groundInfo.normal, Vector3.up) <= _groundSlopeLimit;
		}

		void CheckObstacleInterruption(Vector3 velocity)
		{
			float speed = velocity.magnitude;
			if (Physics.SphereCast(
				_transform.TransformPoint(_collider.center),
				_collider.radius - _skinWidth,
				velocity,
				out RaycastHit hitInfo, 
				velocity.magnitude + _skinWidth
				, _obstaclesLayer,
				QueryTriggerInteraction.Ignore)
			&& Vector3.Angle(hitInfo.normal, Vector3.up) > _groundSlopeLimit)
			{
				Vector3 reflectDirection = Vector3.Reflect(velocity, hitInfo.normal);
				reflectDirection.y = 0;
				ShowLog("Interruption Detection angle: " + Vector3.Angle(hitInfo.normal, Vector3.up));
				Interrupt(speed * reflectDirection.normalized);
			}
		}
		
		void ShowLog(string message) 
		{
			if (_enableLogs) Debug.Log(message);
		}
		public void DetectObjects(GameObject[] detectedObjects)
		{
			_detectedObjects = detectedObjects;
		}

		public void Interrupt() {
			Interrupt(Vector3.zero);
		}

		public void Interrupt(Vector3 velocity)
		{
			if (_isAttackCoroutineRunning)
			{
				StopCoroutine(_attackRoutine);
				_isAttackCoroutineRunning = false;
				_isAttacking = false;
				_physics.isKinematic = false;
			}
			StartCoroutine(InterruptionCoroutine(velocity));
		}

		public Vector3 currentJumpStartPoint { get { return _currentJumpStartPoint; } }
		public Vector3 currentJumpEndPoint { get { return _currentJumpEndPoint; } }
	}
}

[System.Serializable]
public struct BootJumpPattern
{
	[HideInInspector] public string Name;
	public BET_BootJump Properties;
	public float RecoveryDuration;
	[Header("Additive Events")]
	public DropdownUnityEvent OnJumpBuildUp;
	public DropdownUnityEventVector3 OnJumpStart;
	public DropdownUnityEventFloat WhileJumping;
	public DropdownUnityEvent OnJumpEnd;
	public DropdownUnityEvent OnRecovery;
}