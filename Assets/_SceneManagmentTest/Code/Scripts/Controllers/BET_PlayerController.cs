using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DobleADev.BootEnemyTest
{
	public class BET_PlayerController : MonoBehaviour 
	{
		[SerializeField] KinematicBody body;
		[SerializeField] float moveSpeed = 5;
		Vector2 moveInput;
		public void Move(Vector2 moveInput)
		{
			// Debug.Log("moveInput: " + moveInput);
			this.moveInput += moveInput;
			this.moveInput = Vector2.ClampMagnitude(this.moveInput, 1);
		}

		void Update () 
		{
			Move(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")));
			Vector3 moveDirection = (moveInput.y * Vector3.forward) + (moveInput.x * Vector3.right);
			body.velocity = moveSpeed * moveDirection;
		}

		private void LateUpdate() 
		{
			moveInput = Vector2.zero;
		}
	}
}
