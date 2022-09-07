using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
	private Vector3 movement;
	public float speed;


    void FixedUpdate()
	{
		movement = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

		this.transform.position = this.transform.position + movement.normalized * speed * Time.deltaTime;
	}
}
