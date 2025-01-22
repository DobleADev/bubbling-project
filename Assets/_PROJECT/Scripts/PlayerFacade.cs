using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFacade : MonoBehaviour 
{
	[SerializeField] Rigidbody2D _body;
	[SerializeField] float _baseGravity = -0.05f;
	[SerializeField] bool _startWithGravity = true;
	[SerializeField] float _speed = 4;
	private Vector2 moveInput;
	private bool _inputEnabled = true, _gravityEnabled = false;
	public bool inputEnabled { get { return _inputEnabled; } set { _inputEnabled = value; moveInput = default; } }
	public bool gravityEnabled { get { return _gravityEnabled;} set { _body.gravityScale = (_gravityEnabled = value) == true? _baseGravity : 0; } }

	void Start()
	{
		if (_startWithGravity) gravityEnabled = true;
	}

	void Update () 
	{
		if (_inputEnabled)
		{
			moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
			moveInput = Vector2.ClampMagnitude(moveInput, 1);
		}
	}
	
	void FixedUpdate () 
	{
		_body.AddForce(Time.deltaTime * _speed * moveInput);
	}
}
