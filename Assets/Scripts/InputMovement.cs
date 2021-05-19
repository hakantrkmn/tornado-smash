using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputMovement : MonoBehaviour {


	public Vector3 startPos;
	public float power;

	void Update()
	{
        if (Input.GetMouseButtonDown(0))
        {
			startPos = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0))
        {
			var dir = (Input.mousePosition - startPos).normalized;
			transform.position = new Vector3(transform.position.x + (dir.x * power * Time.deltaTime), transform.position.y, transform.position.z + (dir.y * power*Time.deltaTime));
        }

	}
}
