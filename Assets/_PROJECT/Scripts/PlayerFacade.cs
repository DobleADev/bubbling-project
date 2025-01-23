using System.Collections;
using System.Collections.Generic;
using DobleADev.Scriptables.Variables;
using UnityEngine;
using UnityEngine.UI;

public class PlayerFacade : MonoBehaviour 
{
	[Header("Dependencies")]
	[SerializeField] Rigidbody _body;
	[SerializeField] Slider _oxygenSlider;
	[SerializeField] Transform _face;
	[SerializeField] FloatScriptableVariable _oxygenDrainMultiplier;
	[SerializeField] string _draggableTag = "Dragger";
	[SerializeField] string _oxygenTag = "Oxygen";
	[Header("Properties")]
	[SerializeField] float _speed = 4;
	[SerializeField] float _baseGravity = -0.05f;
	[SerializeField] float _defaultMaxOxygen = 100f;
	[SerializeField] float _defaultOxygenDrainPerSecond = 1f;
	[SerializeField] float _defaultOxygenGain = 10f;
	[SerializeField] float _defaultPhysicsDrag = 2.5f;
	[SerializeField] float _seaweedDragPressure = 1f;
	[SerializeField] bool _startWithGravity = true;
	[SerializeField] bool _startWithDrain = true;
	[SerializeField, Range(0, 1)] float _faceFrontLook = 0.25f;
	[Header("Events")]
	[SerializeField] DropdownUnityEvent _onEmptyOxygen;
	private Vector2 moveInput;
	private Vector3 faceDirection = Vector3.right;
	private bool _inputEnabled = true, _gravityEnabled = false, _drainEnabled = false, _isBeingDragged;
	public bool inputEnabled { get { return _inputEnabled; } set { _inputEnabled = value; moveInput = default; } }
	public bool gravityEnabled { get { return _gravityEnabled; } set { _gravityEnabled = value; } }
	public bool drainEnabled { get { return _drainEnabled; } set { _drainEnabled = value; } }

	void Start()
	{
		if (_startWithGravity) gravityEnabled = true;
		if (_startWithDrain) drainEnabled = true;
		_oxygenSlider.value = _oxygenSlider.maxValue = _defaultMaxOxygen;
		if (_face != null)
		{
			faceDirection = Vector3.Lerp(Vector3.right, Vector3.back, _faceFrontLook);
			_face.rotation = Quaternion.LookRotation(-faceDirection);
		}
	}

	void Update () 
	{
		if (_inputEnabled)
		{
			moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
			moveInput = Vector2.ClampMagnitude(moveInput, 1);
		}

		if (_drainEnabled == true && _oxygenSlider.value > 0) 
		{
			_oxygenSlider.value -= Time.deltaTime * _oxygenDrainMultiplier.GetValueTyped() * _defaultOxygenDrainPerSecond;
			if (_oxygenSlider.value <= 0)
			{
				_onEmptyOxygen?.Invoke();
			}
		}

		if (_face != null)
		{
			if (moveInput != Vector2.zero) faceDirection = Vector3.Lerp(moveInput, Vector3.back, _faceFrontLook);
			_face.rotation = Quaternion.RotateTowards(_face.rotation, Quaternion.LookRotation(-faceDirection), Time.deltaTime * 200);
		}
	}
	
	void FixedUpdate () 
	{
		if (_isBeingDragged)
		{
			_body.drag = _defaultPhysicsDrag + _seaweedDragPressure;
		}
		else _body.drag = _defaultPhysicsDrag;
		_body.AddForce(Time.deltaTime * _speed * moveInput);
		if (_gravityEnabled) _body.AddForce(Time.deltaTime * _baseGravity * Vector2.down);
		_isBeingDragged = false;
	}

	private void OnTriggerStay(Collider other) {
		if (other.CompareTag(_draggableTag))
		{
			_isBeingDragged = true;
		}

		if (other.CompareTag(_oxygenTag))
		{
			_oxygenSlider.value += Time.deltaTime * _defaultOxygenDrainPerSecond;
		}
	}

	public void RechargeOxygen()
	{
		_oxygenSlider.value += _defaultOxygenGain * 2f;
	}

}
