using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State{EMPTY, EGG, LIMB}

public class Interaction : MonoBehaviour
{
	[SerializeField] private bool isInRange;
	[SerializeField] private GameObject pnj;
	[SerializeField] private State state;

    private void Start()
    {
		isInRange = false;
		pnj = null;
		state = State.EMPTY;
    }

    private void Update()
	{
        if (Input.GetButtonDown("Interact"))
        {
			if (isInRange)
			{
				switch (state)
				{
					case State.EMPTY:
						Debug.Log("openShop()");
						break;

					case State.EGG:
						;
						break;

					case State.LIMB:
						;
						break;

					default:
						;
						break;
				}
			}
        }
	}

    public void OnTriggerEnter2D(Collider2D col)
    {
		if (!isInRange)
		{
			if (col.tag == "pnj")
			{
				isInRange = true;
				pnj = col.transform.gameObject;
				Debug.Log("collide");

			}
		}
    }

    public void OnTriggerExit2D(Collider2D col)
    {
		isInRange = false;
		pnj = null;
		Debug.Log("exit");
	}

}
