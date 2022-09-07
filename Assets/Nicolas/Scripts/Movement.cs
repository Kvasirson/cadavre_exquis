using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
	private Vector3 movement, inputDir;
	public float speed;

	public GameObject camera;
	public float offset;
	private float deadZoneLeft, deadZoneRight;


    void FixedUpdate()
	{
		inputDir = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
		movement = inputDir.normalized * speed * Time.deltaTime;

		this.transform.position = this.transform.position + movement;
		MoveCamera(movement);
	}

	private void MoveCamera(Vector3 movement)
    {
		deadZoneLeft = camera.transform.position.x - offset;
		deadZoneRight = camera.transform.position.x + offset;

		if(this.transform.position.x <= deadZoneLeft)
        {
			camera.transform.position += movement;
        }
		else if(this.transform.position.x >= deadZoneRight)
        {
			camera.transform.position += movement;
		}
    }
}
