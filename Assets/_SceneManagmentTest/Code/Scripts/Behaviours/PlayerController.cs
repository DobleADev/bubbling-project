using UnityEngine;

namespace DobleADev
{
	
	public class PlayerController : MonoBehaviour 
	{
		[SerializeField] KinematicBody _physics;
		[SerializeField] Transform _forwardReference;
		[SerializeField] float walkSpeed = 6;
		[SerializeField] float groundAcceleration = 80;
		[SerializeField] float airAcceleration = 40;
		[SerializeField] float _jumpSpeed = 6;
		[SerializeField] float _maxJumpTime = 0.25f;
		float _currentSpeed;
		float _currentAcceleration;
		float _currentJumpTime;
		Vector3 _currentWalkVelocity;

		public enum PlayerState 
		{
			Idle,
			Jumping,
			Falling,
			LedgeGrabbing
		}
		[SerializeField] PlayerState _currentState = PlayerState.Idle;

		private void Start() 
		{
			_currentSpeed = walkSpeed;
			_currentJumpTime = 0;
		}
		
		void Update () 
		{
			switch (_currentState)
			{
				case PlayerState.Idle:
				{
					_currentAcceleration = groundAcceleration;
					if (_physics.isGrounded && Input.GetButtonDown("Jump"))
					{
						_currentState = PlayerState.Jumping;
					}

					if (!_physics.isGrounded)
					{
						_currentState = PlayerState.Falling;
					}
				} break;

				case PlayerState.Jumping:
				{
					_currentAcceleration = airAcceleration;
					_currentJumpTime += Time.deltaTime;
					_physics.gravityVelocity = _jumpSpeed * Vector3.up;
					if (Input.GetButtonUp("Jump") || _currentJumpTime >= _maxJumpTime)
					{
						_currentState = PlayerState.Falling;
						_currentJumpTime = 0;
					}

				} break;

				case PlayerState.Falling:
				{
					_currentAcceleration = airAcceleration;
					if (_physics.isGrounded)
					{
						_currentState = PlayerState.Idle;
					}

				} break;

				case PlayerState.LedgeGrabbing:
				{
					
				} break;
			}
			Walk();
			
			
			
		}

		void Walk()
		{
			Vector3 forwardDirection = _forwardReference == null ? Vector3.forward : _forwardReference.forward;
			forwardDirection.y = 0;
			forwardDirection.Normalize();
			Vector3 rightDirection = Vector3.Cross(forwardDirection, Vector3.down);
			Vector3 moveDirection = _currentSpeed * Vector3.ClampMagnitude((Input.GetAxisRaw("Horizontal") * rightDirection) + (Input.GetAxisRaw("Vertical") * forwardDirection), 1);
			_currentWalkVelocity = Vector3.MoveTowards(_currentWalkVelocity, moveDirection, _currentAcceleration * Time.deltaTime);
			_physics.velocity = _currentWalkVelocity;
		}
		
	}

}