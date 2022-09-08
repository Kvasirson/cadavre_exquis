using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
	private Vector3 movement, inputDir;
	public float speed;

	public GameObject targetCamera;
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
		camX = targetCamera.transform.position.x;
		deadZoneLeft = camX - offset;
		deadZoneRight = camX + offset;

		float height = 2f * targetCamera.GetComponent<Camera>().orthographicSize;
		float width = height * targetCamera.GetComponent<Camera>().aspect;


		if (camX - width / 2 >= leftBorder)
		{
			if (this.transform.position.x <= deadZoneLeft)
			{
				targetCamera.transform.position += new Vector3(movement.x, 0);
			}
        }

		if(camX + width / 2 <= rightBorder)
        {
			if (this.transform.position.x >= deadZoneRight)
			{
				targetCamera.transform.position += new Vector3(movement.x, 0);
			}
		}
	}
}
