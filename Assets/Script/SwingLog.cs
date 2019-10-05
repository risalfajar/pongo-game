using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingLog : MonoBehaviour {

	public float rotatingSpeed;
	public float maxDegree;

	private Vector3 dir = Vector3.forward;
	private float minDegree;

	void Start()
	{
		minDegree = 360 - maxDegree;
	}

	void Update () {
		transform.RotateAround(transform.position, dir, rotatingSpeed * Time.deltaTime);

		if(transform.eulerAngles.z >= maxDegree && transform.eulerAngles.z <= 180) //max
		{
			dir = Vector3.back;
		}else if(transform.eulerAngles.z <= minDegree && transform.eulerAngles.z >= 180){ //min
			dir = Vector3.forward;
		}
	}
}
