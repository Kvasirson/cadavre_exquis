using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
	private Vector3 movement, inputDir;
	public float speed;

	public GameObject camera;
	public float offset, leftBorder, rightBorder;
	private float deadZoneLeft, deadZoneRight, camX;

    void FixedUpdate()
	{
		inputDir = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
		movement = inputDir.normalized * speed * Time.deltaTime;

		this.transform.position = this.transform.position + movement;
		MoveCamera(movement);
	}

	private void MoveCamera(Vector3 movement)
    {
		camX = camera.transform.position.x;
		deadZoneLeft = camX - offset;
		deadZoneRight = camX + offset;

		float height = 2f * camera.GetComponent<Camera>().orthographicSize;
		float width = height * camera.GetComponent<Camera>().aspect;


		if (camX - width / 2 >= leftBorder)
		{
			if (this.transform.position.x <= deadZoneLeft)
			{
				camera.transform.position += movement;
			}
        }

		if(camX + width / 2 <= rightBorder)
        {
			if (this.transform.position.x >= deadZoneRight)
			{
				camera.transform.position += movement;
			}
		}
	}
}
