using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
	private Vector3 movement, inputDir, cam;
	public float speed;

	public GameObject targetCamera;
	public float offset;
	private float deadZoneLeft, deadZoneRight, deadZoneUp, deadZoneBottom;

	public Animator animatorBody, animatorArms;
	public SpriteRenderer body, arms, map;

    void FixedUpdate()
	{
		inputDir = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
		movement = speed * Time.deltaTime * inputDir.normalized;

		this.GetComponent<Rigidbody2D>().MovePosition(this.transform.position + movement);
		MoveCamera(movement);



        if (inputDir.x > 0) { 
			body.GetComponent<SpriteRenderer>().flipX = true;
			arms.GetComponent<SpriteRenderer>().flipX = true;
		}
        else 
		{
			body.GetComponent<SpriteRenderer>().flipX = false;
			arms.GetComponent<SpriteRenderer>().flipX = false;
		}

		if(inputDir.y > 0)
        {
			arms.sortingOrder = 0;
			body.sortingOrder = 1;
		}
        else
        {
			arms.sortingOrder = 1;
			body.sortingOrder = 0;
		}

		animatorBody.SetFloat("horizontal", inputDir.x);
		animatorBody.SetFloat("vertical", inputDir.y);
		animatorBody.SetFloat("speed", inputDir.sqrMagnitude);

		animatorArms.SetFloat("horizontal", inputDir.x);
		animatorArms.SetFloat("vertical", inputDir.y);
		animatorArms.SetFloat("speed", inputDir.sqrMagnitude);

		animatorArms.SetBool("holding", this.GetComponent<Interaction>().state == State.EGG || this.GetComponent<Interaction>().state == State.PART);
	}

	private void MoveCamera(Vector3 movement)
    {
		cam = targetCamera.transform.position;
		deadZoneLeft = cam.x - offset;
		deadZoneRight = cam.x + offset;
		deadZoneBottom = cam.y - offset/2;
		deadZoneUp = cam.y + offset/2;

		float height = 2f * targetCamera.GetComponent<Camera>().orthographicSize;
		float width = height * targetCamera.GetComponent<Camera>().aspect;


		float leftBorder = map.transform.position.x - map.bounds.extents.x;
		float rightBorder = map.transform.position.x + map.bounds.extents.x;
		float bottomBorder = map.transform.position.y - map.bounds.extents.y;
		float upBorder = map.transform.position.y + map.bounds.extents.y;

		float camLeft = cam.x - width / 2;
		float camRight = cam.x + width / 2;
		float camUp = cam.y + height / 2;
		float camBottom = cam.y - height / 2;


		Debug.Log("cam" + camUp);

		if (camLeft>=leftBorder)
        {
			if (this.transform.position.x <= deadZoneLeft)	{ targetCamera.transform.position += new Vector3(movement.x, 0); }
        }
		if (camRight <= rightBorder) 
		{
			if (this.transform.position.x >= deadZoneRight)	{ targetCamera.transform.position += new Vector3(movement.x, 0); } 
		}
		if (camUp <= upBorder) 
		{
			if (this.transform.position.y >= deadZoneUp)	{ targetCamera.transform.position += new Vector3(0, movement.y); } 
		}
		if (camBottom >= bottomBorder) 
		{ 
			if (this.transform.position.y <= deadZoneBottom){ targetCamera.transform.position += new Vector3(0, movement.y); } 
		}
	}
}
