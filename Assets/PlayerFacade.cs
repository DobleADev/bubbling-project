using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFacade : MonoBehaviour 
{
	[SerializeField] Rigidbody2D _body;
	[SerializeField] float _speed = 4;
	private Vector2 moveInput;

	void Update () 
	{
		moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
		moveInput = Vector2.ClampMagnitude(moveInput, 1);
	}
	
	void FixedUpdate () 
	{
		_body.AddForce(Time.deltaTime * _speed * moveInput);
	}
}
