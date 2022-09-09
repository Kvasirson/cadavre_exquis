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

	public Animator animatorBody, animatorArms;
	public SpriteRenderer body, arms;

    void FixedUpdate()
	{
		inputDir = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
		movement = speed * Time.deltaTime * inputDir.normalized;

		this.transform.position = this.transform.position + movement;
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
		camX = targetCamera.transform.position.x;
		deadZoneLeft = camX - offset;
		deadZoneRight = camX + offset;

		float height = 2f * targetCamera.GetComponent<Camera>().orthographicSize;
		float width = height * targetCamera.GetComponent<Camera>().aspect;


		if (camX - width / 2 >= leftBorder)
		{
			if (this.transform.position.x <= deadZoneLeft)
			{
				targetCamera.transform.position += movement;
			}
        }

		if(camX + width / 2 <= rightBorder)
        {
			if (this.transform.position.x >= deadZoneRight)
			{
				targetCamera.transform.position += movement;
			}
		}
	}
}
